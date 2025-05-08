using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent_SO", menuName = "Scriptable Objects/GameEvent_SO")]
public class GameEvent_SO : ScriptableObject
{
    public EEventType EventType;
    public string EventID;
    public string EventName;
    public bool RequireSave;

    public EventFunction_SO[] EventFunctions;

    public void SetUp()
    {
        for (int i = 0; i < EventFunctions.Length; ++i)
            EventFunctions[i].Setup();
    }

    public IEnumerator Execute()
    {
        for (int i = 0; i < EventFunctions.Length; ++i)
        {
            GameManager.Instance.SetTimeCase(GameManager.ETimeCase.PlayingDialogue);
            yield return EventFunctions[i].Execute();
            if (GameManager.Instance.ShouldWaitForDialogue())
                yield return new WaitUntil(() => !GameManager.Instance.ShouldWaitForDialogue());
            GameManager.Instance.RevertTimeCase();
        }
    }

    public enum EEventType
    {
        Main,
        Sub,
        Trigger,
    }
}
