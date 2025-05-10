using System.Collections;
using Unity.VisualScripting;
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

    protected override void Awake()
    {
        base.Awake();

        // 만약 Prefab이 없다면 Resources/SceneCanvas를 Load하여 사용
        if (_sceneCanvasPrefab == null)
            _sceneCanvasPrefab = Resources.Load<GameObject>("UI/SceneCanvas");

        if (_sceneCanvas == null)
            _sceneCanvas = Instantiate(_sceneCanvasPrefab, transform);

        FadePanel = _sceneCanvas.GetComponentInChildren<Image>();

        // 추후작성 필요
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //ep2 포탈을 위한 주석처리
        //SceneManager.sceneLoaded += (scene, loadSceneMode) => StartCoroutine(AfterAwake(scene, loadSceneMode)); // 임시용 코드 이후에 위 코드와 교체
    }

    IEnumerator AfterAwake(Scene scene, LoadSceneMode loadSceneMode)
    {
        yield return null;
        if (scene.name == "Title")
        {
            yield break;
        }

        // 순서는 ID
        IDManager.Instance.SetUpIdentifiers();
        QuestManager.Instance.SetUp("Episode1");
        //QuestManager.Instance.StartQuest("Ep1");

        // if Load
        // SaveManger.Instance.Load();
    }

    void LoadSaveData() { }

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
