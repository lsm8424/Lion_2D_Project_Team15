using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : IScreenEffect
{
    Image _image;
    Color _startColor;
    Color _endColor;

    public bool IsCompleted { get; private set; }
    public float Duration { get; set; }

    public Fade(Color startColor, Color endColor, float duration)
    {
        _image = SceneController.Instance.FadePanel;
        _startColor = startColor;
        _endColor = endColor;
        Duration = duration;
        IsCompleted = false;
    }

    public IEnumerator Execute(Action onComplete = null)
    {
        IsCompleted = false;
        float percent = 0;
        float elapsedTime = 0;

        if (_image == null)
            _image = SceneController.Instance.FadePanel;

        Color currentColor = _image.color;
        currentColor = _startColor;
        _image.color = currentColor;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / Duration;
            currentColor = Color.Lerp(_startColor, _endColor, percent);
            _image.color = currentColor;

            if (GameManager.Instance.NeedsWaitForSetting())
                yield return new WaitUntil(() => !GameManager.Instance.NeedsWaitForSetting());
            yield return null;
        }

        currentColor = _endColor;
        _image.color = currentColor;

        onComplete?.Invoke();
        IsCompleted = true;
    }
}
