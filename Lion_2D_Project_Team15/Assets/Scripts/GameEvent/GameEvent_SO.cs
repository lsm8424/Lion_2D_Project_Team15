using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent_SO", menuName = "Scriptable Objects/GameEvent_SO")]
public class GameEvent_SO : ScriptableObject
{
    public EEventType EventType;
    public string EventID;
    public string EventName;
    public bool RequireSave;

    public EventFunction_SO[] EventFunctions;

    public void SetUp()
    {
        for (int i = 0; i < EventFunctions.Length; ++i)
            EventFunctions.Initialize();
    }

    public IEnumerator Execute()
    {
        for (int i = 0; i < EventFunctions.Length; ++i)
            yield return EventFunctions[i].Execute();
    }

    public enum EEventType
    {
        Main,
        Sub,
        Trigger,
    }
}

