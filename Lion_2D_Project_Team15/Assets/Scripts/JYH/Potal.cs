using UnityEngine;

public class Potal : MonoBehaviour
{
    public int stageIndex = 0;
    public int nextStage = 1;
    private GameObject player;

    private GameObject nextpotal;
    public Transform SpawnPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //stageIndex = Stage.Instance.currentStage;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        // Teleport the player to the next stage
        // You can use SceneManager.LoadScene(nextStage) if you are using Unity's scene management
        // Or you can use a custom method to load the next stage
        FindPotal();
        player.transform.position = nextpotal.GetComponent<Potal>().SpawnPos.position;
    }

    private void FindPotal()
    {
        // Find the portal in the scene
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Potal");
        foreach (GameObject portal in portals)
        {
            if(portal.GetComponent<Potal>().stageIndex == nextStage)
            {
                nextpotal = portal;
            }
        }
    }
}
