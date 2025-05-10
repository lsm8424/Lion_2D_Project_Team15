using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public Dictionary<string, Quest_SO> Quests { get; private set; } = new();
    public Dictionary<string, int> Progresses { get; private set; } = new();

    // ✅ 중복 실행 방지용 상태 캐시
    private HashSet<string> _activeQuests = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartQuest(string questID)
    {
        if (!Quests.TryGetValue(questID, out var quest))
        {
            Debug.LogError($"유효한 Quest ID가 아닙니다. QuestID: {questID}");
            return;
        }

        if (Progresses.ContainsKey(questID))
        {
            Debug.LogWarning($"[QuestManager] '{questID}'는 이미 시작된 퀘스트입니다.");
            return;
        }

        Progresses.Add(questID, 0);
        quest.SetTrigger(0);
    }

    public IEnumerator AdvanceProgress(string questID)
    {
        if (!Quests.TryGetValue(questID, out var quest))
        {
            Debug.LogError($"[QuestManager] 잘못된 QuestID: {questID}");
            yield break;
        }

        if (_activeQuests.Contains(questID))
        {
            Debug.LogWarning($"[QuestManager] '{questID}'는 이미 실행 중입니다. 중복 방지됨.");
            yield break;
        }

        _activeQuests.Add(questID);

        var questProgress = quest.Progress;
        int currentProgress = Progresses.ContainsKey(questID) ? Progresses[questID] : 0;

        if (currentProgress >= questProgress.Length)
        {
            Debug.Log($"[QuestManager] 퀘스트 '{questID}'는 이미 완료되었습니다.");
            _activeQuests.Remove(questID);
            yield break;
        }

        Debug.Log($"[QuestManager] Executing Progress[{currentProgress}]");

        quest.RemoveTrigger(currentProgress);

        var currentEvent = questProgress[currentProgress].GameEvent;
        if (currentEvent == null)
        {
            Debug.LogError($"[QuestManager] Progress[{currentProgress}]의 GameEvent가 null입니다.");
            _activeQuests.Remove(questID);
            yield break;
        }

        yield return currentEvent.Execute();

        SaveManager.Instance.Save();
        
        int nextProgress = currentProgress + 1;
        Progresses[questID] = nextProgress;
        Debug.Log($"[QuestManager] 다음 단계로: Progress[{nextProgress}]");

        if (nextProgress >= questProgress.Length)
        {
            Debug.Log($"[QuestManager] 퀘스트 '{questID}' 완료!");
            _activeQuests.Remove(questID);
            yield break;
        }

        quest.SetTrigger(nextProgress);
        _activeQuests.Remove(questID);
    }

    public void SetUp(string path)
    {
        // EventManager가 아직 초기화되지 않았다면 먼저 세팅
        if (!EventManager.Instance.DidSetup)
        {
            EventManager.Instance.SetupEvents(path);
        }

        Quest_SO[] quests = Resources.LoadAll<Quest_SO>($"GameEvent/{path}");

        for (int i = 0; i < quests.Length; ++i)
        {
            string id = quests[i].QuestID;
            Quests.Add(id, quests[i]);
        }
    }

    public void OnTriggerComplete(string questID, int progress)
    {
        if (!Quests.ContainsKey(questID))
        {
            Debug.LogError("유효하지 않은 QuestID: " + questID);
            return;
        }

        // ✅ 항상 등록 또는 갱신
        Progresses[questID] = progress;
        Debug.Log($"[QuestManager] OnTriggerComplete → QuestID: {questID}, Progress: {progress}");

        StartCoroutine(AdvanceProgress(questID));
    }
}
