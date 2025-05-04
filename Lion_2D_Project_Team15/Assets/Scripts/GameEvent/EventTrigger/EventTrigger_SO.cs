using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "EventTrigger_SO", menuName = "Scriptable Objects/EventTrigger_SO")]
public class EventTrigger_SO : ScriptableObject
{
    public string ObjectID;
    public InteractionType Type;
    public TriggerCondition_SO[] Conditions;

    [HideInInspector] public string QuestID; // Quest_SO에서 값 세팅할 예정
    [HideInInspector] public int ProgressLevel; // Quest_SO에서 값 세팅할 예정

    IInteractable _targetInteractable;
    public void SetUp()
    {
        if (!IDManager.Instance.TryGet(ObjectID, out var targetObject))
        {
            Debug.LogError($"비유효한 ObjectID {ObjectID}");
            return;
        }

        Type interfaceType = typeof(IInteractable);

        if (!targetObject.TryGetComponent(interfaceType, out var component))
        {
            Debug.LogError($"해당 오브젝트에 적절한 컴포넌트를 찾을 수 없습니다. {targetObject.name} Type: {interfaceType}");
            return;
        }
        var interactable = targetObject.GetComponent<IInteractable>();
        EventInfo eventInfo = interfaceType.GetEvent("OnInteracted");

        //switch (TriggerType)
        //{
        //    case EEventTriggerType.OnInteracted:
        //        type = Type.GetType("IInteractable");
        //        break;
        //    case EEventTriggerType.OnTriggerEntered:
        //        type = Type.GetType("ITriggerEnterable");
        //        break;
        //}

        // 메소드 정보 얻고, 타겟 정보 얻고 등록/해제는 밑에 함수들로
        // 조건은 간단히 Interactive, 

    }

    public void AddEventTrigger()
    {
        _targetInteractable.OnInteracted += Event;
    }

    public void RemoveTrigger()
    {
        _targetInteractable.OnInteracted -= Event;
    }

    void Event(InteractionType type)
    {
        if (type != Type) return;

        bool flag = true;
        foreach (var condition in Conditions)
            flag &= condition.Validate();

        if (!flag) return;

        QuestManager.Instance.OnTriggerComplete(QuestID, ProgressLevel);
    }


    public enum EEventTriggerType
    {
        OnInteracted,
        //OnTriggerEntered,
    }
}
