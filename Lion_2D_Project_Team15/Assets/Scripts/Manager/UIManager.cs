using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    SettingCanvas _settingCanvas;
    GameObject _settingsPanel;
    [field: SerializeField] public GameObject VolumePanel { get; private set; }
    [field: SerializeField] public GameObject GameOverCanvasPrefab { get; private set; }

    readonly Stack<GameObject> PanelStack = new();

    protected override void Awake()
    {
        base.Awake();

        if (_settingCanvas == null)
        {
            GameObject canvasPrefab = Resources.Load<GameObject>("UI/SettingCanvas");
            _settingCanvas = Instantiate(canvasPrefab, transform).GetComponent<SettingCanvas>();
            _settingCanvas.gameObject.SetActive(false);
        }

        if (GameOverCanvasPrefab == null)
            GameOverCanvasPrefab = Resources.Load<GameObject>("UI/GameOverCanvas");

        _settingsPanel = _settingCanvas.SettingPanel;
        VolumePanel = _settingCanvas.VolumePanel;
        _settingsPanel.SetActive(true);
        VolumePanel.SetActive(false);
    }

    public void OnPressedESC()
    {
        if (PanelStack.Count > 0)
        {
            PopPanel();
        }
        else if (PanelStack.Count == 0 && GameManager.Instance.CurrentTime != GameManager.ETimeCase.Title)
        {
            OpenPanel(_settingCanvas.gameObject);
        }
    }

    public void OpenPanel(GameObject panel)
    {
        if (PanelStack.Count == 0)
        {
            GameManager.Instance.SetTimeCase(GameManager.ETimeCase.Setting);
        }

        PanelStack.Push(panel);
        panel.SetActive(true);
    }

    public void PopPanel()
    {
        if (PanelStack.Count == 0)
        {
            Debug.LogError("닫을 패널이 존재하지 않습니다.");
            return;
        }

        GameObject currentPanel = PanelStack.Pop();
        currentPanel.SetActive(false);

        if (PanelStack.Count == 0)
            GameManager.Instance.RevertTimeCase();
    }
}
