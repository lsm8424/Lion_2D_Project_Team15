using TMPro;
using UnityEngine;

public class Stage2_BG_Pattern1 : MonoBehaviour
{
    [Header("떨어지는 음표")]
    public GameObject fallingPrefab;
    public GameObject shadowPrefab;
    [SerializeField] private float fallingCoolTime;
    [SerializeField] private float fallDuration;
    [SerializeField] private int maxX;
    [SerializeField] private int maxY;
    public bool isOn;

    [Header("사운드 효과")]
    public AudioClip fallNoteSound;
    public float value;

    private Vector3 targetPosition;
    private float delta;

    void Start()
    {
        delta = fallingCoolTime;
    }

    void Update()
    {
        if (isOn == false)
            return;

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

        GameObject fallingNote = Instantiate(fallingPrefab);

        Stage2_Boss_Audio.Instance.PlayOneShot(fallNoteSound, value);

        fallingNote.GetComponent<FallingNote>().Initialize(targetPosition, fallDuration);
        GameObject shadow = Instantiate(shadowPrefab);
        shadow.GetComponent<GrowShadow>().SetShadow(targetPosition, fallDuration);
        
    }
}
