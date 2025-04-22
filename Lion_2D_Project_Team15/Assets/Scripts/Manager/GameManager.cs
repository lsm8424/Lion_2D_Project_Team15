using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f;    // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f;  // 대화창 관련 TimeScale

    /// <summary>
    /// 상황에 맞는 TimeScale을 적용시키기 위한 Case
    /// </summary>
    public enum ETimeCase
    {
        Normal,
        OnDialogue,
        OnSetting,
    }

    /// <summary>
    /// 게임상황에 따라 오브젝트를 동작/정지
    /// </summary>
    /// <param name="timeCase"></param>
    public void SetTimeScale(ETimeCase timeCase)
    {
        switch (timeCase)
        {
            case ETimeCase.Normal:
                EntityTimeScale = 1f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.OnDialogue:
                EntityTimeScale = 0f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.OnSetting:
                EntityTimeScale = 0f;
                DialogueTimeScale = 0f;
                break;
        }
    }
}
