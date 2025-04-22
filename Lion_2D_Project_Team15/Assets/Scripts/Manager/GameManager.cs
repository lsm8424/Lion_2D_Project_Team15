using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f;    // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f;  // ��ȭâ ���� TimeScale

    /// <summary>
    /// ��Ȳ�� �´� TimeScale�� �����Ű�� ���� Case
    /// </summary>
    public enum ETimeCase
    {
        Normal,
        OnDialogue,
        OnSetting,
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
