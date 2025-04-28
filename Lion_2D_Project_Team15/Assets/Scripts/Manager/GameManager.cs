using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f;    // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f;  // 대화창 관련 TimeScale
    public ETimeCase CurrentTime;


    /// <summary>
    /// 전체적인 GameObject를 제어하기 위해 게임 상태를 정의
    /// </summary>
    public enum ETimeCase
    {
        Default,
        Dialogue,
        Loading,
        Setting,
    }

    /// <summary>
    /// 상황애 맞는 GameObject 관리
    /// </summary>
    /// <param name="timeCase"></param>
    public void SetTimeScale(ETimeCase timeCase)
    {
        switch (timeCase)
        {
            case ETimeCase.Default:
                EntityTimeScale = 1f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.Dialogue:
                EntityTimeScale = 0f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.Loading:
                EntityTimeScale = 0f;
                DialogueTimeScale = 0f;
                break;
            case ETimeCase.Setting:
                EntityTimeScale = 0f;
                DialogueTimeScale = 0f;
                break;
        }

        CurrentTime = timeCase;
    }
}