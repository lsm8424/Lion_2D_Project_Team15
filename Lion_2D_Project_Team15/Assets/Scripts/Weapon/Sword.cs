using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetRun(bool isRunning)
    {
        anim.SetBool("Run", isRunning);
    }

    public void SetJump(bool isJumping)
    {
        anim.SetBool("Jump", isJumping);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void Flip(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
