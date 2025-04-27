using System;
using System.Collections;
using UnityEngine;

public class ScreenEffectController
{
    /// <summary>
    /// ����Ʈ�� �����ϸ�
    /// </summary>
    /// <param name="startEffect"></param>
    /// <param name="endEffect"></param>
    /// <param name="onStart"></param>
    /// <param name="condition">onStart, startEffect ���� ���� ��� ����</param>
    /// <param name="onEnd"></param>
    /// <returns></returns>
    public static IEnumerator InOutEffect(IScreenEffect startEffect, IScreenEffect endEffect, Action onStart, Func<bool> condition, Action onEnd)
    {
        SceneController sceneController = SceneController.Instance;

        onStart?.Invoke();
        yield return sceneController.StartCoroutine(startEffect.Execute());

        // ���Ǵ��
        if (condition != null)
            yield return new WaitUntil(condition);

        sceneController.StartCoroutine(endEffect.Execute());
        onEnd?.Invoke();
    }

    // ����Ʈ ������ ���ο� ��� �ʿ��ϴٸ� �޼ҵ� �ۼ�
}
