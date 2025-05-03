using UnityEngine;
using System;
using System.Reflection;
using System.Data;

[CreateAssetMenu(fileName = "EventTrigger_SO", menuName = "Scriptable Objects/EventTrigger_SO")]
public class EventTrigger_SO : ScriptableObject
{
    public string ObjectID;
    public EEventTriggerType TriggerType;

    public void SetUp()
    {
        if (!IDManager.Instance.TryGet(ObjectID, out var obj))
        {
            Debug.LogError($"비유효한 ObjectID {ObjectID}");
            return;
        }

        Type type = null;
        switch (TriggerType)
        {
            case EEventTriggerType.Interaction:
                type = Type.GetType("Interactable");
                break;
        }

        if (type == null)
        {
            // 현재 가능성 없음
        }

    }

    public void AddEventTrigger()
    {
        switch (TriggerType)
        {
            case EEventTriggerType.Interaction:

                break;
        }
    }

    public void RemoveTrigger()
    {
        switch (TriggerType)
        {
            case EEventTriggerType.Interaction:
                break;
        }
    }


    public enum EEventTriggerType
    {
        Interaction,
    }
}
