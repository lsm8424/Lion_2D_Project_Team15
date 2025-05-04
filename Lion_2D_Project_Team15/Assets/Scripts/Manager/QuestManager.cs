using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.MPE;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public Dictionary<string, Quest_SO> Quests { get; private set; } = new();
    public Dictionary<string, int> Progresses { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartQuest(string questID)
    {
        if (!Quests.TryGetValue(questID, out var quest))
        {
            Debug.LogError($"유효한 Quest ID가 아닙니다. EventID: {questID}");
            return;
        }

        if (Progresses.ContainsKey(questID))
        {
            Debug.LogError("이미 진행상황이 존재합니다 Progress: " + Progresses[questID]);
        }

        Progresses.Add(questID, 0);
        quest.SetTrigger(0);
    }

    public IEnumerator AdvanceProgress(string questID)
    {
        if (!Quests.ContainsKey(questID))
        {
            Debug.LogError("유효하지 않은 QuestID: " + questID);
            yield break;
        }

        var questProgress = Quests[questID].Progress;
        int progressLevel = 0;  // default - 시작하지 않은 이벤트인 경우

        // 이전 이벤트 트리거 해제 및 실행
        if (Progresses.ContainsKey(questID))
        {
            int prevProgressLevel = Progresses[questID];

            if (questProgress.Length <= prevProgressLevel)
            {
                Debug.LogError("이 퀘스트는 이미 종료되었습니다. QuestID: " + questID);
                yield break;
            }

            foreach (var prevTriggers in questProgress[prevProgressLevel].Triggers)
                prevTriggers.RemoveTrigger();

            progressLevel = prevProgressLevel + 1;
            Progresses[questID] = progressLevel;
            yield return questProgress[prevProgressLevel].GameEvent.Execute();
        }

        // 만약 마지막 이벤트였다면
        if (questProgress.Length <= progressLevel)
            yield break;


        foreach (var curTrigger in questProgress[progressLevel].Triggers)
        {
            curTrigger.AddEventTrigger();
        }
    }

    public void SetUp(string path)
    {
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

        // 시작되지 않은 퀘스트인 경우
        if (Progresses.ContainsKey(questID))
            return;

        StartCoroutine(AdvanceProgress(questID));
    }
}
