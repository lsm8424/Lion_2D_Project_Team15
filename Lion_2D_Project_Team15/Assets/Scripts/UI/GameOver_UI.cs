using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour
{
    [SerializeField]
    Image _panelImage;

    [SerializeField]
    float _panelFadeTime = 1f;

    [SerializeField]
    Button _restartButton;

    [SerializeField]
    Image _restartButtonImage;

    [SerializeField]
    TextMeshProUGUI _restartButtonText;

    [SerializeField]
    float _buttonFadeTime = 0.5f;

    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        _restartButton.interactable = false;

        Fade fade = new Fade(_panelImage, Color.clear, new Color(0, 0, 0, 0.75f), _panelFadeTime);
        yield return fade.Execute();

        float percent = 0;
        float elapsedTime = 0;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / _buttonFadeTime;

            _restartButtonImage.color = Color.Lerp(Color.clear, Color.white, percent);
            _restartButtonText.color = Color.Lerp(Color.clear, Color.white, percent);
            yield return null;
        }

        _restartButtonImage.color = Color.white;
        _restartButtonText.color = Color.white;

        _restartButton.interactable = true;
    }

    public void OnPressRestart()
    {
        GameManager.Instance.LoadGame();
    }
}
