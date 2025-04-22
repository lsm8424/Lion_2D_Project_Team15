using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �� �̵� �� ��Ż ��ġ �̵��� ó���ϴ� �̱��� �Ŵ���
/// </summary>
public class StageManager : MonoBehaviour
{
    #region singleton
    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        // �ν��Ͻ��� ���� ��� �ڱ� �ڽ��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���� �� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

    // Player ĳ�̿� ������Ƽ
    private GameObject player;
    private GameObject Player => player ??= GameObject.FindGameObjectWithTag("Player");

    // ��Ż �ε��� �� ��Ż ��ü ���� ��ųʸ�
    private Dictionary<int, Portal> portalDict = new();
    // �� ��ȯ �� ��Ż ����� ���� ��ųʸ� �ʱ�ȭ



    /// <summary>
    /// �� �� ��Ż�� �ڽ��� ����� �� �ֵ��� ����
    /// </summary>
    public void RegisterPortal(Portal portal)
    {
        if (!portalDict.ContainsKey(portal.portalIndex))
        {
            portalDict.Add(portal.portalIndex, portal);
        }
        else
        {
            Debug.LogWarning($"[StageManager] ��Ż �ε��� �ߺ� ��� �õ�: {portal.portalIndex}");
        }
    }

    /// <summary>
    /// ���� �� �� ��Ż�� �̵�
    /// </summary>
    public void TeleportToPortal(int targetIndex)
    {
        if (!portalDict.TryGetValue(targetIndex, out Portal targetPortal))
        {
            Debug.LogError($"[StageManager] �̵� ����: �ε��� {targetIndex}�� ��Ż�� ã�� �� �����ϴ�.");
            return;
        }

        if (Player == null)
        {
            Debug.LogError("[StageManager] Player ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        Player.transform.position = targetPortal.targetPortal.position;
    }

    /// <summary>
    /// �ٸ� ������ �̵��ϰ�, �ش� �� �� ��Ż �ε��� ��ġ�� �̵�
    /// </summary>
    public void TeleportScene(string sceneName, int spawnPortalIndex = 0)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[StageManager] �� '{sceneName}' ��(��) �ε��� �� �����ϴ�. Build Settings�� ��ϵǾ� �ִ��� Ȯ���ϼ���.");
            return;
        }

        StartCoroutine(LoadSceneAndTeleport(sceneName, spawnPortalIndex));
    }

    /// <summary>
    /// �� �񵿱� �ε� + ���� ��Ż ��ġ�� �̵� ó��
    /// </summary>
    private IEnumerator LoadSceneAndTeleport(string sceneName, int spawnPortalIndex)
    {
        portalDict.Clear(); // ���� �� ��Ż ������ �ʱ�ȭ

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // �� ��ȯ �� ��Ż ��� ��� (1������)
        yield return new WaitForEndOfFrame();

        player = GameObject.FindGameObjectWithTag("Player"); // �� ������ �÷��̾� ��Ž��

        if (!portalDict.TryGetValue(spawnPortalIndex, out Portal spawnPortal))
        {
            Debug.LogWarning($"[StageManager] ���� ��Ż �ε��� {spawnPortalIndex}�� ã�� �� �����ϴ�.");
            yield break;
        }

        if (Player != null)
        {
            Player.transform.position = spawnPortal.targetPortal.position;
        }
    }
}
