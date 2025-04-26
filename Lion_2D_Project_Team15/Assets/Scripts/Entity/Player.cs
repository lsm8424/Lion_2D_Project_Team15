using UnityEngine;

public class Player : Entity
{
    // ���������������������������� Singleton ����������������������������

    // Player �ν��Ͻ��� �������� ���� �����ϵ��� static���� ����
    public static Player Instance { get; private set; } 

    private void Awake()
    {
        // ���� Player�� �̹� �����Ѵٸ� ���� ������Ʈ�� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; // �ν��Ͻ� ���
        }
    }

    // ��ɺ� ��� ��ũ��Ʈ ���� (�ܺο��� Player.Instance.movement ó�� ��� ����)
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerInteraction interaction;

    private void Start()
    {
        // Player�� �پ��ִ� ��ɺ� ��ũ��Ʈ�� ������
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        // �� ��� ����� �ż��� ����
        movement.HandleMove(); // �̵�
        movement.HandleJump(); // ����
        combat.HandleAttack(); // �⺻ ���� (��Ŭ��)
        combat.HandleSkill(); // ��ų ���� (��Ŭ��)
        interaction.HandleInteraction(); // F Ű ��ȣ�ۿ� (NPC, ������ ��)
    }
}
