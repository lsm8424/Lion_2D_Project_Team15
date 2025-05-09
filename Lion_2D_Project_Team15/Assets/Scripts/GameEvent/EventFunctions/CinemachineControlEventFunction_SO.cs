using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CinemachineControlEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/CinemachineControlEventFunction_SO"
)]
public class CinemachineControlEventFunction_SO : EventFunction_SO
{
    [Header("Control Type")]
    public ECameraControlType ControlType = ECameraControlType.Movement;

    [Header("Camera Settings")]
    public string targetCameraName;
    public bool deactivateWhenDone = true;

    [Header("Movement Settings")]
    public float Duration = 2f;
    public EInterpolationType InterpolationType;

    [Header("Target Settings")]
    public string newTargetObjectID; // 새로운 타겟의 ID

    [Header("Start Values")]
    public Vector2 StartScreenPosition;
    public Vector3 StartOffset;
    public float StartZoom = 6f;

    [Header("End Values")]
    public Vector2 EndScreenPosition;
    public Vector3 EndOffset;
    public float EndZoom = 6f;

    [Header("Delay Settings")]
    public float WaitBefore = 0f;
    public float WaitAfter = 0f;

    [Header("Auto Start")]
    public bool useCurrentAsStart = false;

    private CinemachineCamera targetCamera;
    private CinemachinePositionComposer positionComposer;

    public enum EInterpolationType
    {
        Lerp,
        Slerp,
        EaseInQuad,
        EaseInCubic,
        EaseOutQuad,
        EaseOutCubic,
        EaseInOutQuad,
        EaseInOutCubic,
        SmootherStep,
        EaseOutElastic,
        EaseOutBounce,
    }

    public enum ECameraControlType
    {
        Movement, // 카메라 움직임 제어
        Target // 카메라 타겟 변경
    }

    public override void Setup()
    {
        if (string.IsNullOrEmpty(targetCameraName))
        {
            Debug.LogError("Target camera name is not set!");
            return;
        }

        // 비활성화된 오브젝트까지 포함해서 찾는다
        var found = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go => go.name == targetCameraName);

        if (found == null)
        {
            Debug.LogError($"Cannot find camera with name: {targetCameraName} (비활성 포함)");
            return;
        }

        targetCamera = found.GetComponent<CinemachineCamera>();
        positionComposer = found.GetComponent<CinemachinePositionComposer>();

        if (targetCamera == null || positionComposer == null)
        {
            Debug.LogError($"Missing required components on {targetCameraName}");
            return;
        }

        if (useCurrentAsStart)
        {
            StartScreenPosition = positionComposer.Composition.ScreenPosition;
            StartOffset = positionComposer.TargetOffset;
            StartZoom = targetCamera.Lens.OrthographicSize;
        }

        // 씬 시작 시 Start 위치로 세팅
        positionComposer.Composition.ScreenPosition = StartScreenPosition;
        positionComposer.TargetOffset = StartOffset;
        targetCamera.Lens.OrthographicSize = StartZoom;
    }

    public override IEnumerator Execute()
    {
        if (targetCamera == null || positionComposer == null)
        {
            Debug.LogError("Camera or PositionComposer is not set!");
            yield break;
        }

        targetCamera.gameObject.SetActive(true);

        switch (ControlType)
        {
            case ECameraControlType.Movement:
                yield return ExecuteMovement();
                break;

            case ECameraControlType.Target:
                yield return ExecuteTargetChange();
                break;
        }

        if (deactivateWhenDone)
            targetCamera.gameObject.SetActive(false);
    }

    private IEnumerator ExecuteMovement()
    {
        if (WaitBefore > 0f)
            yield return new WaitForSeconds(WaitBefore);

        float elapsed = 0f;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            float easedT = ApplyEasing(t, InterpolationType);

            positionComposer.Composition.ScreenPosition = Vector2.Lerp(
                StartScreenPosition,
                EndScreenPosition,
                easedT
            );
            positionComposer.TargetOffset = Vector3.Lerp(StartOffset, EndOffset, easedT);
            targetCamera.Lens.OrthographicSize = Mathf.Lerp(StartZoom, EndZoom, easedT);

            if (GameManager.Instance.ShouldWaitForDialogue())
                yield return new WaitUntil(() => !GameManager.Instance.ShouldWaitForDialogue());

            yield return null;
        }

        // 이동 완료 후 정확한 위치 보정
        positionComposer.Composition.ScreenPosition = EndScreenPosition;
        positionComposer.TargetOffset = EndOffset;
        targetCamera.Lens.OrthographicSize = EndZoom;

        if (WaitAfter > 0f)
            yield return new WaitForSeconds(WaitAfter);
    }

    private IEnumerator ExecuteTargetChange()
    {
        if (string.IsNullOrEmpty(newTargetObjectID))
        {
            Debug.LogError("New target object ID is not set!");
            yield break;
        }

        // IDManager를 통해 새로운 타겟 오브젝트 찾기
        if (
            !IDManager.Instance.TryGet(newTargetObjectID, out IdentifiableMonoBehavior identifiable)
        )
        {
            Debug.LogError($"Cannot find target object with ID: {newTargetObjectID}");
            yield break;
        }

        // 불필요한 중복 블록 제거
        // 타겟 변경만 수행
        targetCamera.Follow = identifiable.transform;
        Debug.Log($"Camera target changed to: {newTargetObjectID}");
        yield return null;
    }

    private float ApplyEasing(float t, EInterpolationType type)
    {
        switch (type)
        {
            case EInterpolationType.Lerp:
                return t;
            case EInterpolationType.Slerp:
                return t;
            case EInterpolationType.EaseInQuad:
                return t * t;
            case EInterpolationType.EaseInCubic:
                return t * t * t;
            case EInterpolationType.EaseOutQuad:
                return 1 - (1 - t) * (1 - t);
            case EInterpolationType.EaseOutCubic:
                t -= 1;
                return t * t * t + 1;
            case EInterpolationType.EaseInOutQuad:
                return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
            case EInterpolationType.EaseInOutCubic:
                return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
            case EInterpolationType.SmootherStep:
                t = Mathf.Clamp01(t);
                return t * t * t * (t * (6f * t - 15f) + 10f);
            case EInterpolationType.EaseOutElastic:
                if (t == 0 || t == 1)
                    return t;
                float c4 = (2 * Mathf.PI) / 3f;
                return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
            case EInterpolationType.EaseOutBounce:
                if (t < 1 / 2.75f)
                    return 7.5625f * t * t;
                else if (t < 2 / 2.75f)
                {
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                }
                else if (t < 2.5 / 2.75f)
                {
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                }
                else
                {
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
                }
            default:
                return t;
        }
    }
}
