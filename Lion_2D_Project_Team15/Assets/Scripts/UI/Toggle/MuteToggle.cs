using UnityEngine;
using UnityEngine.UI;
using AudioType = AudioManager.EAudioType;
public class MuteToggle : MonoBehaviour
{
    [SerializeField] Toggle _toggle;
    [SerializeField] AudioType _audioType;

    // Mute = 1, Not mute = 2
    void Start()
    {
        _toggle.isOn = PlayerPrefs.GetInt(_audioType.ToString() + "Mute", 0) == 1;
    }

    public void SaveSetting()
    {
        PlayerPrefs.SetInt(_audioType.ToString() + "Mute", _toggle.isOn ? 1 : 0);
    }
}
