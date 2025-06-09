using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnButton : MonoBehaviour
{
    public Image ButtonImage; // TextMeshProUGUI 컴포넌트

    public void OnClick()
    {
        //텍스트 색깔 변경
        ButtonImage.color = new Color(0.7f,0.7f,0, 1); // 투명도 변경
    }

    public void OutClick()
    {
        //텍스트 색깔 변경
        ButtonImage.color = new Color(1, 1, 0, 1); // 투명도 변경
    }

    public void StartButtonClick()
    {
        Debug.Log("버튼 클릭됨(씬이동)");

        Invoke("OutClick", 0.2f); // 클릭 후 0.1초 뒤에 OutClick() 호출
        
        SceneController.Instance.LoadSceneWithFadeInOut("Prologue2", 1f);
    }

    public void SettingButtonClick()
    {
        Invoke("OutClick", 0.2f); // 클릭 후 0.1초 뒤에 OutClick() 호출

        UIManager.Instance.OpenPanel(UIManager.Instance.VolumePanel);
    }

    public void ExitButtonClick()
    {
        Invoke("OutClick", 0.2f); // 클릭 후 0.1초 뒤에 OutClick() 호출

        Debug.Log("게임 종료");
        Application.Quit();
    }
}
