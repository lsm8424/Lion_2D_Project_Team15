using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController  : Singleton<SceneController>
{
    #region Resource
    [SerializeField] GameObject _sceneCanvasPrefab;
    GameObject _sceneCanvas;
    public Image FadePanel { get; private set; }
    #endregion

    AsyncOperation _currentOperation;
    string _sceneName;
    bool _hasStarted;

    protected override void Awake()
    {
        base.Awake();

        // ���� Prefab�� ���ٸ� Resources/SceneCanvas�� Load�Ͽ� ���
        if (_sceneCanvasPrefab == null)
            _sceneCanvasPrefab = Resources.Load<GameObject>("UI/SceneCanvas");

        if (_sceneCanvas == null)
            _sceneCanvas = Instantiate(_sceneCanvasPrefab, transform);

        FadePanel = _sceneCanvas.GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Fade In/Out ȿ���� �����ϸ� Scene�ε�
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
    /// ����Ʈ ȿ���� �����Ͽ� �� ��ȯ
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="startEffect"></param>
    /// <param name="endEffect"></param>
    public void LoadSceneWithEffect(string sceneName, IScreenEffect startEffect, IScreenEffect endEffect)
    {
        if (_hasStarted)
            return;

        _hasStarted = true;
        _sceneName = sceneName;

        StartCoroutine(ScreenEffectController.InOutEffect(
            startEffect,
            endEffect,
            LoadSceneAsync,
            () => GetProgress() >= 1f,
            SwitchScene
        ));
    }

    /// <summary>
    /// Scene �غ�
    /// </summary>
    public void LoadSceneAsync()
    {
        _currentOperation = SceneManager.LoadSceneAsync(_sceneName);
        _currentOperation.allowSceneActivation = false;
        _hasStarted = true;
    }

    /// <summary>
    /// 0.9f - ���� ���� �غ�� ����
    /// 1f - �ε尡 �Ϸ�� ����
    /// </summary>
    /// <returns></returns>
    public float GetProgress() => Mathf.Clamp01(_currentOperation.progress / 0.9f);

    /// <summary>
    /// Scene ��ȯ
    /// </summary>
    public void SwitchScene()
    {
        _currentOperation.allowSceneActivation = true;
        _hasStarted = false;
    }
}
