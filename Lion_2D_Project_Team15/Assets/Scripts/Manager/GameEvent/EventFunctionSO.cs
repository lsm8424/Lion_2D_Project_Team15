using UnityEngine;
using static SimpleEventFunction;

public abstract class EventFunctionSO : ScriptableObject
{
    public EGameEventFunctionType FunctionType;

    public abstract void Initialize();
    public abstract void Execute();
}

public enum EGameEventFunctionType
{
    Normal,
    Quest,
    Cutscene,
    Unique,
}
