using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreenEffectEventFunction_SO", menuName = "Scriptable Objects/EventFunction/ScreenEffectEventFunction_SO")]
public class ScreenEffectEventFunction_SO : EventFunction_SO
{
    public float Duration = 1f;
    public EScreenEffectType ScreenEffectType;


    static IScreenEffect[] screenEffects;

    public enum EScreenEffectType
    {
        FadeIn,
        FadeOut,
        Clear
    }

    public override void Setup()
    {
    }

    public override IEnumerator Execute()
    {
        screenEffects[(int)ScreenEffectType].Duration = Duration;
        yield return screenEffects[(int)ScreenEffectType].Execute();
    }

    void OnEnable()
    {
        screenEffects = new IScreenEffect[]
        {
            new Fade(Color.clear, Color.black, 0),
            new Fade(Color.black, Color.clear, 0),
            new Fade(Color.clear, Color.clear, 0),
        };
    }

    void OnDisable()
    {
        screenEffects = null;
    }
}
