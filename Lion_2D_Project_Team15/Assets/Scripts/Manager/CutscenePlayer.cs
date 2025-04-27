using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutscenePlayer : Singleton<CutscenePlayer>
{
    public TimelineAsset Clip { get; private set; }
    PlayableDirector _playableDirector;

    protected override void Awake()
    {
        base.Awake();

        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void Play(TimelineAsset clip)
    {
        if (Clip == null)
        {
            Debug.LogError("Clip이 등록되지 않았습니다.");
        }

        Clip = clip;
        _playableDirector.Play(Clip);
    }
}
