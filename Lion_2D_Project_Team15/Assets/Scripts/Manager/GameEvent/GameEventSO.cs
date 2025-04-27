using System;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventSO", menuName = "Scriptable Objects/GameEventSO")]
public class GameEventSO : ScriptableObject
{
    public string EventId;

    public EventFunctionSO[] EventFunctions;
}

