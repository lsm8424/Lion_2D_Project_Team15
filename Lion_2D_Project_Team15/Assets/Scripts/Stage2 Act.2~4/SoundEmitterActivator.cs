using UnityEngine;

public class SoundEmitterActivator : MonoBehaviour
{
    public GameObject soundEmitter; // SoundEmitter 오브젝트를 Inspector에서 할당

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soundEmitter.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soundEmitter.SetActive(false);
        }
    }
}
