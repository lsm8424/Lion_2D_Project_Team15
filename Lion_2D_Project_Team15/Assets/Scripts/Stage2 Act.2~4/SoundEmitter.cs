using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public GameObject soundParticlePrefab;
    public Transform target;              // 소리 입자가 향할 목표 (포탈)
    public float spawnInterval = 1.5f;
    public float particleLifetime = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnParticle();
            timer = 0f;
        }
    }

    void SpawnParticle()
    {
        GameObject particle = Instantiate(soundParticlePrefab, transform.position, Quaternion.identity);
        if (target != null)
            particle.GetComponent<SoundParticle>().SetTarget(target.position);
        Destroy(particle, particleLifetime);
    }
}
