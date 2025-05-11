using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float EntityTimeScale { get; private set; } = 1f; // Entity, NPC TimeScale
    public float DialogueTimeScale { get; private set; } = 1f; // 대화창 관련 TimeScale
    [field:SerializeField] public ETimeCase CurrentTime { get; private set; } = ETimeCase.EntityMovement;
    public Stack<ETimeCase> _prevCaseStack = new();
    public bool IsManagerLoaded { get; private set; } = false;
    /// <summary>
    /// 전체적인 GameObject를 제어하기 위해 게임 상태를 정의
    /// </summary>
    public enum ETimeCase
    {
        EntityMovement = 0,
        PlayingDialogue = 1,
        Setting = 2,
        Title = 3,
        Loading = 4,
    }

    protected override void Awake()
    {
        base.Awake();
        WaitForManagersToLoad();
    }

    void Update()
    {
        if (!IsManagerLoaded)
            return;

        if (CurrentTime <= ETimeCase.Title)
            if (Input.GetKeyDown(KeyCode.Escape))
                UIManager.Instance.OnPressedESC();

        // 테스트용 코드
        if (Input.GetKeyDown(KeyCode.Alpha0))
            StartGame();
        if (Input.GetKeyDown(KeyCode.Minus))
            LoadGame();
    }

    async UniTask WaitForManagersToLoad()
    {
        await UniTask.WaitUntil(() => SceneController.Instance.didAwake);
        await UniTask.WaitUntil(() => IDManager.Instance.didAwake);
        await UniTask.WaitUntil(() => EventManager.Instance.didAwake);
        await UniTask.WaitUntil(() => QuestManager.Instance.didAwake);
        await UniTask.WaitUntil(() => DialogueManager.Instance.didAwake);
        await UniTask.WaitUntil(() => AudioManager.Instance.didAwake);
        await UniTask.WaitUntil(() => SaveManager.Instance.didAwake);
        IsManagerLoaded = true;
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

    public void ResetTimecase()
    {
        _prevCaseStack.Clear();
        CurrentTime = ETimeCase.EntityMovement;
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
            case ETimeCase.Title:
                EntityTimeScale = 1f;
                DialogueTimeScale = 1f;
                break;
            default:
                Debug.LogError("유효하지 않은 TimeCase " + timeCase);
                break;
        }
    }

    public void OnTitle()
    {
        SetTimeCase(ETimeCase.Title);
        if (SaveManager.Instance.HasSave)
        {
            // Load 버튼 활성화
        }
    }

    public void StartGame()
    {
        SceneController.Instance.LoadSceneWithFadeInOut("EP_1", 2f);
    }

    public void LoadGame()
    {
        SceneController.Instance.LoadSaveScene(new Fade(Color.clear, Color.black, 1f), new Fade(Color.black, Color.clear, 1f));

        // Scene Load 후
        // EventManager, QuestManager, IdManager SetUp.
        // SaveManager.Load()
        // 이후 fadeout 되면서 시작
    }
}
