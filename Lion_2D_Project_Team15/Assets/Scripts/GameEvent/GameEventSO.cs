using System;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventSO", menuName = "Scriptable Objects/GameEventSO")]
public class GameEventSO : ScriptableObject
{
    public EEventType EventType;
    public string EventId;
    public string EventName;
    public bool RequireSave;

    public EventFunctionSO[] EventFunctions;

    public enum EEventType
    {
        Main,
        Sub,
        Trigger,
    }
}

