using UnityEngine;

/// <summary>
/// ��Ż�� ���� ����� �����ϴ� ������
/// </summary>
public enum PortalType
{
    SceneChange,     // �ٸ� ������ �̵�
    PositionChange   // ���� �� �� ��Ż ��ġ�� �̵�
}

/// <summary>
/// ��Ż ������ �����ϴ� ������Ʈ
/// </summary>
public class Portal : MonoBehaviour
{
    public PortalType portalType;        // ��Ż ���� Ÿ�� ����

    [Header("��Ż �ε��� ����")]
    public int MapIndex;              // ���� ��Ż�� ���� ���� �ε��� (�� �̵� �� ���)
    public int portalIndex;              // ���� ��Ż�� ���� �ε���

    [Header("��ġ �̵���")]
    public int targetPortalIndex;        // �̵��� ��Ż �ε���

    [Header("�� �̵���")]
    public string targetSceneName;       // �̵��� �� �̸�

    [Header("����")]
    public Transform targetPortal;       // ������ ��Ż Transform (������ ������)

    private void Start()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.RegisterPortal(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        switch (portalType)
        {
            case PortalType.SceneChange:
                if (string.IsNullOrEmpty(targetSceneName))
                {
                    Debug.LogError($"[Portal] TargetSceneName�� ��� �ֽ��ϴ�. ({gameObject.name})");
                    return;
                }
                // �� �̵� ��û (��ǥ ��Ż �ε��� ����)
                StageManager.Instance.TeleportScene(targetSceneName);
                break;

            case PortalType.PositionChange:
                // ���� �� �� ��Ż �̵�
                StageManager.Instance.TeleportToPortal(targetPortalIndex);
                break;
        }
    }
}