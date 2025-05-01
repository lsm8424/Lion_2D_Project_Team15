using System.Collections;
using UnityEngine;

public abstract class EventFunction_SO : ScriptableObject
{
    public EGameEventFunctionType FunctionType;
    public enum EGameEventFunctionType
    {
        Normal,
        Cutscene,
        ParameterFunction,
        Unique,
    }

    public abstract void Setup();
    public abstract IEnumerator Execute();
}

