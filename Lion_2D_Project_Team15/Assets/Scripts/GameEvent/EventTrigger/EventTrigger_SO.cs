using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "EventTrigger_SO", menuName = "Scriptable Objects/EventTrigger_SO")]
public class EventTrigger_SO : ScriptableObject
{
    public string ObjectID;
    public InteractionType Type;
    public TriggerCondition_SO[] Conditions;

    [HideInInspector]
    public string QuestID; // Quest_SO에서 값 세팅할 예정

    [HideInInspector]
    public int ProgressLevel; // Quest_SO에서 값 세팅할 예정

    IInteractable _targetInteractable;

    public void SetUp()
    {
        Debug.Log($"[EventTrigger] SetUp 시작 - ObjectID: {ObjectID}");

        if (!IDManager.Instance.TryGet(ObjectID, out var targetObject))
        {
            Debug.LogError($"[EventTrigger] IDManager에서 ObjectID '{ObjectID}'를 찾을 수 없습니다.");
            return;
        }

        Type interfaceType = typeof(IInteractable);

        Debug.Log($"[EventTrigger] 오브젝트 찾음: {targetObject.name}");

        if (!targetObject.TryGetComponent<IInteractable>(out var component))
        {
            Debug.LogError($"[EventTrigger] {targetObject.name} 에서 IInteractable 구현체를 찾을 수 없습니다.");
            return;
        }

        var interactable = targetObject.GetComponent<IInteractable>();
        EventInfo eventInfo = interfaceType.GetEvent("OnInteracted");

        _targetInteractable = component;

        //Debug.Log("[EventTrigger] IInteractable 할당 성공");

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
        if (_targetInteractable == null)
        {
            Debug.LogError(
                "[EventTrigger] AddEventTrigger 호출 시 _targetInteractable이 null입니다. SetUp이 먼저 되어야 합니다."
            );
            return;
        }

        _targetInteractable.OnInteracted += Event;
    }

    public void RemoveTrigger()
    {
        _targetInteractable.OnInteracted -= Event;
    }

    void Event(InteractionType type)
    {
        //Debug.Log($"[EventTrigger] 상호작용 감지됨! 받은 타입: {type}");

        if (type != Type)
        {
            //Debug.Log($"[EventTrigger] 타입 불일치: 설정된 {Type}, 받은 {type}");
            return;
        }

        bool flag = true;
        foreach (var condition in Conditions)
        {
            flag &= condition.Validate();
        }

        //Debug.Log($"[EventTrigger] 조건 검사 결과: {flag}");

        if (!flag)
            return;

        //Debug.Log("[EventTrigger] 조건 충족 → 퀘스트 진행!");
        QuestManager.Instance.OnTriggerComplete(QuestID, ProgressLevel);
    }

    public enum EEventTriggerType
    {
        OnInteracted,
        //OnTriggerEntered,
    }
}
