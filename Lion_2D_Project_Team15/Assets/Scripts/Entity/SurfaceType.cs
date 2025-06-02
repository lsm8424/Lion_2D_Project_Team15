using UnityEngine;

// 어떤 지면인지와 해당 발걸음 소리를 한곳에 묶어두는 컴포넌트
public class SurfaceType : MonoBehaviour
{
    [Tooltip("이 지면을 밟았을 때 사용할 발걸음 효과음")]
    public AudioClip footstepClip;
}
