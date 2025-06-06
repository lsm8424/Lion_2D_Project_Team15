using UnityEngine;

public class JumpScene : MonoBehaviour
{
    public string SceneName;
    public string SceneName2;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            SceneController.Instance.LoadSceneWithFadeInOut(SceneName, 0.1f);

        if (Input.GetKeyDown(KeyCode.F3))
            SceneController.Instance.LoadSceneWithFadeInOut(SceneName2, 0.1f);
    }
}
