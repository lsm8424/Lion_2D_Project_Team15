using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    public Animator animator; // 웨이브 애니메이터
    public AudioClip crashSound; // 충돌 사운드
    public float value;

    private void OnAnimatorMove()
    {
        animator.SetBool("Move", true); // 웨이브 애니메이션 시작
    }

    public void TriggerCrash()
    {
        Stage2_Boss_Audio.Instance.PlayOneShot(crashSound, value); // 충돌 사운드 재생
        animator.SetTrigger("Crash"); // 웨이브 애니메이션 트리거
    }

    private void OnDestroy()
    { 
        Destroy(gameObject);
    }

}
