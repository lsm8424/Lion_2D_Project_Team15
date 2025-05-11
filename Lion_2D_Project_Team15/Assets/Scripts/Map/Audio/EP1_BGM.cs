using UnityEngine;

public class EP1_BGM : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] bgmClips; // Array to hold the BGM clips

    private GameObject player;
    private bool isDoing = false;

    EventTrigger_SO eventTriggerSO;

    private void Update()
    {

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null)
        {
            if (player.transform.position.x > 97f && player.transform.position.y > 48.2 && !isDoing)
            {
                audioSource.clip = bgmClips[1];
                if (!audioSource.isPlaying)
                {
                    isDoing = true;
                    audioSource.Play();
                }
            }
            else if (player.transform.position.x > 97.5f && isDoing)
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
