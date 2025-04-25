using TMPro;
using UnityEngine;
using AudioType = AudioManager.EAudioType;

public class VolumeSliderController : SliderController
{
    [SerializeField] TextMeshProUGUI _percentage;
    [SerializeField] AudioType _audioType;
    public void SetVolume()
    {
        string audioGroupName = _audioType.ToString();
        float value = _slider.value;
        AudioManager.Instance.SetVolume(audioGroupName, value);
        PlayerPrefs.SetFloat(audioGroupName + "Volume", value);
        SetText();
    }

    public void SetText()
    {
        _percentage.SetText($"{100 * _slider.value:F0}%");
    }

    void Start()
    {
        float volume = PlayerPrefs.GetFloat(_audioType.ToString() + "Volume", 1f);
        _slider.value = volume;
    }

}
