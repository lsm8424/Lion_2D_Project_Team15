using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButton : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI; // TextMeshProUGUI 컴포넌트

    public void OnClick()
    {
        //텍스트 색깔 변경
        textMeshProUGUI.color = new Color(1, 1, 1, 0.5f); // 투명도 변경
    }

    public void OutClick()
    {
        //텍스트 색깔 변경
        textMeshProUGUI.color = new Color(1, 1, 1, 1); // 투명도 변경
    }

    public void StartButtonClick()
    {
        Debug.Log("버튼 클릭됨(씬이동)");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // 다음 씬으로 이동
    }

    public void SettingButtonClick()
    {
        UIManager.Instance.ToggleSettings();
    }

    public void ExitButtonClick()
    {
        Debug.Log("게임 종료");
        //Application.Quit();
    }   

}
