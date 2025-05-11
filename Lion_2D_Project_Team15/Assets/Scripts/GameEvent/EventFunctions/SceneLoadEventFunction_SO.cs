using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SceneLoadEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/SceneLoadEventFunction_SO"
)]
public class SceneLoadEventFunction_SO : EventFunction_SO
{
    [Header("씬 전환 설정")]
    public string sceneName;
    public float fadeDuration = 1f;

    public override void Setup() { }

    public override IEnumerator Execute()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneLoadEventFunction] 씬 이름이 설정되지 않았습니다.");
            yield break;
        }

        SceneController.Instance.LoadSceneWithFadeInOut(sceneName, fadeDuration);
        yield return null; // 로딩 자체는 SceneController에서 처리되므로 즉시 리턴
    }
}
