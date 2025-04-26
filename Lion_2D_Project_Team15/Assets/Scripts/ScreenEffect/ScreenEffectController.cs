using System;
using System.Collections;
using UnityEngine;

public class ScreenEffectController
{
    /// <summary>
    /// 이펙트를 적용하며
    /// </summary>
    /// <param name="startEffect"></param>
    /// <param name="endEffect"></param>
    /// <param name="onStart"></param>
    /// <param name="condition">onStart, startEffect 실행 이후 대기 조건</param>
    /// <param name="onEnd"></param>
    /// <returns></returns>
    public static IEnumerator InOutEffect(IScreenEffect startEffect, IScreenEffect endEffect, Action onStart, Func<bool> condition, Action onEnd)
    {
        SceneController sceneController = SceneController.Instance;

        onStart?.Invoke();
        yield return sceneController.StartCoroutine(startEffect.Execute());

        // 조건대기
        if (condition != null)
            yield return new WaitUntil(condition);

        sceneController.StartCoroutine(endEffect.Execute());
        onEnd?.Invoke();
    }

    // 이펙트 순서에 새로운 제어가 필요하다면 메소드 작성
}
