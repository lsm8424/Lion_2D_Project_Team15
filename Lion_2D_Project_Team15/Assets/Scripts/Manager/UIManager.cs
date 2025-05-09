using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    GameObject _settingsCanvas;

    /// <summary>
    /// 설정창을 토글합니다
    /// </summary>
    public void ToggleSettings()
    {
        if (_settingsCanvas == null)
        {
            GameObject canvasPrefab = Resources.Load<GameObject>("UI/SettingsCanvas");
            _settingsCanvas = Instantiate(canvasPrefab, transform);

            _settingsCanvas.SetActive(true);
            GameManager.Instance.SetTimeCase(GameManager.ETimeCase.Setting);
            return;
        }

        bool isActive = _settingsCanvas.activeSelf;
        if (isActive)
        {
            _settingsCanvas.SetActive(false);
            GameManager.Instance.RevertTimeCase();
        }
        else
        {
            _settingsCanvas.SetActive(true);
            GameManager.Instance.SetTimeCase(GameManager.ETimeCase.Setting);
        }
    }
}
