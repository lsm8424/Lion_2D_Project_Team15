using UnityEngine;

public class Stage2_Boss_Pattern2 : MonoBehaviour
{
    [Header("Pattern 1 (웨이브 공격)")]
    [SerializeField] private GameObject waveSkillPrefab; // 웨이브 프리팹
    [SerializeField] private float moveSpeed; // 이동속도
    [SerializeField] private int waveCount; // 웨이브 개수
    [SerializeField] private float nockBackForce; // 넉백 힘
    [SerializeField] private float coolTime; // 쿨타임
    [SerializeField] private float damage; // 공격력

    [Header("Warning Box")]
    [SerializeField] private GameObject warningBoxPrefab;   // 경고 박스 프리팹
    [SerializeField] private float warningTime; // 경고 시간
    private bool isWarning = false; // 경고 중인지 여부

    private float delta;
    GameObject player; // 플레이어 오브젝트

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트 찾기
    }

    void Update()
    {
        delta -= Time.deltaTime;
        if (delta <= 0)
        {
            SpawnWarning();
            Invoke("CoolTimeSet", warningTime); // 1초 후에 쿨타임 초기화
        }
    }

    void SpawnWarning()
    {
        if (player == null || isWarning) return;

        isWarning = true; // 경고 중으로 설정

        int ranCount = Random.Range(1, waveCount + 1); // 랜덤으로 몹 개수 정하기

        GameObject warning = Instantiate(warningBoxPrefab, transform.position, Quaternion.identity);
        warning.GetComponent<Stage2_Boss_WarningBox>().Initialize(player.transform, transform.position, ranCount, warningTime, () =>
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            GameObject loachSkill = Instantiate(waveSkillPrefab, warning.transform.position, Quaternion.identity);
            loachSkill.GetComponent<Stage2_Boss_WaveSkill>().SetWave(angle, moveSpeed, ranCount,
                nockBackForce, damage, transform.position);
        });

    }

    void CoolTimeSet()
    {
        isWarning = false; // 경고 중이 아님
        delta = coolTime;
    } // 쿨타임 초기화

}
