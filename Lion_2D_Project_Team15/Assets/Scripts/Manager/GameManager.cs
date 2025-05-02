using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f;    // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f;  // ��ȭâ ���� TimeScale
    public ETimeCase CurrentTime;


    /// <summary>
    /// ��ü���� GameObject�� �����ϱ� ���� ���� ���¸� ����
    /// </summary>
    public enum ETimeCase
    {
        Default,
        Dialogue,
        Loading,
        Setting,
    }

    /// <summary>
    /// ��Ȳ�� �´� GameObject ����
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