using UnityEngine;

[CreateAssetMenu(fileName = "UniqueFunction_SO", menuName = "Scriptable Objects/EventFunction/UniqueFunction_SO")]
public abstract class UniqueFunction_SO : EventFunction_SO
{
    public UniqueFunction_SO()
    {
        FunctionType = EGameEventFunctionType.Unique;
    }
}
