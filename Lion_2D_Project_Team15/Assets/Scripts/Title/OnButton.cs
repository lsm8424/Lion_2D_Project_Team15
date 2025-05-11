using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnButton : MonoBehaviour
{
    public Image ButtonImage; // TextMeshProUGUI 컴포넌트

    public void OnClick()
    {
        //텍스트 색깔 변경
        ButtonImage.color = new Color(0.7f, 0.7f, 0.7f, 1); // 투명도 변경
    }

    public void OutClick()
    {
        //텍스트 색깔 변경
        ButtonImage.color = new Color(1, 1, 1, 1); // 투명도 변경
    }

    public void StartButtonClick()
    {
        Debug.Log("버튼 클릭됨(씬이동)");

        //캔버스 비활성화
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.sortingOrder = -2;

        SceneController.Instance.LoadSceneWithFadeInOut("Prologue2", 1f);
    }

    public void SettingButtonClick()
    {
        UIManager.Instance.OpenPanel(UIManager.Instance.VolumePanel);
    }

    public void ExitButtonClick()
    {
        Debug.Log("게임 종료");
        //Application.Quit();
    }   

}
