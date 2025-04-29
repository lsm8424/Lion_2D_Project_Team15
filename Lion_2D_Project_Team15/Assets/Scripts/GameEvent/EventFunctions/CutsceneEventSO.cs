using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "CutsceneEventSO", menuName = "Scriptable Objects/EventFunction/CutsceneEventSO")]
public class CutsceneEventSO : EventFunctionSO
{
    public TimelineAsset Clip;

    public CutsceneEventSO()
    {
        FunctionType = EGameEventFunctionType.Cutscene;
    }

    public override void Execute()
    {
        CutscenePlayer.Instance.Play(Clip);
    }

    public override void Initialize() { }
}
