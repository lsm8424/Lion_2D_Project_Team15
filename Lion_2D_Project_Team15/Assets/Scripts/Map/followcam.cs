using UnityEngine;

public class followcam : MonoBehaviour
{
    public GameObject[] Maps;

    public void transCam(int index)
    {
        this.transform.position = Maps[index].transform.position + new Vector3(0,0,-10);
    }
}
