using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "CutsceneEvent_SO", menuName = "Scriptable Objects/EventFunction/CutsceneEvent_SO")]
public class CutsceneEvent_SO : EventFunction_SO
{
    public TimelineAsset Clip;

    public CutsceneEvent_SO()
    {
        FunctionType = EGameEventFunctionType.Cutscene;
    }

    // 아직 구현되지 않음
    public override IEnumerator Execute()
    {
        if (Clip == null)
        {
            Debug.LogError("Clip이 할당되지 않았습니다.");
            yield break;
        }
        CutscenePlayer.Instance.Play(Clip);
    }

    public override void Setup() { }
}
