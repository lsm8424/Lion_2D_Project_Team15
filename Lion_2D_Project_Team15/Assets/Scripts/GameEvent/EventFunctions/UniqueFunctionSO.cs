using UnityEngine;

[CreateAssetMenu(fileName = "UniqueFunctionSO", menuName = "Scriptable Objects/EventFunction/UniqueFunctionSO")]
public abstract class UniqueFunctionSO : EventFunctionSO
{
    public UniqueFunctionSO()
    {
        FunctionType = EGameEventFunctionType.Unique;
    }
}
