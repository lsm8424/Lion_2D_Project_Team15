using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] protected Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void OnValueChanged(float percent) => _slider.value = percent;

    public void OnValueChanged(float value, float max) => _slider.value = value / max;
}
