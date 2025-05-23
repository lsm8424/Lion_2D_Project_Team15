using UnityEngine;
using UnityEngine.UI;

public class SettingCanvas : MonoBehaviour
{
    [field: SerializeField] public GameObject SettingPanel { get; private set; }
    [field: SerializeField] public GameObject VolumePanel { get; private set; }

    [field: SerializeField] public Button SaveButton { get; private set; }

    public void OnPanelOpen(GameObject panel)
    {
        UIManager.Instance.OpenPanel(panel);
    }

    void OnEnable()
    {
        SaveButton.interactable = GameManager.Instance.CurrentTime != GameManager.ETimeCase.PlayingDialogue;
    }

    public void OnPressedSaveButton()
    {
        UIManager.Instance.PopPanel();
        SaveManager.Instance.Save();
    }

    public void LoadButton()
    {
        UIManager.Instance.PopPanel();
        GameManager.Instance.LoadGame();
    }

    public void ClosePanel()
    {
        UIManager.Instance.PopPanel();
    }
}
