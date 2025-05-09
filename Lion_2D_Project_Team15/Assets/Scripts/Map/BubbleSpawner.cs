using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float spawnInterval = 0.5f;
    public float minX = -2.0f;
    public float maxX = 2.0f;
    public float minY = -3.0f;
    public float maxY = -1.0f;
    public float minHeight = 2.0f;
    public float maxHeight = 5.0f;

    public int maxBubbleCount = 10; // ★ 최대 버블 개수 지정

    private float timer;
    private List<GameObject> bubbles = new List<GameObject>();

    void Update()
    {
        // 살아있는 버블 개수가 최대치보다 작을 때만 생성
        if (bubbles.Count < maxBubbleCount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnBubble();
                timer = 0f;
            }
        }

        // 리스트에서 사라진 버블 정리
        bubbles.RemoveAll(b => b == null);
    }

    void SpawnBubble()
    {
        float x = Random.Range(minX, maxX);
        float bubbleMinY = Random.Range(minY, maxY);
        float height = Random.Range(minHeight, maxHeight);
        float bubbleMaxY = bubbleMinY + height;

        Vector3 pos = new Vector3(x, bubbleMinY, 0);
        GameObject bubbleObj = Instantiate(bubblePrefab, pos, Quaternion.identity);

        Bubble bubble = bubbleObj.GetComponent<Bubble>();
        if (bubble != null)
        {
            bubble.Setup(bubbleMinY, bubbleMaxY);
        }

        bubbles.Add(bubbleObj);
    }
}
