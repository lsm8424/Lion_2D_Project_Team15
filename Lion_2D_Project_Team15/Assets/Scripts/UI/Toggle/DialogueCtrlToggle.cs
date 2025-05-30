using UnityEngine;
using UnityEngine.UI;

public class DialogueCtrlToggle : MonoBehaviour
{
    [SerializeField] Toggle _toggle;
    [SerializeField] GameObject _offImage;
    [SerializeField] RectTransform _ctrlLabel;

    public void OnValueChanged()
    {
        if (_toggle.isOn)
        {
            _ctrlLabel.anchoredPosition = Vector2.zero;
            _offImage.SetActive(false);
        }
        else
        {
            _ctrlLabel.anchoredPosition = new Vector2(0, 8f);
            _offImage.SetActive(true);
        }
    }

    public void SetAuto()
    {
        DialogueManager.Instance.IsAuto = !_toggle.isOn;
    }
    public void SetSkip()
    {
        DialogueManager.Instance.IsSkip = !_toggle.isOn;
    }
}
