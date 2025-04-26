using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    GameObject _settingsCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 테스트용
            //SceneController.Instance.LoadSceneWithFadeInOut("Main", 3f);
            //ToggleSettings();
        }
    }

    /// <summary>
    /// 설정창을 토글합니다
    /// </summary>
    public void ToggleSettings()
    {
        if (_settingsCanvas == null)
        {
            GameObject canvasPrefab = Resources.Load<GameObject>("UI/SettingsCanvas");
            _settingsCanvas = Instantiate(canvasPrefab, transform);
            return;
        }
        _settingsCanvas.SetActive(!_settingsCanvas.activeSelf);
    }
}
