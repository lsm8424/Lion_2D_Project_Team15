using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutscenePlayer : Singleton<CutscenePlayer>
{
    [field: SerializeField]
    public TimelineAsset Clip { get; private set; }
    public PlayableDirector PlayableDirector { get; private set; }
    public bool IsPlaying = false;

    protected override void Awake()
    {
        base.Awake();

        PlayableDirector = GetComponent<PlayableDirector>();
    }

    public void Play(TimelineAsset clip)
    {
        if (Clip == null)
        {
            Debug.LogError("Clip이 등록되지 않았습니다.");
        }
        IsPlaying = true;
        Clip = clip;
        PlayableDirector.extrapolationMode = DirectorWrapMode.Hold;
        PlayableDirector.Play(Clip);
        StartCoroutine(WhilePlaying());
    }

    IEnumerator WhilePlaying()
    {
        while (IsPlaying)
        {
            if (GameManager.Instance.ShouldWaitForDialogue())
            {
                //PlayableDirector.Pause(); // Pause()를 사용하면 애니메이션이 초기상태로 돌아가는 경우가 발생함.
                PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
                yield return new WaitUntil(() => !GameManager.Instance.ShouldWaitForDialogue());
                //PlayableDirector.Play();
                PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
            yield return null;
        }
    }

    public void TimelineEndTrigger()
    {
        IsPlaying = false;
    }
}
