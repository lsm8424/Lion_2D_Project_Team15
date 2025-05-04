using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest_SO", menuName = "Scriptable Objects/Quest_SO")]
public class Quest_SO : ScriptableObject
{
    public string QuestID;
    public QuestInfo[] Progress;

    public void SetTrigger(int i)
    {
        foreach (var item in Progress[i].Triggers)
            item.AddEventTrigger();
    }

    public void RemoveTrigger(int i)
    {
        foreach (var item in Progress[i].Triggers)
            item.RemoveTrigger();
    }

    [Serializable]
    public struct QuestInfo
    {
        public EventTrigger_SO[] Triggers;
        public GameEvent_SO GameEvent;
    }
}
