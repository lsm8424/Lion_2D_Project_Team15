using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AudioType = AudioManager.EAudioType;
public class MuteToggle : MonoBehaviour
{
    [SerializeField] Toggle _toggle;
    [SerializeField] AudioType _audioType;

    [SerializeField] Image _volumeOn;
    [SerializeField] Image _volumeOff;

    // Mute = 1, Not mute = 0
    void Awake()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
        _toggle.isOn = PlayerPrefs.GetInt(_audioType.ToString() + "Mute", 0) == 1;
        OnValueChanged(_toggle.isOn);
    }

    public void SaveSetting()
    {
        PlayerPrefs.SetInt(_audioType.ToString() + "Mute", _toggle.isOn ? 1 : 0);
    }

    public void LoadSetting()
    {
        _toggle.isOn = PlayerPrefs.GetInt(_audioType.ToString() + "Mute", 0) == 1;
        OnValueChanged(_toggle.isOn);
        SaveSetting();
    }

    public void OnValueChanged(bool isOn)
    {
        _volumeOn.gameObject.SetActive(!isOn);
        _volumeOff.gameObject.SetActive(isOn);

        if (!isOn)
        {
            _toggle.targetGraphic = _volumeOn;
        }

        _toggle.onValueChanged?.Invoke(isOn);
    }
}
