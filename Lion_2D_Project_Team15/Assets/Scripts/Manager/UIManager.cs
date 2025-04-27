using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    GameObject _settingsCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �׽�Ʈ��
            //SceneController.Instance.LoadSceneWithFadeInOut("JYH_1", 3f);
            //ToggleSettings();
        }
    }

    /// <summary>
    /// ����â�� ����մϴ�
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
