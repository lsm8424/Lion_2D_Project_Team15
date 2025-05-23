using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        // 콘솔에 메시지 표시 (테스트용)
        Debug.Log("게임 종료!");

        // 애플리케이션 종료
        Application.Quit();
    }
}
