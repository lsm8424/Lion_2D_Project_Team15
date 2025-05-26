using UnityEngine;

public class EP2_BGM : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] bgmClips; // Array to hold the BGM clips

    private GameObject player;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null)
        {
            if (player.transform.position.x > 120)
            {
                audioSource.clip = bgmClips[1];
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.clip = bgmClips[0];
            
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
