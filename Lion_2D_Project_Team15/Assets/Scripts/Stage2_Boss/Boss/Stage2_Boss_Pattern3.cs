using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2_Boss_Pattern3 : MonoBehaviour
{
    [Header("Pattern3(TrapZone : 키입력)")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float lifeTime;    //트랩 지속시간
    [SerializeField] private float coolDown;    //쿨타임
    [SerializeField] private int keyInputCount; //키입력 개수
    [SerializeField] private float trapSize;    //트랩 사이즈
    [SerializeField] private float keyDuration; //키입력 시간
    [SerializeField] private float damage;      //트랩 데미지    

    //랜덤한 위치 정하기
    private float maxX = 18;
    private float maxY = 18;

    private bool isPatternActive = false;
    private float delta = 5;

    private void Update()
    {
        if (!isPatternActive)
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
        isPatternActive = true;

        Vector3 randomSpawn = Vector3.zero;

        while(true)
        {
            randomSpawn = new Vector3(Random.Range(0, maxX), Random.Range(0, maxY), 0);
            float distance = Vector3.Distance(randomSpawn, Vector3.zero);
            if(distance >4 && distance < 18)
                break;
        }

        GameObject wave = Instantiate(trapPrefab, transform.position + NearPlayerSpawn(randomSpawn), Quaternion.identity);

        wave.GetComponent<Stage2_Boss_TrapSkill>().SetWave(lifeTime, trapSize, damage, keyInputCount, keyDuration);

        Invoke("TimeSet", lifeTime);
    }

    private Vector3 NearPlayerSpawn(Vector3 pos)
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 direction = playerPos - transform.position;

        Vector3 newdirection = Vector3.zero;

        if(direction.x <0)
            newdirection.x = -pos.x;
        else
            newdirection.x = pos.x;
        if (direction.y < 0)
            newdirection.y = -pos.y;
        else
            newdirection.y = pos.y;

        return newdirection;
    }

    private void TimeSet()
    {
        isPatternActive = false;
        delta = coolDown;
    }

}
