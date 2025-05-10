using System.Collections;
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
    public bool ShouldLoadData = false;

    [Space]
    [Header("Debug")]
    [SerializeField] bool DebugMode = false;
    [SerializeField] string DebugLoadSceneName = "Episode_1";
    [SerializeField] bool StartQuest = true;

    protected override void Awake()
    {
        base.Awake();

        // 만약 Prefab이 없다면 Resources/SceneCanvas를 Load하여 사용
        if (_sceneCanvasPrefab == null)
            _sceneCanvasPrefab = Resources.Load<GameObject>("UI/SceneCanvas");

        if (_sceneCanvas == null)
            _sceneCanvas = Instantiate(_sceneCanvasPrefab, transform);

        FadePanel = _sceneCanvas.GetComponentInChildren<Image>();


        SceneManager.sceneLoaded += (scene, loadSceneMode) => StartCoroutine(AfterAwake(scene, loadSceneMode));
    }

    IEnumerator AfterAwake(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"Scene {scene.name} is Loading...");
        GameManager.Instance.SetTimeCase(GameManager.ETimeCase.Loading);
        yield return null;

        if (scene.name == "Title")
        {
            yield break;
        }

        // Scene 이름은 "Ep_숫자" 로 가정
        string[] split = scene.name.Split('_');
        string episode = split[1];

        if (DebugMode)
            episode = DebugLoadSceneName.Split('_')[1];

        Debug.Log($"Episode{episode} is Setting...");

        // ID, Event, Quest 순으로 초기화
        IDManager.Instance.SetUpIdentifiers();
        EventManager.Instance.SetupEvents("Episode" + episode);
        QuestManager.Instance.SetUp("Episode"+ episode);

#if UNITY_EDITOR
        if (!DebugMode || (DebugMode && StartQuest))
            if (!ShouldLoadData)
                QuestManager.Instance.StartQuest("Ep" + episode);
#else
        if (!ShouldLoadData)
            QuestManager.Instance.StartQuest("Ep" + episode);
#endif

        if (ShouldLoadData)
        {
            SaveManager.Instance.Load();
            ShouldLoadData = false;
        }
        Debug.Log($"Scene {scene.name} is Loaded.");
        GameManager.Instance.SetTimeCase(GameManager.ETimeCase.EntityMovement);
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
