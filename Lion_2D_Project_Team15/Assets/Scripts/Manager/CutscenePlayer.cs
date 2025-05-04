using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutscenePlayer : Singleton<CutscenePlayer>
{
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
        PlayableDirector.Play(Clip);
    }


    public void TimelineEndTrigger()
    {
        IsPlaying = false;
    }
}
