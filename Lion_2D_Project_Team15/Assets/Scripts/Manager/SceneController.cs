using System;
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
    public bool IsSceneLoaded { get; private set; }

    [field: SerializeField]
    public bool IsLoadMode { get; private set; } = false;

    IScreenEffect _startEffect;
    IScreenEffect _endEffect;

    [Space]
    [Header("Debug")]
    [SerializeField]
    bool DebugMode = false;

    protected override void Awake()
    {
        base.Awake();

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
        { "Ep_2", new SceneInfo("Episode2", "Episode2", "Ep2") },
        { "Ep_2_Boss", new SceneInfo("Episode2_Boss", "Episode2_Boss", "Ep2Boss") }
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

        SceneInfo sceneInfo;
        if (!SceneLoadInfo.TryGetValue(scene.name, out sceneInfo))
        {
            Debug.LogError("잘못된 Scene이름 " + scene.name);
            yield break;
        }

        Debug.Log($"{sceneInfo.QuestPath} is Setting...");

        // ID, Event, Quest 순으로 초기화
        IDManager.Instance.SetUpIdentifiers();

        // 공통 Normal 이벤트 설정
        Normal_SO[] normalEvents = Resources.LoadAll<Normal_SO>("GameEvent/Normal");
        if (normalEvents != null && normalEvents.Length > 0)
        {
            Debug.Log($"공통 Normal 이벤트 {normalEvents.Length}개 로드");
            foreach (var normalEvent in normalEvents)
            {
                normalEvent.SetTrigger();
            }
        }

        // 씬별 이벤트 설정
        EventManager.Instance.SetupEvents(sceneInfo.EventPath);
        QuestManager.Instance.SetUp(sceneInfo.QuestPath);

        if (IsLoadMode)
        {

            for (int i = 0; i < 5; ++i)     // Scene 이동 시 Cinemachine으로 인한 메인 카메라 이동의 지연을 방지하기 위한 대기
                SaveManager.Instance.Load();

            for (int i = 0; i < 5; ++i) // Scene 이동 시 Cinemachine으로 인한 메인 카메라 이동의 지연을 방지하기 위한 대기
                yield return null;

            SaveManager.Instance.Load();

            if (_endEffect != null)
            {
                yield return _endEffect.Execute();
                _endEffect = null;
            }
            GameManager.Instance.RevertTimeCase();
            IsLoadMode = false;
        }
        else
        {
            if (_endEffect != null)
            {
                yield return _endEffect.Execute();
                _endEffect = null;
            }
            GameManager.Instance.RevertTimeCase();
            QuestManager.Instance.StartQuest(sceneInfo.StartQuestName);
        }

        IsSceneLoaded = true;
        Debug.Log($"Scene {scene.name} is Loaded.");
    }

    public void LoadSaveScene(IScreenEffect startEffect, IScreenEffect endEffect)
    {
        if (IsLoadMode)
            return;

        IsLoadMode = true;

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

        _startEffect = startEffect;
        _endEffect = endEffect;

        _hasStarted = true;
        _sceneName = sceneName;

        StartCoroutine(StartLoadCoroutine());
    }

    IEnumerator StartLoadCoroutine()
    {
        _currentOperation = SceneManager.LoadSceneAsync(_sceneName);
        _currentOperation.allowSceneActivation = false;
        _hasStarted = true;
        IsSceneLoaded = false;

        if (_startEffect != null)
        {
            yield return _startEffect.Execute();
            _startEffect = null;
        }
        yield return new WaitUntil(() => _currentOperation.progress == 0.9f);

        // OnSceneLoaded 이벤트에서 실행
        //if (_endEffect != null)
        //{
        //    yield return _endEffect.Execute();
        //    _endEffect = null;
        //}

        _currentOperation.allowSceneActivation = true;
        _hasStarted = false;
    }
}
