using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    #region Resource
    [SerializeField]
    GameObject _sceneCanvasPrefab;
    GameObject _sceneCanvas;
    public Image FadePanel { get; private set; }
    #endregion

    AsyncOperation _currentOperation;
    string _sceneName;
    bool _hasStarted;

    [field: SerializeField]
    public bool ShouldLoadData { get; private set; } = false;

    [Space]
    [Header("Debug")]
    [SerializeField]
    bool DebugMode = false;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("[SceneController] Awake 호출됨");

        // 만약 Prefab이 없다면 Resources/SceneCanvas를 Load하여 사용
        if (_sceneCanvasPrefab == null)
            _sceneCanvasPrefab = Resources.Load<GameObject>("UI/SceneCanvas");

        if (_sceneCanvas == null)
            _sceneCanvas = Instantiate(_sceneCanvasPrefab, transform);

        FadePanel = _sceneCanvas.GetComponentInChildren<Image>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += (scene, loadSceneMode) =>
            StartCoroutine(AfterAwake(scene, loadSceneMode));
    }

    Dictionary<string, SceneInfo> SceneLoadInfo = new Dictionary<string, SceneInfo>()
    {
        { "TitleScene", new SceneInfo("", "", "") },
        { "Prologue2", new SceneInfo("Prologue", "Prologue", "Prologue") },
        { "Ep_1", new SceneInfo("Episode1", "Episode1", "Ep1") },
        { "Ep_2", new SceneInfo("Episode2", "Episode2", "") },
    };

    readonly struct SceneInfo
    {
        public readonly string QuestPath;
        public readonly string EventPath;
        public readonly string StartQuestName;

        public SceneInfo(string questPath, string eventPath, string startQuestName)
        {
            QuestPath = questPath;
            EventPath = eventPath;
            StartQuestName = startQuestName;
        }
    }

    IEnumerator AfterAwake(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"Scene {scene.name} is Loading...");
        GameManager.Instance.SetTimeCase(GameManager.ETimeCase.Loading);
        yield return null;
        if (scene.name == "TitleScene")
        {
            GameManager.Instance.RevertTimeCase();
            yield break;
        }

        // 순서는 ID
        IDManager.Instance.SetUpIdentifiers();

        SceneInfo sceneInfo;
        if (!SceneLoadInfo.TryGetValue(scene.name, out sceneInfo))
        {
            Debug.LogError("잘못된 Scene이름 " + scene.name);
            yield break;
        }
        // QuestManager.Instance.SetUp("Prologue");
        // QuestManager.Instance.StartQuest("Prologue");


        Debug.Log($"{sceneInfo.QuestPath} is Setting...");

        // ID, Event, Quest 순으로 초기화
        IDManager.Instance.SetUpIdentifiers();

        if (DebugMode)
        {
            if (ShouldLoadData)
            {
                SaveManager.Instance.Load();
                ShouldLoadData = false;
            }
            yield break;
        }

        EventManager.Instance.SetupEvents(sceneInfo.EventPath);
        QuestManager.Instance.SetUp(sceneInfo.QuestPath);

        if (ShouldLoadData)
        {
            SaveManager.Instance.Load();
            ShouldLoadData = false;
        }
        Debug.Log($"Scene {scene.name} is Loaded.");
        GameManager.Instance.RevertTimeCase();
        if (!ShouldLoadData)
            QuestManager.Instance.StartQuest(sceneInfo.StartQuestName);
    }

    public void LoadSaveScene(IScreenEffect startEffect, IScreenEffect endEffect)
    {
        ShouldLoadData = true;

        LoadSceneWithEffect(SaveManager.Instance.GetSceneName(), startEffect, endEffect);
    }

    /// <summary>
    /// Fade In/Out 효과를 적용하며 Scene로드
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="fadeDuration"></param>
    public void LoadSceneWithFadeInOut(string sceneName, float fadeDuration)
    {
        var fadeIn = new Fade(Color.clear, Color.black, fadeDuration);
        var fadeOut = new Fade(Color.black, Color.clear, fadeDuration);

        LoadSceneWithEffect(sceneName, fadeIn, fadeOut);
    }

    /// <summary>
    /// 이펙트 효과를 적용하여 씬 전환
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="startEffect"></param>
    /// <param name="endEffect"></param>
    public void LoadSceneWithEffect(
        string sceneName,
        IScreenEffect startEffect,
        IScreenEffect endEffect
    )
    {
        if (_hasStarted)
            return;

        _hasStarted = true;
        _sceneName = sceneName;

        StartCoroutine(
            ScreenEffectController.InOutEffect(
                startEffect,
                endEffect,
                LoadSceneAsync,
                () => GetProgress() >= 1f,
                SwitchScene
            )
        );
    }

    /// <summary>
    /// Scene 준비
    /// </summary>
    public void LoadSceneAsync()
    {
        _currentOperation = SceneManager.LoadSceneAsync(_sceneName);
        _currentOperation.allowSceneActivation = false;
        _hasStarted = true;
    }

    /// <summary>
    /// 0.9f - 다음 씬이 준비된 상태
    /// 1f - 로드가 완료된 상태
    /// </summary>
    /// <returns></returns>
    public float GetProgress() => Mathf.Clamp01(_currentOperation.progress / 0.9f);

    /// <summary>
    /// Scene 전환
    /// </summary>
    public void SwitchScene()
    {
        _currentOperation.allowSceneActivation = true;
        _hasStarted = false;
    }
}
