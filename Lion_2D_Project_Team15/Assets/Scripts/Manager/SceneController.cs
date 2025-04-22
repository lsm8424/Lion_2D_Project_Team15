using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController  : Singleton<SceneController>
{
    #region Resource
    [SerializeField] GameObject _sceneCanvasPrefab;
    Image _fadePanel;
    #endregion

    #region Flag
    [HideInInspector] public bool IsSceneLoaded { get; private set; }
    bool _isFadeInComplete;
    [HideInInspector] public bool IsFadeOutComplete { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // 만약 Prefab이 없다면 Resources/SceneCanvas를 Load하여 사용
        if (_sceneCanvasPrefab == null)
        {
            _sceneCanvasPrefab = Resources.Load<GameObject>("SceneCanvas");
        }
        GameObject spawnedCanvas = Instantiate(_sceneCanvasPrefab);
        DontDestroyOnLoad(spawnedCanvas);
        _fadePanel = spawnedCanvas.GetComponentInChildren<Image>();
    }

    /// <summary>
    /// d
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="fadeDuration"></param>
    public void LoadSceneAsync(string sceneName, float fadeDuration)
    {
        StartCoroutine(LoadSceneAsyncWithFadeInOut(sceneName, fadeDuration));
    }

    /// <summary>
    /// Fade In/Out 효과를 적용하며 Scene로드
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="fadeDuration"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAsyncWithFadeInOut(string sceneName, float fadeDuration)
    {
        // Set Flag
        _isFadeInComplete = false;
        IsFadeOutComplete = false;
        IsSceneLoaded = false;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        StartCoroutine(FadeIn(fadeDuration));

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // 만약 씬 전환이 일어나도록 만드려면
            if (progress >= 1 && _isFadeInComplete)
            {
                operation.allowSceneActivation = true;
                StartCoroutine(FadeOut(fadeDuration));
            }

            yield return null;
        }

        IsSceneLoaded = true;
    }

    #region Fade In/Out
    public IEnumerator FadeIn(float duration)
    {
        _isFadeInComplete = false;
        float percent = 0;
        float elapsedTime = 0;

        Color panelColor = _fadePanel.color;
        panelColor.a = 0;
        _fadePanel.color = panelColor;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / duration;

            panelColor.a = percent;
            _fadePanel.color = panelColor;

            yield return null;
        }

        panelColor.a = 1;
        _fadePanel.color = panelColor;

        _isFadeInComplete = true;
    }

    public IEnumerator FadeOut(float duration)
    {
        _isFadeInComplete = false;
        float percent = 0;
        float elapsedTime = 0;

        Color panelColor = _fadePanel.color;
        panelColor.a = 1;
        _fadePanel.color = panelColor;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / duration;

            panelColor.a = 1 - percent;
            _fadePanel.color = panelColor;

            yield return null;
        }

        panelColor.a = 0;
        _fadePanel.color = panelColor;
        _isFadeInComplete = true;
    }
    #endregion
}
