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
        EntityMovement = 0,
        PlayingDialogue = 1,
        Setting = 2,
        Loading = 3,
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 테스트용
            UIManager.Instance.ToggleSettings();
        }
    }

    public bool ShouldWaitForDialogue() => CurrentTime > ETimeCase.PlayingDialogue;

    public bool ShouldWaitForEntity() => CurrentTime > ETimeCase.EntityMovement;

    /// <summary>
    /// 상황애 맞는 GameObject 관리
    /// </summary>
    /// <param name="timeCase"></param>
    public void SetTimeCase(ETimeCase timeCase)
    {
        _prevCaseStack.Push(CurrentTime);
        CurrentTime = timeCase;
        AdjustTimeScale(timeCase);
    }

    public void RevertTimeCase()
    {
        if (_prevCaseStack.Count == 0)
        {
            Debug.LogError("의도되지 않은 경우");
            return;
        }

        var prevCase = _prevCaseStack.Pop();
        CurrentTime = prevCase;
        AdjustTimeScale(CurrentTime);
    }

    void AdjustTimeScale(ETimeCase timeCase)
    {
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
