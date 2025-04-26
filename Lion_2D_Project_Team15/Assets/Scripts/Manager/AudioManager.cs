using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioMixer _audioMixer;

    [field: SerializeField] public AudioSource BackgroundAudio { get; private set; }

    readonly Dictionary<EAudioType, AudioMixerGroup> _audioGroups = new();
    public enum EAudioType
    {
        Master,
        Background,
        SFX,
    }

    protected override void Awake()
    {
        base.Awake();

        if (_audioMixer == null)
        {
            _audioMixer = Resources.Load<AudioMixer>("Audio/VolumeMixer");
        }

        // AudioMixerGroup �ʱ�ȭ
        for (int i = 0; i < 3; ++i)
        {
            EAudioType type = (EAudioType)i;
            string groupName = type.ToString();
            _audioGroups[type] = _audioMixer.FindMatchingGroups(groupName)[0];
        }

        // Background ����
        BackgroundAudio = GetComponent<AudioSource>();
        BackgroundAudio.outputAudioMixerGroup = _audioGroups[EAudioType.Background];
    }
    public void SetVolume(EAudioType audioType, float volume) => SetVolume(audioType.ToString(), volume);

    // volume�� 0�� ���, �����δ� mute���� ����
    public void SetVolume(string audioMixerGroup, float volume)
    {
        _audioMixer.SetFloat(audioMixerGroup+"Volume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 2f)) * 20); // ���ú� ���
    }


    // �̻��
    //public void SoundSFX(Transform parent, AudioClip audioClip, float volume = 1f)
    //{
    //    GameObject spawnedObject = PoolManager.Instance.Get(_SFXAudioSourcePrefab.name);
    //    spawnedObject.transform.SetParent(parent);

    //    if (spawnedObject.TryGetComponent<AudioSource>(out var audioSource))
    //    {
    //        audioSource.volume = volume;
    //        audioSource.PlayOneShot(audioClip);
    //    }

    //    audioSource.
    //}
}
