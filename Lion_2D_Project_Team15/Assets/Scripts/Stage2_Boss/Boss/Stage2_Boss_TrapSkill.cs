using System.Collections;
using UnityEngine;

public class Stage2_Boss_TrapSkill : MonoBehaviour
{
    private float lifeTime;
    private float trapSize;
    private float damage;

    private bool triggerEnter = false;
    private float stuckDuration;  // 흡수되는 시간
    private float stuckTimer = 0f;

    private Vector3 teleportTarget;  // 순간이동할 위치
    public GameObject teleportEffect;

    private Transform player = null;

    void Start()
    {
        transform.localScale = new Vector3(trapSize, trapSize, 1); // Set the size of the wave
    }

    void Update()
    {
        //플레이어가 트랩에 들어오지 않았을 때만 시간 감소
        if (!triggerEnter)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject); // Destroy the wave after its lifetime
            }
        }
        else if (player != null)
        {
            stuckTimer += Time.deltaTime;
            float t = stuckTimer / stuckDuration;

            // 플레이어를 트랩 중앙으로 천천히 끌어당김
            player.position = Vector3.Lerp(player.position, transform.position, t);

            if (stuckTimer >= stuckDuration)
            {
                StartCoroutine(TeleportEffect()); // 순간이동 이펙트 실행
            }
        }
    }

    IEnumerator TeleportEffect()
    {
        GameObject effect = Instantiate(teleportEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // 이펙트 애니메이션 길이만큼 대기
        yield return null;
        player.position = teleportTarget;
        yield return null;
        GameObject effect2 = Instantiate(teleportEffect, teleportTarget, Quaternion.identity);
        Destroy(effect2, effect2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // 이펙트 애니메이션 길이만큼 대기

        player.GetComponent<move>().isStuck = false; // 플레이어를 회오리 상태로 설정

        // 상태 해제 및 데미지
        //Player.Instance.isStuck = true; // 플레이어를 회오리 상태로 설정

        Destroy(gameObject);
    }

    public void SetWave(float lifetime, float trapsize, float damage, float stuckduration, Vector3 teleportTarget)
    {
        lifeTime = lifetime;
        trapSize = trapsize;
        this.damage = damage;
        stuckDuration = stuckduration;
        this.teleportTarget = teleportTarget;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerEnter) return;

        //플레이어가 회오리 가운데로 빨려들어가며 순간이동
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<move>().isStuck || collision.GetComponent<move>().isKeyInput) return;

            //회오리 순간이동 상태거나 키입력 상태일 때는 return
            //if (Player.Instance.isStuck || Player.Instance.isKeyInput)
            //{
            //    return;
            //}

            player = collision.transform;
            triggerEnter = true;

            collision.GetComponent<move>().isStuck = true; // 플레이어를 회오리 상태로 설정
            Debug.Log("회오리 데미지 + Stuck");

            // 플레이어에게 데미지 주기
            //Player.Instance.TakeDamage(damage);
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (triggerEnter) return;

    //    if (collision.CompareTag("Player"))
    //    {
    //        triggerEnter = true;

    //        KeyInputManager.Instance.StartKeyInput(keyInputCount, keyDuration, () =>
    //        {
    //            // 성공 시: 속도 원복

    //            Debug.Log("Success!"); // 성공 시 처리
    //            Destroy(gameObject); // 트랩 제거

    //        },
    //         () =>
    //         {
    //             // 실패 시: 속도 원복 + 데미지 + 스턴 등

    //             Debug.Log("Fail!"); // 실패 시 처리
    //             Destroy(gameObject); // 트랩 제거
    //         });
    //    }
    //}

}
