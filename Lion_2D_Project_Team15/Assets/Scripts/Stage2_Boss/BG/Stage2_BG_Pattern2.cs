using UnityEngine;

public class Stage2_BG_Pattern2 : MonoBehaviour
{
    [Header("Background Pattern2 : 키 입력")]
    [SerializeField] private GameObject oceanPrefab; //해류 프리팹
    [SerializeField] private float coolTime;    //쿨타임
    [SerializeField] private int keyInputCount; //키입력 개수
    [SerializeField] private float keyDuration; //키입력 시간
    [SerializeField] private float growSpeed;   //성장 속도
    [SerializeField] private float damage;      //실패시 데미지

    [Header("Warning")]
    [SerializeField] private GameObject warningPrefab;   // 경고 박스 프리팹
    [SerializeField] private float warningTime; // 경고 시간

    float delta; //쿨타임
    bool isActive = false; //키입력 상태

    void Start()
    {
        delta = coolTime;
    }

    void Update()
    {
        if (isActive)
            return;

        delta -= Time.deltaTime;

        if (delta < 0)
        {
            SpawnOcean();
            delta = coolTime;
        }
    }

    void SpawnOcean()
    {
        isActive = true;

        float rot = RandomRot();

        GameObject warning = Instantiate(warningPrefab, transform.position, Quaternion.identity);
        warning.GetComponent<Stage2_BG_WarningSector>().Initialize(rot, warningTime, () =>
        {
            GameObject ocean = Instantiate(oceanPrefab, transform.position, Quaternion.identity);

            ocean.GetComponent<Stage2_BG_Ocean>().SetOcean(keyInputCount, keyDuration, rot, growSpeed, damage);

            isActive = false; //키입력 상태 해제   
        });
    }

    float RandomRot()
    {
        int random = Random.Range(0, 4);

        float rot = 0f;

        switch (random)
        {
            case 0: rot = 0; break;
            case 1: rot = 90; break;
            case 2: rot = 180; break;
            case 3: rot = 270; break;
        }

        return rot;
    }
}
