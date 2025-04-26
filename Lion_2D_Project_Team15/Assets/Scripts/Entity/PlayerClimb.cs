using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed; // ��ٸ� ���������� �ӵ�

    private void Update()
    {
        // �÷��̾ ���� ��ٸ� ������ ��쿡�� ����
        if (Player.Instance.interaction.IsOnLadder())
        {
            // W/S Ű �Է� �ޱ�
            float v = Input.GetAxisRaw("Vertical"); // W/S �Է�
            // �Է� �������� ��/�Ʒ� �̵� ���� ���
            Vector3 climbDir = new Vector3(0, v, 0);
            // �ش� �������� �̵� (�߷� ���� ���� �̵�)
            transform.Translate(climbDir * climbSpeed * Time.deltaTime);
            // ���� �ö�ź ��ٸ� ���� ��������
            Ladder ladder = Player.Instance.interaction.GetCurrentLadder();
            // ��ٸ� ��ü�� �ְ�, �÷��̾ ������ ���� �̻� �ö󰡸� �ڵ� Ż��
            if (ladder != null & transform.position.y >= ladder.topExitY)
            {
                Player.Instance.interaction.ForceExitLadder(); // ��ٸ� Ż�� ó��
            }
        }
    }
}
