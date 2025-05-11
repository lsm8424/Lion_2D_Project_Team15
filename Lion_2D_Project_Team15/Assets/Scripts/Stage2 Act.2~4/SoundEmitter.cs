using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public GameObject soundParticlePrefab;
    public Transform[] waypoints;
    public float spawnInterval = 1.5f;
    public float particleLifetime = 3f;

    public Transform player;
    public float forwardOffset = 1.5f;

    private float timer;
    private int currentWaypointIndex = 0; // 현재 웨이포인트 인덱스 관리

    void Update()
    {
        if (player != null)
        {
            float dir = Mathf.Sign(player.localScale.x);
            Vector3 offset = Vector3.right * dir * forwardOffset;
            transform.position = player.position + offset;
        }

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
        SoundParticle sp = particle.GetComponent<SoundParticle>();
        if (sp != null && waypoints != null && waypoints.Length > 0)
        {
            sp.SetTargetWaypoint(waypoints[currentWaypointIndex]);
        }
        Destroy(particle, particleLifetime);
    }

    public void AdvanceWaypoint()
    {
        if (waypoints != null && currentWaypointIndex < waypoints.Length - 1)
        {
            currentWaypointIndex++;
        }
    }
}
