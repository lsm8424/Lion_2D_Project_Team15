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

        collider.isTrigger = player.GetComponent<PlayerInteraction>().IsOnLadder();
    }
}
