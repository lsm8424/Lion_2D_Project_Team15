using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    public Animator animator; // 웨이브 애니메이터

    private void OnAnimatorMove()
    {
        animator.SetBool("Move", true); // 웨이브 애니메이션 시작
    }

    public void TriggerCrash()
    {
        animator.SetTrigger("Crash"); // 웨이브 애니메이션 트리거
    }

    private void OnDestroy()
    { 
        Destroy(gameObject);
    }

}
