using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 이동 및 포탈 위치 이동을 처리하는 싱글톤 매니저
/// </summary>
public class StageManager : Singleton<StageManager>
{
    #region singleton

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(FindPlayerWithRetry());
    }

    private IEnumerator FindPlayerWithRetry()
    {
        float timeout = 5f; // Maximum time to wait
        float elapsed = 0f;

        while (elapsed < timeout)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log("[StageManager] Player 오브젝트를 찾았습니다.");
                break;
            }

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (player == null)
        {
            Debug.LogError("[StageManager] Player 오브젝트 등록에 실패하였습니다. (timeout: " + timeout + "초)");
        }
    }
    #endregion

    /// <summary>
    /// Player 태그를 가진 오브젝트를 찾아서 player 변수에 할당
    /// </summary>
    /// <returns>player 오브젝트를 찾았으면 true, 못찾았으면 false 반환</returns>
    public bool AssignPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[StageManager] Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return false;
        }

        Debug.Log("[StageManager] Player 오브젝트를 찾아서 할당했습니다: " + player.name);
        return true;
    }

    // Player 캐싱용 프로퍼티
    public GameObject player;

    // 포탈 인덱스 → 포탈 객체 매핑 딕셔너리
    private Dictionary<int, Portal> portalDict = new();

    // 씬 전환 시 포탈 등록을 위한 딕셔너리 초기화



    /// <summary>
    /// 씬 내 포탈이 자신을 등록할 수 있도록 제공
    /// </summary>
    public void RegisterPortal(Portal portal)
    {
        if (!portalDict.ContainsKey(portal.portalIndex))
        {
            portalDict.Add(portal.portalIndex, portal);
        }
        else
        {
            Debug.LogWarning($"[StageManager] 포탈 인덱스 중복 등록 시도: {portal.portalIndex}");
        }
    }

    /// <summary>
    /// 같은 씬 내 포탈 위치로 이동 (페이드 포함)
    /// </summary>
    public void TeleportToPortal(int targetIndex)
    {
        StartCoroutine(FadeAndTeleport(targetIndex)); // 페이드 포함 이동 처리
    }

    /// <summary>
    /// 위치 이동 전후로 페이드 인/아웃 적용
    /// </summary>
    private IEnumerator FadeAndTeleport(int targetIndex)
    {
        Fade fadeIn = new Fade(Color.clear, Color.black, 0.5f); // 페이드 인
        yield return fadeIn.Execute();

        if (!portalDict.TryGetValue(targetIndex, out Portal targetPortal))
        {
            Debug.LogError($"[StageManager] 이동 실패: 인덱스 {targetIndex} 포탈을 찾을 수 없습니다.");
            yield break;
        }

        if (player == null)
        {
            Debug.LogError("[StageManager] Player가 없습니다.");
            yield break;
        }

        // 플레이어 위치 이동
        player.transform.position = targetPortal.targetPortal.position;
        Debug.Log(
            $"[StageManager] 포탈 {targetIndex}로 이동 완료. 위치: {targetPortal.targetPortal.position}"
        );

        // 카메라 설정
        var followCam = Camera.main.GetComponent<followcam>();
        if (followCam != null)
        {
            followCam.transCam(targetPortal.MapIndex);
        }

        // 약간의 지연 후 페이드 아웃
        yield return new WaitForSeconds(0.1f);

        Fade fadeOut = new Fade(Color.black, Color.clear, 0.5f); //페이드 아웃
        yield return fadeOut.Execute();
    }

    /// <summary>
    /// 포탈을 통해 다른 씬으로 이동 요청 (SceneController 이용, 수정 없음)
    /// </summary>
    public void TeleportScene(string sceneName, int spawnPortalIndex)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[StageManager] 씬 '{sceneName}' 을(를) 로드할 수 없습니다.");
            return;
        }

        // 기존 SceneController의 LoadSceneWithFadeInOut만 호출
        SceneController.Instance.LoadSceneWithFadeInOut(sceneName, 0.5f);

        //씬 전환전 포탈 초기화
        portalDict.Clear();

        // 별도로 코루틴 돌려서 포탈 이동까지 관리
        StartCoroutine(HandleAfterSceneLoad(spawnPortalIndex, sceneName));
    }

    /// <summary>
    /// 씬 로딩 완료 후 포탈 이동 처리 (SceneController는 수정 안함)
    /// </summary>
    private IEnumerator HandleAfterSceneLoad(int spawnPortalIndex, string sceneName)
    {
        //현재 활성화된 씬과 씬이름이 일치할때까지 대기
        while (SceneManager.GetActiveScene().name != sceneName)
            yield return null;

        yield return new WaitForSeconds(0.3f); // 포탈 등록 대기

        if (!AssignPlayer()) // 새로운 함수 사용
        {
            Debug.LogError("[StageManager] 씬 로드 후 Player를 찾을 수 없어서 포탈 이동을 취소합니다.");
            yield break;
        }

        if (sceneName == "EP_2_Boss")
        {
            Player.Instance.movement.isBossRound = true;
            Player.Instance.combat.hasCoralStaff = true;
        }

        if (!portalDict.TryGetValue(spawnPortalIndex, out Portal spawnPortal))
        {
            Debug.LogWarning($"[StageManager] 도착 포탈 인덱스 {spawnPortalIndex}를 찾을 수 없습니다.");
            yield break;
        }

        if (player != null)
        {
            player.transform.position = spawnPortal.targetPortal.position;
            //카메라 임시 설정
            Camera.main.GetComponent<followcam>().transCam(spawnPortal.MapIndex);
        }
    }
}
