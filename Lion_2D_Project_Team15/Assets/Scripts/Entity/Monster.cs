using UnityEngine;

public class Monster : Entity
{
    [Header("Monster Stats")]
    public float moveSpeed; 
    public float attackPower;
    public float attackCooldown; //���ݼӵ�
    private float lastAttackTime = -999f; //���� ��Ÿ���� �ٷ� �����ϵ��� �ϱ� ����.������ ���۵��ڸ��� �÷��̾ �ٷ� ������ �� �ֵ��� �ϱ� ����
                                          //"������ ���� �ð�"�� �Ϻη� ���� ���� �� �ð����� ����

    [Header("Attack Settings")]
    public float attackRange;

    private Transform player;

    public void Start()
    {
        // 'Player' �±װ� ���� ������Ʈ �ڵ� ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void Update()
    {
        if (player == null) return; //�÷��̾ ������ �ൿ X

        float distance = Vector3.Distance(transform.position, player.position); //���Ϳ� �÷��̾� ������ �Ÿ� ���

        if (distance > attackRange)
        {
            Move(); //��Ÿ� ���̸� �÷��̾ ���� �̵�
        }
        else
        {
            Attack(); //��Ÿ� �̳��� ���� �õ�
        }

    }

    public override void Move()
    {
        // �÷��̾ ���� �̵�
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // �÷��̾� �ٶ󺸰� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown) // ������ ���� ���� ��Ÿ���� ������ ���� ���� ����
        {
            lastAttackTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Attack");

            Debug.Log($"���Ͱ� �÷��̾ ����! ���ݷ�: {attackPower}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
