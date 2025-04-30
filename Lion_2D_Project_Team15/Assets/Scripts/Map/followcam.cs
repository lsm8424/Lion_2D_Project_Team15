using Unity.Cinemachine;
using UnityEngine;

public class followcam : MonoBehaviour
{
    public CinemachineCamera[] cinemachineCamera;

    public void transCam(int index)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //카메라 타겟 설정
        cinemachineCamera[index].Follow = player.transform;

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
