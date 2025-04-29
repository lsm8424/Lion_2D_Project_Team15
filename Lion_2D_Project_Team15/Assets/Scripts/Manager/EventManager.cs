using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, GameEventSO> Events { get; private set; } = new();
    public Dictionary<string, object> Flags { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();

        SetupEvents(1);
    }

    public void RunEvent(string eventId)
    {
        if (!Events.ContainsKey(eventId))
        {
            Debug.LogError($"유효한 Event ID가 아닙니다. EventID: {eventId}");
            return;
        }

        foreach (var function in Events[eventId].EventFunctions)
            function.Execute();
    }

    // Flag의 규칙을 정해 겹치지 않도록
    public T CheckFlag<T>(string key)
    {
        if (!Flags.ContainsKey(key))
            return default;

        return (T)Flags[key];
    }
    public void SetFlag(string key, bool flagValue)
    {
        Flags[key] = flagValue;
    }

    public void SetupEvents(int stageNumber)
    {
        Events.Clear();

        string stageEventPath = "Stage " + stageNumber;
        GameEventSO[] gameEvents = Resources.LoadAll<GameEventSO>($"GameEvent/{stageEventPath}");

        for (int i = 0; i < gameEvents.Length; ++i)
        {
            string id = gameEvents[i].EventId;
            var functions = gameEvents[i].EventFunctions;

            foreach (var func in functions)
                func.Initialize();

            Events.Add(id, gameEvents[i]);
        }
    }
}
