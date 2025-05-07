using TMPro;
using UnityEngine;

public class Stage2_BG_Pattern1 : MonoBehaviour
{
    public GameObject fallingPrefab;

    [Header("떨어지는 음표")]
    [SerializeField] private float fallingCoolTime;
    [SerializeField] private float fallDuration;
    [SerializeField] private int maxX;
    [SerializeField] private int maxY;

    private Vector3 targetPosition;
    private float delta;

    void Start()
    {

    }

    void Update()
    {
        delta -= Time.deltaTime;
        if (delta <= 0)
        {
            SpawnFallingNote();
            delta = fallingCoolTime;
        }
    }

    void SpawnFallingNote()
    {
        int x = 0;
        int y = 0;

        Vector3 spawnPosition = Vector3.zero;

        float distance = spawnPosition.magnitude;

        while (true)
        {
            x = Random.Range(-maxX, maxX + 1);
            y = Random.Range(-maxY, maxY + 1);

            spawnPosition = new Vector3(x, y, 0);

            distance = spawnPosition.magnitude;
            
            if(distance >= 3 && distance < 20)
                break;
        }

        targetPosition = transform.position + spawnPosition;

        GameObject fallingNote = Instantiate(fallingPrefab, targetPosition + Vector3.up * 3, Quaternion.identity);
        fallingNote.GetComponent<FallingNote>().Initialize(targetPosition, fallDuration);
    }
}
