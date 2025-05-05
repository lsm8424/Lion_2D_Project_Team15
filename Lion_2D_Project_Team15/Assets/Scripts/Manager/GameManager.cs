using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f; // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f; // 대화창 관련 TimeScale
    public ETimeCase CurrentTime { get; private set; } = ETimeCase.EntityMovement;
    public Stack<ETimeCase> _prevCaseStack = new();
    /// <summary>
    /// 전체적인 GameObject를 제어하기 위해 게임 상태를 정의
    /// </summary>
    public enum ETimeCase
    {
        EntityMovement,
        PlayingDialogue,
        Loading,
        Setting,
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 테스트용
            UIManager.Instance.ToggleSettings();
        }
    }

    public bool NeedsWaitForSetting() => CurrentTime == ETimeCase.Setting;
    public bool NeedsWaitForDialogue() => CurrentTime == ETimeCase.PlayingDialogue;
    /// <summary>
    /// 상황애 맞는 GameObject 관리
    /// </summary>
    /// <param name="timeCase"></param>
    public void SetTimeScale(ETimeCase timeCase)
    {
        _prevCaseStack.Push(CurrentTime);
        CurrentTime = timeCase;
        ApplyTimeScale(timeCase);
    }
    public void RevertTimeScale()
    {
        if (_prevCaseStack.Count == 0)
        {
            Debug.LogError("의도되지 않은 경우");
            return;
        }

        var prevCase = _prevCaseStack.Pop();
        CurrentTime = prevCase;
        ApplyTimeScale(CurrentTime);
    }

    void ApplyTimeScale(ETimeCase timeCase)
    {
        Debug.Log("CurrentTimeScale: " + CurrentTime);
        switch (timeCase)
        {
            case ETimeCase.EntityMovement:
                EntityTimeScale = 1f;
                DialogueTimeScale = 1f;
                break;
            case ETimeCase.PlayingDialogue:
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
    }
}
