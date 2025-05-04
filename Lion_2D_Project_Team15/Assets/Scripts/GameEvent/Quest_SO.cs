using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest_SO", menuName = "Scriptable Objects/Quest_SO")]
public class Quest_SO : ScriptableObject
{
    public string QuestID;
    public QuestInfo[] Progress;

    public void SetTrigger(int i)
    {
        Progress[i].Trigger.AddEventTrigger();
    }

    public void RemoveTrigger(int i)
    {
        Progress[i].Trigger.AddEventTrigger();
    }

    [Serializable]
    public struct QuestInfo
    {
        public EventTrigger_SO Trigger;
        public GameEvent_SO GameEvent;
    }
}
