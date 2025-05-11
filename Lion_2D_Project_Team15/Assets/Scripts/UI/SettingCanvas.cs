using UnityEngine;

public class SettingCanvas : MonoBehaviour
{
    [field: SerializeField] public GameObject SettingPanel { get; private set; }
    [field: SerializeField] public GameObject VolumePanel { get; private set; }

    public void OnPanelOpen(GameObject panel)
    {
        UIManager.Instance.OpenPanel(panel);
    }

    public void SaveButton()
    {
        SaveManager.Instance.Save();
    }

    public void LoadButton()
    {
        SaveManager.Instance.Load();
    }

    public void ClosePanel()
    {
        UIManager.Instance.PopPanel();
    }
}
