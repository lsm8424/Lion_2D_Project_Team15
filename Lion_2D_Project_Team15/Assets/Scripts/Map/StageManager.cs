using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 씬 이동 및 포탈 위치 이동을 처리하는 싱글톤 매니저
/// </summary>
public class StageManager : Singleton<GameManager>
{
    #region singleton
    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        // 인스턴스가 없을 경우 자기 자신을 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시 유지
        }
        else
        {
            Destroy(gameObject); // 중복 제거
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

    // Player 캐싱용 프로퍼티
    private GameObject player;
    private GameObject Player => player ??= GameObject.FindGameObjectWithTag("Player");

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
    /// 같은 씬 내 포탈로 이동
    /// </summary>
    public void TeleportToPortal(int targetIndex)
    {
        if (!portalDict.TryGetValue(targetIndex, out Portal targetPortal))
        {
            Debug.LogError($"[StageManager] 이동 실패: 인덱스 {targetIndex}의 포탈을 찾을 수 없습니다.");
            return;
        }

        if (Player == null)
        {
            Debug.LogError("[StageManager] Player 오브젝트를 찾을 수 없습니다.");
            return;
        }

        Player.transform.position = targetPortal.targetPortal.position;
    }
    //    //페이드 인아웃 + 플레이어 이동
    //    StartCoroutine(FadeAndTeleport(targetPortal));
    //}

    //private IEnumerator FadeAndTeleport(Portal targetPortal)
    //{
    //    //yield return SceneController.Instance.FadeIn(0.5f); // 페이드 인

    //    Player.transform.position = targetPortal.targetPortal.position;

    //    #region 나중에 지워야할 것
    //    player.GetComponent<move>().currentMap = targetPortal.MapIndex;
    //    Camera.main.GetComponent<followcam>().transCam(targetPortal.MapIndex);
    //    #endregion

    //    //yield return SceneController.Instance.FadeOut(0.5f); // 페이드 아웃

    //}

    /// <summary>
    /// 다른 씬으로 이동하고, 해당 씬 내 포탈 인덱스 위치로 이동
    /// </summary>
    public void TeleportScene(string sceneName, int spawnPortalIndex = 0)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[StageManager] 씬 '{sceneName}' 을(를) 로드할 수 없습니다. Build Settings에 등록되어 있는지 확인하세요.");
            return;
        }

        // 씬 전환 전 페이드 아웃 추가
        StartCoroutine(FadeOutThenLoad(sceneName, spawnPortalIndex));
    }

    private IEnumerator FadeOutThenLoad(string sceneName, int spawnPortalIndex)
    {
        //yield return SceneController.Instance.FadeIn(0.5f); //페이드 인 먼저 실행

        // 이제 기존 로딩 코루틴 실행
        yield return LoadSceneAndTeleport(sceneName, spawnPortalIndex);
    }


    /// <summary>
    /// 씬 비동기 로딩 + 도착 포탈 위치로 이동 처리
    /// </summary>
    private IEnumerator LoadSceneAndTeleport(string sceneName, int spawnPortalIndex)
    {
        portalDict.Clear(); // 이전 씬 포탈 데이터 초기화

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // 씬 전환 후 포탈 등록 대기 (0.3초대기)
        yield return new WaitForSeconds(0.3f);

        player = GameObject.FindGameObjectWithTag("Player"); // 새 씬에서 플레이어 재탐색

        if (!portalDict.TryGetValue(spawnPortalIndex, out Portal spawnPortal))
        {
            Debug.LogWarning($"[StageManager] 도착 포탈 인덱스 {spawnPortalIndex}를 찾을 수 없습니다.");
            yield break;
        }



        if (Player != null)
        {
            Player.transform.position = spawnPortal.targetPortal.position;

            #region 나중에 지워야할 것
            player.GetComponent<move>().currentMap = spawnPortal.MapIndex; // 이동할 맵의 인덱스로
            Camera.main.GetComponent<followcam>().transCam(spawnPortal.MapIndex); // 카메라 이동
            #endregion

            //페이드 아웃
            //yield return SceneController.Instance.FadeOut(0.5f);  // 페이드 아웃
        }

        
    }
}
