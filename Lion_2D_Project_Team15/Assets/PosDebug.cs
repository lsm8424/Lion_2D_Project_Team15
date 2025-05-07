using UnityEngine;

public class PosDebug : MonoBehaviour
{
    void Start() { }

    void Update()
    {
        Debug.Log("Player Pos: " + transform.position);
    }
}
