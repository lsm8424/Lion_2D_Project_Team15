using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "DisplayObjectEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/DisplayObjectEventFunction_SO"
)]
public class DisplayObjectEventFunction_SO : EventFunction_SO
{
    [Header("대상 오브젝트")]
    public string ObjectID;

    [Header("표시할 프리팹")]
    public GameObject PrefabToDisplay;

    [Header("위치 오프셋")]
    public Vector3 Offset;

    [Header("등장/퇴장 연출")]
    public EDisplayEffect DisplayEffect = EDisplayEffect.ScaleInOut;

    public enum EDisplayEffect
    {
        None,
        ScaleInOut,
        FadeInOut
    }

    private GameObject _spawnedObject;

    public override void Setup() { }

    public override IEnumerator Execute()
    {
        EventFunctionTracker.BeginEvent();
        if (!IDManager.Instance.TryGet(ObjectID, out var targetObj))
        {
            Debug.LogError($"[DisplayObjectEvent] 유효하지 않은 ObjectID: {ObjectID}");
            yield break;
        }

        if (PrefabToDisplay == null)
        {
            Debug.LogError("[DisplayObjectEvent] 프리팹이 할당되지 않았습니다.");
            yield break;
        }

        // 생성
        _spawnedObject = GameObject.Instantiate(PrefabToDisplay);
        _spawnedObject.name = "EventDisplayObject";
        _spawnedObject.transform.position = targetObj.transform.position + Offset;

        Transform objTransform = _spawnedObject.transform;
        SpriteRenderer sr = _spawnedObject.GetComponentInChildren<SpriteRenderer>(); // 연출용

        // 등장 연출
        switch (DisplayEffect)
        {
            case EDisplayEffect.ScaleInOut:
                objTransform.localScale = Vector3.zero;
                yield return ScaleCoroutine(objTransform, Vector3.one, 0.3f);
                break;

            case EDisplayEffect.FadeInOut:
                if (sr != null)
                {
                    sr.color = new Color(1, 1, 1, 0);
                    yield return FadeCoroutine(sr, 1f, 0.3f);
                }
                break;
        }

        // 대기
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        // 퇴장 연출
        switch (DisplayEffect)
        {
            case EDisplayEffect.ScaleInOut:
                yield return ScaleCoroutine(objTransform, Vector3.zero, 0.3f);
                break;

            case EDisplayEffect.FadeInOut:
                if (sr != null)
                {
                    yield return FadeCoroutine(sr, 0f, 0.3f);
                }
                break;
        }

        Destroy(_spawnedObject);

        // 종료 대기
        yield return new WaitForSeconds(1f);
        EventFunctionTracker.EndEvent();
    }

    private IEnumerator ScaleCoroutine(Transform target, Vector3 targetScale, float duration)
    {
        float time = 0f;
        Vector3 start = target.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(start, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }

    private IEnumerator FadeCoroutine(SpriteRenderer sr, float targetAlpha, float duration)
    {
        float time = 0f;
        Color startColor = sr.color;
        float startAlpha = startColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Color final = sr.color;
        final.a = targetAlpha;
        sr.color = final;
    }
}
