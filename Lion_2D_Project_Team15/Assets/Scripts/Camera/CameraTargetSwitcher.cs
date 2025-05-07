using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetSwitcher : MonoBehaviour
{
    public CinemachineCamera virtualCam;
    public CinemachinePositionComposer composer;
    public Transform playerTarget;
    public Transform defaultTarget;

    public void SetFollowToPlayer()
    {
        virtualCam.Follow = playerTarget;
        composer.TargetOffset = new Vector3(-3f, 0, 0);
    }

    public void ResetFollow()
    {
        virtualCam.Follow = defaultTarget;
        composer.TargetOffset = new Vector3(5f, 0, 0);
    }
}
