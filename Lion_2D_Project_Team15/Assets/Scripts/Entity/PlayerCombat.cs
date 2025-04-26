using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackPower; // �⺻ ������ ������
    public float attackCooldown; // �⺻ ���� ��Ÿ��
    public float skillCooldown; // ��ų ��Ÿ��

    public float attackRange = 1.5f; // ���� ����

    private float lastAttackTime = -999f; // ������ ���� �ð� (ó������ ���� �����ϰ� �ʱ�ȭ)
    private float lastSkillTime = -999f; // ������ ��ų ��� �ð�

    private Animator anim; // �ִϸ����� ������Ʈ

    private void Start()
    {
        anim = GetComponent<Animator>(); // �� ������Ʈ�� Animator ��������
    }

    public void HandleAttack()
    {
        // ���콺 ��Ŭ�� & ��Ÿ�� üũ
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // ���� �ð� ����
            if (anim != null)
                anim.SetTrigger("Attack"); // �ִϸ��̼� ����

            Debug.Log("�⺻ ����! ���ݷ�: " + attackPower); 
        }
    }

    public void HandleSkill()
    {
        // ���콺 ��Ŭ�� & ��Ÿ�� üũ
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCooldown)
        {
            lastSkillTime = Time.time; // ��ų �ð� ����
            if (anim != null)
                anim.SetTrigger("Skill");

            Debug.Log("��ų ���!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Scence �信�� �������� �� ���� ������ ���� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
