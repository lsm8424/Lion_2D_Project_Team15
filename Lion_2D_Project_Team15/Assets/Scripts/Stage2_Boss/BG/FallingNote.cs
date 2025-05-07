using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingNote : MonoBehaviour
{
    public Transform shadow;           // 미리 바닥에 위치한 그림자 오브젝트
    private float fallDuration;     //떨어지는 시간
    private Vector3 startScale;
    private Vector3 endScale = new Vector3(0.1f, 0.1f, 0.1f);

    private float timer = 0f;
    private Vector3 targetPosition;

    [SerializeField] float groundDisappearTime = 3f;
    private Tilemap groundTilemap;


    void Start()
    {
        Destroy(gameObject, fallDuration);

        groundTilemap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / fallDuration;

        transform.position = Vector3.Lerp(transform.position, shadow.position, t);
        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        if (t >= 0.99f)
        {
            // 낙하 완료 - 충돌 판정 활성화
            GetComponent<Collider2D>().enabled = true;
            Destroy(shadow.gameObject); // 그림자 제거
            this.enabled = false; // 더 이상 업데이트하지 않음
        }
    }

    public void Initialize(Vector3 position, float fallduration)
    {
        targetPosition = position;
        transform.position = position + new Vector3(0f, 2f, 0f); // 높이 조절 (탑뷰 상 가상의 위)
        shadow.position = position;
        transform.localScale = startScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 hitPos = collision.GetContact(0).point;
            Vector3Int cell = groundTilemap.WorldToCell(hitPos);

            TileBase original = groundTilemap.GetTile(cell);
            if (original != null)
            {
                groundTilemap.SetTile(cell, null); // 타일 제거
                StartCoroutine(RestoreTile(cell, original));
            }

            Destroy(gameObject); // 낙하물 제거
        }
    }

    IEnumerator RestoreTile(Vector3Int cell, TileBase tile)
    {
        yield return new WaitForSeconds(groundDisappearTime);
        groundTilemap.SetTile(cell, tile); // 타일 복원
    }
}
