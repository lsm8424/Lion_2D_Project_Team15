using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2_Boss_Pattern3 : MonoBehaviour
{
    [Header("Pattern3(TrapZone : 순간이동)")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float lifeTime;    //트랩 지속시간
    [SerializeField] private float coolDown;    //쿨타임
    [SerializeField] private int trapSize;    //트랩 사이즈
    [SerializeField] private float damage;      //트랩 데미지
    [SerializeField] private float stuckDuration; //트랩에 갇히는 시간

    [Header("Warning Trap")]
    [SerializeField] private GameObject warningTrapPrefab;   // 경고 박스 프리팹
    [SerializeField] private float warningTime; // 경고 시간

    //랜덤한 위치 정하기
    private float maxX = 18;
    private float maxY = 18;

    private bool isWaveSpawn = false;
    private float delta;

    private void Start()
    {
        delta = coolDown;
    }

    private void Update()
    {
        if (!isWaveSpawn)
        {
            delta -= Time.deltaTime;
            if (delta < 0)
            {
                SpawnWave();
            }
        }
    }


    private void SpawnWave()
    {
        isWaveSpawn = true;

        Vector3 randomSpawn = Vector3.zero;

        while (true)
        {
            randomSpawn = new Vector3(Random.Range(0, maxX), Random.Range(0, maxY), 0);
            float distance = Vector3.Distance(randomSpawn, Vector3.zero);
            if (distance > 4 && distance < 18)
                break;
        }

        //트랩 생성 위치
        Vector3 spawnPos = transform.position + NearPlayerSpawn(randomSpawn);

        //플레이어가 순간이동할 위치
        Vector3 teleportTarget = transform.position +
            FlipRandomSpawn(NearPlayerSpawn(randomSpawn));

        GameObject warning = Instantiate(warningTrapPrefab, spawnPos, Quaternion.identity);
        warning.GetComponent<Stage2_Boss_TrapWarning>().Initialize(trapSize, warningTime, () =>
        {
            GameObject trap = Instantiate(trapPrefab, spawnPos, Quaternion.identity);

            trap.GetComponent<Stage2_Boss_TrapSkill>().SetWave(lifeTime, trapSize, damage, stuckDuration, teleportTarget);
        });

        Invoke("TimeSet", lifeTime);
    }

    private Vector3 NearPlayerSpawn(Vector3 pos)
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 direction = playerPos - transform.position;

        Vector3 newdirection = Vector3.zero;

        if (direction.x < 0)
            newdirection.x = -pos.x;
        else
            newdirection.x = pos.x;
        if (direction.y < 0)
            newdirection.y = -pos.y;
        else
            newdirection.y = pos.y;

        return newdirection;
    }

    private Vector3 FlipRandomSpawn(Vector3 pos)
    {
        Vector3 spawnPos = Vector3.zero;

        if (pos.x > 0)
            spawnPos.x = Random.Range(-maxX, -4);
        else
            spawnPos.x = Random.Range(4, maxX);
        if (pos.y > 0)
            spawnPos.y = Random.Range(-maxY, -4);
        else
            spawnPos.y = Random.Range(4, maxY);

        return spawnPos;
    }

    private void TimeSet()
    {
        isWaveSpawn = false;
        delta = coolDown;
    }

}
