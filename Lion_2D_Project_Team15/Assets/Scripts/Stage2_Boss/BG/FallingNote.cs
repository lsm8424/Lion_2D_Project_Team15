using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingNote : MonoBehaviour
{
    private float fallDuration;     //떨어지는 시간
    private Vector3 startScale;
    private Vector3 endScale = new Vector3(0.1f, 0.1f, 0.1f);

    private float timer = 0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    [Header("타일이 삭제되는 효과")]
    public GameObject disappearTilePrefab; //사라진 타일 프리팹
    public GameObject EffectPrefab;
    public float groundDisappearTime;

    private bool alreadyHit = false;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / fallDuration;

        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        if (t >= 0.99f)
        {
            // 낙하 완료 - 충돌 판정 활성화
            GetComponent<Collider2D>().enabled = true;
        }
    }

    public void Initialize(Vector3 position, float fallduration)
    {
        targetPosition = position;
        startPosition = position + new Vector3(0f, 5f, 0f); // 높이 조절 (탑뷰 상 가상의 위)
        fallDuration = fallduration;
        transform.position = startPosition;
        transform.localScale = startScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyHit) return;

        if (collision.CompareTag("Player"))
        {
            // 데미지 처리
            Debug.Log("플레이어가 음표에 맞음!");
            alreadyHit = true;
            Destroy(gameObject);
            return;
        }

        else if (collision.CompareTag("Ground"))
        {
            StartCoroutine(DelayedGroundHit());
        }
    }
    private IEnumerator DelayedGroundHit()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 대기

        if (alreadyHit) yield break; // 그 사이에 Player에 맞았으면 무시

        alreadyHit = true;

        Debug.Log("바닥에 닿음!");

        GameObject effect = Instantiate(EffectPrefab, targetPosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);

        GameObject tile = Instantiate(disappearTilePrefab, targetPosition, Quaternion.identity);
        Destroy(tile, groundDisappearTime);

        Destroy(gameObject);
    }



}
