using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : IScreenEffect
{
    Image _image;
    Color _startColor;
    Color _endColor;
    float _duration;

    public bool IsCompleted { get; private set; }

    public Fade(Image image, Color startColor, Color endColor, float duration)
    {
        _image = image;
        _startColor = startColor;
        _endColor = endColor;
        _duration = duration;
        IsCompleted = false;
    }

    public IEnumerator Execute(Action onComplete = null)
    {
        IsCompleted = false;
        float percent = 0;
        float elapsedTime = 0;

        Color currentColor = _image.color;
        currentColor = _startColor;
        _image.color = currentColor;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / _duration;
            currentColor = Color.Lerp(_startColor, _endColor, percent);
            _image.color = currentColor;
            yield return null;
        }

        currentColor = _endColor;
        _image.color = currentColor;

        onComplete?.Invoke();
        IsCompleted = true;
    }
}
