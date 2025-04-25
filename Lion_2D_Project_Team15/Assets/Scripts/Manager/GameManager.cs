using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f;    // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f;  // ��ȭâ ���� TimeScale

    public ETimeCase prevTimeCase = ETimeCase.Normal;

    /// <summary>
    /// ��Ȳ�� �´� TimeScale�� �����Ű�� ���� Case
    /// </summary>
    public enum ETimeCase
    {
        Normal,
        Dialogue,
        Pause,
    }

    /// <summary>
    /// ���ӻ�Ȳ�� ���� ������Ʈ�� ����/����
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
            case ETimeCase.Dialogue:
                EntityTimeScale = 0f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.Pause:
                EntityTimeScale = 0f;
                DialogueTimeScale = 0f;
                break;
        }

        if (timeCase != ETimeCase.Pause)
            prevTimeCase = timeCase;
    }

    public void PauseGame() => SetTimeScale(ETimeCase.Pause);
    public void ResumeGame() => SetTimeScale(prevTimeCase);
}
