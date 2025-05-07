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
    private float maxX = 15;
    private float maxY = 15;

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

        float randomX = Random.Range(-maxX, maxX);
        float randomY = Random.Range(-maxY, maxY);

        //두 숫자가 절대값 3보다 작지 않을 때 까지 반복
        while (Mathf.Abs(randomX) > 3 && Mathf.Abs(randomY) > 3)
        {
            randomX = Random.Range(-maxX, maxX);
            randomY = Random.Range(-maxY, maxY);
        }

        GameObject wave = Instantiate(trapPrefab, transform.position + new Vector3(randomX, randomY, 0), Quaternion.identity);

        wave.GetComponent<Stage2_Boss_TrapSkill>().SetWave(lifeTime, trapSize, damage, keyInputCount, keyDuration);

        Invoke("TimeSet", lifeTime);
    }

    private void TimeSet()
    {
        isPatternActive = false;
        delta = coolDown;
    }

}
