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

        // Wait for scene to be loaded and initialized
        while (!SceneController.Instance.IsSceneLoaded)
        {
            yield return null;
        }
    }
}
