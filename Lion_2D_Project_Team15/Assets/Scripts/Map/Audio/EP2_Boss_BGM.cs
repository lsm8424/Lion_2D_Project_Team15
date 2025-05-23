using UnityEngine;

public class EP2_Boss_BGM : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
        if(GameManager.Instance.EntityTimeScale == 0)
            audioSource.Pause();
    }
}
