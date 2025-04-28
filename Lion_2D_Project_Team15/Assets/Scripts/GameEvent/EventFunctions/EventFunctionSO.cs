using UnityEngine;
using static SimpleEventFunction;

public abstract class EventFunctionSO : ScriptableObject
{
    public EGameEventFunctionType FunctionType;
    public enum EGameEventFunctionType
    {
        Normal,
        Quest,
        Cutscene,
        Unique,
    }

    public abstract void Initialize();
    public abstract void Execute();
}

