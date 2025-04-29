using Unity.Cinemachine;
using UnityEngine;

public class followcam : MonoBehaviour
{
    public CinemachineCamera[] cinemachineCamera;

    public void transCam(int index)
    {
        cinemachineCamera[index].gameObject.SetActive(true);

        for (int i = 0; i < cinemachineCamera.Length; i++)
        {
            if (i != index)
            {
                cinemachineCamera[i].gameObject.SetActive(false);
            }
        }
    }
}
