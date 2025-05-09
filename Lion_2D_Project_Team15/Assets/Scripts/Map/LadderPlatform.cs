using UnityEngine;

public class LadderPlatform : MonoBehaviour
{
    GameObject player;
    [SerializeField] private Collider2D collider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        TriggerOnOff();
    }

    private void TriggerOnOff()
    {
        if (player == null)
            return;

        var interaction = player.GetComponent<PlayerInteraction>();
        // 수정: 사다리 위에 있거나, 아래 방향키 입력 시 트리거 활성화
        collider.isTrigger = interaction.IsOnLadder() || interaction.IsPressingDown;
    }
}
