using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventSequence_SO", menuName = "Scriptable Objects/GameEventSequence_SO")]
public class GameEventSequence_SO : ScriptableObject
{
    public EventTriggerPair[] EventTriggerSequence;

    [Serializable]
    public struct EventTriggerPair
    {
        public EEventTriggerType EventTrigger;
        public GameEvent_SO GameEvent;

        public string InteractionTarget;

        public string EnterEntity;
        public string ColliderID;
        Collider2D _enterCollider;    // 이것도 리플렉션으로 Collider를 가져와야 할듯 되는지 모르겠음

    }



    public EventTrigger_SO UniqueTrigger;

    public enum EEventTriggerType
    {
        None,
        Interaction,
        EnterArea,  // 미구현
        // Condition,  // 미구현 -> Func<bool> ? 굳이 이렇게 만들어야 할까
        Unique
    }

    public IEnumerator ProcessEventSequence()
    {
        for (int i = 0; i < EventTriggerSequence.Length; ++i)
        {
            EEventTriggerType triggerType = EventTriggerSequence[i].EventTrigger;
            GameEvent_SO gameEvent = EventTriggerSequence[i].GameEvent;

            switch (triggerType)
            {
                case EEventTriggerType.None:
                    break;
                case EEventTriggerType.Interaction:
                    yield return HandleTriggerInteraction();
                    break;
                case EEventTriggerType.EnterArea:
                    break;
                case EEventTriggerType.Unique:
                    break;
            }

            yield return gameEvent.Execute();
        }
    }

    IEnumerator HandleTriggerInteraction()
    {
        yield return null;
    }
}
