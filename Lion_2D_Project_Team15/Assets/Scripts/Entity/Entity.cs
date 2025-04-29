using UnityEngine;

public class Entity : MonoBehaviour
{
    public float HP;
    public Animator anim;

    public virtual void TakeDamgae(float value)
    {
        HP -= value;
        if (HP <= 0) Death();
    }

    public virtual void Move()
    {

    }
    

    protected virtual void Death()
    {
        Debug.Log($"{gameObject.name} has died.");
        //�ִϸ��̼� or ������Ʈ ���
    }
}
