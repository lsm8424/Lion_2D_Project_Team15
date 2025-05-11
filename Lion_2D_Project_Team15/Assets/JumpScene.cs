using UnityEngine;

public class JumpScene : MonoBehaviour
{
    public string SceneName;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            SceneController.Instance.LoadSceneWithFadeInOut(SceneName, 0.1f);
    }
}
