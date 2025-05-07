using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCtrl : MonoBehaviour
{
    public CinemachinePositionComposer cinemaPosition;
    public CinemachineCamera cinemaCamera;

    // [SerializeField]
    // private float duration;
    // private float time = 0f;

    // [Header("기본 값")]
    // [SerializeField] private Vector2 defaultScreenPosition = new Vector2(0,0.2f);
    // [SerializeField] private Vector3 defaultOffset = new Vector3(0,0,0);
    // [SerializeField] private float defaultZoom = 6;
    [Header("초기 값")]
    [SerializeField]
    private Vector2 initPosition;

    [SerializeField]
    private Vector3 initOffset;

    [SerializeField]
    private float initZoom;

    private void Start()
    {
        SetCinemaChine();
        //Invoke(nameof(TimeSet), 1f); // 1초 후에 TimeSet() 메서드 호출
    }

    // private void Update()
    // {
    //     if(time > 0)
    //     {
    //         time -= Time.deltaTime;

    //         CamaraZoom();

    //         if (time <= 0)
    //         {
    //             return;
    //         }
    //     }
    // }

    // private void CamaraZoom()
    // {
    //     // 1초당 이동할 변화량
    //     Vector2 screenPos = (defaultScreenPosition - initPosition) / duration;
    //     Vector3 offset = (defaultOffset - initOffset) / duration;
    //     float zoom = (defaultZoom - initZoom) / duration;

    //     cinemaCamera.Lens.OrthographicSize += zoom * Time.deltaTime;
    //     cinemaPosition.Composition.ScreenPosition += screenPos * Time.deltaTime;
    //     cinemaPosition.TargetOffset += offset * Time.deltaTime;

    // }

    private void SetCinemaChine()
    {
        // 카메라의 기본값을 설정합니다.
        cinemaPosition.Composition.ScreenPosition = initPosition;
        cinemaPosition.TargetOffset = initOffset;
        cinemaCamera.Lens.OrthographicSize = initZoom;
    }

    //void TimeSet() => time = duration;
}
