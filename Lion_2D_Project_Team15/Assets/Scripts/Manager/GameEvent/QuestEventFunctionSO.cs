using UnityEngine;

[CreateAssetMenu(fileName = "QuestEventFunctionSO", menuName = "Scriptable Objects/QuestEventFunctionSO")]
public class QuestEventFunctionSO : EventFunctionSO
{
    public string QuestId;

    public QuestEventFunctionSO()
    {
        FunctionType = EGameEventFunctionType.Quest;
    }

    public override void Execute()
    {

    }

    public override void Initialize()
    {
    }
}
