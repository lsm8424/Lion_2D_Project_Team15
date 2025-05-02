using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CinemachineControlEventFunction_SO", menuName = "Scriptable Objects/EventFunction/CinemachineControlEventFunction_SO")]
public class CinemachineControlEventFunction_SO : EventFunction_SO
{
    // 카메라를 멤버필드 설정필요
    public float Duration;
    public EInterpolationType InterpolationType;

    // Position or Transform 둘 중 하나 결정
    Vector3 Start;
    Vector3 End;

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

    public delegate Vector3 Interpolation(Vector3 start, Vector3 end, float t);
    public Interpolation[] InterpolationFunc =
    {
        Vector3.Lerp,
        Vector3.Slerp,
        EaseInQuad,
        EaseInCubic,
        EaseOutQuad,
        EaseOutCubic,
        EaseInOutQuad,
        EaseInOutCubic,
        SmootherStep,
        EaseOutElastic,
        EaseOutBounce,
    };

    public override IEnumerator Execute()
    {
        // CameraOn
        // Camera position = Start;
        float percent = 0;
        float elapsedTime = 0;

        Interpolation interpolation = InterpolationFunc[(int)InterpolationType];

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / Duration;

            Vector3 nextPosition = interpolation(Start, End, percent);
            // Camera position = nextPosition;
            yield return null;
        }

        //Camera position = End;
        // Camera turnOff 계산안됨
    }

    public override void Setup()
    {
    }

    static Vector3 EaseInQuad(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        return Vector3.Lerp(start, end, t * t);
    }

    static Vector3 EaseInCubic(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        return Vector3.Lerp(start, end, t * t * t);
    }

    static Vector3 EaseOutQuad(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = 1 - (1 - t) * (1 - t);
        return Vector3.Lerp(start, end, t);
    }

    static Vector3 EaseOutCubic(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t -= 1;
        t = t * t * t + 1;
        return Vector3.Lerp(start, end, t);
    }

    static Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        return Vector3.Lerp(start, end, t);
    }
    static Vector3 EaseInOutCubic(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t < 0.5f
        ? 4f * t * t * t
        : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        return Vector3.Lerp(start, end, t);
    }
    static Vector3 SmootherStep(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        return Vector3.Lerp(start, end, t);
    }

    static Vector3 EaseOutElastic(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        float c4 = (2 * Mathf.PI) / 3f;
        t = t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1;
        return Vector3.Lerp(start, end, t);
    }

    static Vector3 EaseOutBounce(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        if (t < 1 / 2.75f)
            t = 7.5625f * t * t;
        else if (t < 2 / 2.75f)
        {
            t -= 1.5f / 2.75f;
            t = 7.5625f * t * t + 0.75f;
        }
        else if (t < 2.5 / 2.75)
        {
            t -= 2.25f / 2.75f;
            t = 7.5625f * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / 2.75f;
            t = 7.5625f * t * t + 0.984375f;
        }
        return Vector3.Lerp(start, end, t);
    }
}
