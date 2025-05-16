using System;
using System.Collections;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "Quest_SO", menuName = "Scriptable Objects/Quest_SO")]
public class Quest_SO : ScriptableObject
{
    public string QuestID;
    public QuestInfo[] Progress;

    public void SetTrigger(int i)
    {
        if (i >= Progress.Length)
        {
            Debug.LogError($"[Quest_SO] Index {i}가 Progress 배열 범위를 벗어났습니다.");
            return;
        }

        var progress = Progress[i];

        if (progress.Trigger == null)
        {
            Debug.Log($"[Quest_SO] Progress[{i}]에는 Trigger가 없으므로 자동 실행합니다.");
            QuestManager.Instance.StartCoroutine(AutoAdvance(i));
            return;
        }

        progress.Trigger.QuestID = QuestID;
        progress.Trigger.ProgressLevel = i;
        progress.Trigger.SetUp();
        progress.Trigger.AddEventTrigger();
    }

    public void RemoveTrigger(int i)
    {
        if (i >= Progress.Length)
            return;

        var trigger = Progress[i].Trigger;
        if (trigger != null)
            trigger.RemoveTrigger();
    }

    private IEnumerator AutoAdvance(int i)
    {
        var gameEvent = Progress[i].GameEvent;
        if (gameEvent == null)
        {
            Debug.LogError($"[Quest_SO] Progress[{i}]의 GameEvent가 null입니다.");
            yield break;
        }

        yield return gameEvent.Execute();

        if (Progress[i].ShouldSave)
        {
            Debug.Log("Save in AutoAdvance. Progress Level이 제대로 저장안될 가능성이 있음.");
            QuestManager.Instance.Progresses[QuestID] = i + 1;
            SaveManager.Instance.Save();
        }

        // 직접 다음 진행도로 넘어가도록 수정
        int nextProgress = i + 1;
        if (nextProgress < Progress.Length)
        {
            SetTrigger(nextProgress);
        }
    }

    [Serializable]
    public struct QuestInfo
    {
        public EventTrigger_SO Trigger;
        public GameEvent_SO GameEvent;
        public bool ShouldSave;
    }
}
