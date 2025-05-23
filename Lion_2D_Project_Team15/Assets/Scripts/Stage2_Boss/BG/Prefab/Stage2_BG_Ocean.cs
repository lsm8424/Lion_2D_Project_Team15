using Unity.VisualScripting;
using UnityEngine;

public class Stage2_BG_Ocean : MonoBehaviour
{
    private int keyInputCount; // 키 입력 개수
    private float keyDuration; // 키 입력 시간
    private float growSpeed; // 성장 속도
    bool triggerEnter = false; // 트리거에 들어갔는지 여부

    private float damage; // 실패 시 데미지
    private Vector3 targetScale = new Vector3(20, 20, 1); // 목표 크기

    [Header("사운드 효과")]
    public AudioClip trueSound; //성공소리
    public AudioClip falseSound; //실패소리

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance.EntityTimeScale == 0) return;

        transform.localScale += Vector3.one * growSpeed * Time.deltaTime; // 크기 증가

        if (transform.localScale.x >= targetScale.x)
        {
            Destroy(gameObject); // 목표 크기에 도달하면 오브젝트 삭제
        }
    }

    public void SetOcean(int keyinputcount, float keyduration, float rot, float growspeed, float damage)
    {
        keyInputCount = keyinputcount; // Set the number of key inputs
        keyDuration = keyduration; // Set the duration of key inputs
        transform.rotation = Quaternion.Euler(0, 0, rot); // Set the rotation of the wave
        growSpeed = growspeed; // Set the growth speed
        this.damage = damage; // Set the damage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerEnter) return;

        if (collision.CompareTag("Player"))
        {
            //if (collision.GetComponent<move>().isStuck || collision.GetComponent<move>().isKeyInput) return;

            //회오리 순간이동 상태거나 키입력 상태일 때는 return
            if (Player.Instance.isStuck || Player.Instance.isKeyInput) return;

            triggerEnter = true;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.enabled = false; // 스프라이트 렌더러 비활성화

            //collision.GetComponent<move>().isKeyInput = true; // 플레이어를 회오리 상태로 설정
            Player.Instance.isKeyInput = true; // 플레이어를 회오리 상태로 설정

            KeyInputManager.Instance.StartKeyInput(keyInputCount, keyDuration, () =>
            {
                // 성공 시:
                Debug.Log("키입력 성공!"); // 성공 시 처리
                Stage2_Boss_Audio.Instance.PlayOneShot(trueSound, 0.2f); // 성공 소리 재생
                //collision.GetComponent<move>().isKeyInput = false;
                Player.Instance.isKeyInput = false; // 플레이어를 회오리 상태 복원
            },
             () =>
             {
                 // 실패 시: 데미지 + 스턴 등
                 Debug.Log("키입력 실패! + 해류 + 스턴"); // 실패 시 처리
                 Stage2_Boss_Audio.Instance.PlayOneShot(falseSound, 0.2f); // 실패 소리 재생
                 //collision.GetComponent<move>().isKeyInput = false;
                 Player.Instance.isKeyInput = false; // 플레이어를 회오리 상태 복원
                 Player.Instance.TakeDamage(damage); // 플레이어에게 데미지
                 Player.Instance.Stun();      //플레이어 스턴
             });
        }
    }
}
