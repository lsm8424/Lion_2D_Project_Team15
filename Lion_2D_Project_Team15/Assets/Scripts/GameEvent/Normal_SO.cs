using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Normal_SO", menuName = "Scriptable Objects/Normal_SO")]
public class Normal_SO : ScriptableObject
{
    public string EventID;
    public NormalEventInfo EventInfo;

    // 이벤트 트리거 설정
    public void SetTrigger()
    {
        if (string.IsNullOrEmpty(EventID))
        {
            Debug.LogError("[Normal_SO] EventID가 설정되지 않았습니다.");
            return;
        }

        if (EventInfo.Trigger == null)
        {
            Debug.Log($"[Normal_SO] {EventID}에는 Trigger가 없으므로 자동 실행합니다.");
            if (EventManager.Instance != null)
                EventManager.Instance.StartCoroutine(ExecuteEvent());
            return;
        }

        EventInfo.Trigger.EventID = EventID;
        EventInfo.Trigger.SetUp();
        EventInfo.Trigger.AddEventTrigger();
    }

    // 트리거 제거 (이벤트 재실행을 위해)
    public void RemoveTrigger()
    {
        if (EventInfo.Trigger == null)
            return;

        try
        {
            EventInfo.Trigger.RemoveTrigger();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Normal_SO] 트리거 제거 중 오류 발생: {e.Message}");
        }
    }

    // 이벤트 실행
    private IEnumerator ExecuteEvent()
    {
        if (EventInfo.GameEvent == null)
        {
            Debug.LogError($"[Normal_SO] {EventID}의 GameEvent가 null입니다.");
            yield break;
        }

        IEnumerator eventEnumerator = null;
        try
        {
            eventEnumerator = EventInfo.GameEvent.Execute();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Normal_SO] 이벤트 실행 중 오류 발생: {e.Message}");
        }
        finally
        {
            RemoveTrigger(); // 이벤트 완료 후 트리거 제거
        }
        if (eventEnumerator != null)
        {
            yield return eventEnumerator;
        }
    }

    [Serializable]
    public struct NormalEventInfo
    {
        public EventTrigger_SO Trigger;
        public GameEvent_SO GameEvent;
    }
}
