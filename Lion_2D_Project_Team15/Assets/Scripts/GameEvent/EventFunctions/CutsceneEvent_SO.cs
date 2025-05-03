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

        CutscenePlayer cutscenePlayer = CutscenePlayer.Instance;
        PlayableDirector playableDirector = cutscenePlayer.PlayableDirector;

        // Timeline이 종료되었는지 확실하게 알 수 있는 방법이 없음
        // Timeline 끝에 Trigger로 알리는 방법 CutscenePlayer.TimelineEndTrigger();

        cutscenePlayer.Play(Clip);
        yield return new WaitUntil(() => !cutscenePlayer.IsPlaying);
    }

    public override void Setup()
    {
    
    }
}
