using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    SettingCanvas _settingCanvas;
    GameObject _settingsPanel;
    [field: SerializeField] public GameObject VolumePanel { get; private set; }

    readonly Stack<GameObject> PanelStack = new();

    protected override void Awake()
    {
        base.Awake();

        if (_settingCanvas == null)
        {
            GameObject canvasPrefab = Resources.Load<GameObject>("UI/SettingCanvas");
            _settingCanvas = Instantiate(canvasPrefab, transform).GetComponent<SettingCanvas>();
            _settingCanvas.gameObject.SetActive(true);
        }

        _settingsPanel = _settingCanvas.SettingPanel;
        VolumePanel = _settingCanvas.VolumePanel;
        _settingsPanel.gameObject.SetActive(false);
        VolumePanel.gameObject.SetActive(false);
    }

    

    public void OnPressedESC()
    {
        // 열려있는지 창 확인
        // 그 후 열려있으면 창 닫고
        // 열려있지 않다면 세팅패널 열기

        // 창이 하나라도 열려있는지 확인
        if (PanelStack.Count > 0)
        {
            PopPanel();
        }
        else if (PanelStack.Count == 0 && GameManager.Instance.CurrentTime != GameManager.ETimeCase.Title)
        {
            OpenPanel(_settingsPanel);
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
