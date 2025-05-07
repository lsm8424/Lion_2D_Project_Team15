using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, GameEvent_SO> Events { get; private set; } = new();
    public Dictionary<string, object> Flags { get; private set; } = new();
    public bool DidSetup = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public Coroutine RunEvent(string eventID)
    {
        if (!Events.TryGetValue(eventID, out var gameEvent))
        {
            Debug.LogError($"유효한 Event ID가 아닙니다. EventID: {eventID}");
            return null;
        }

        return StartCoroutine(gameEvent.Execute());
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

    public void SetupEvents(string path)
    {
        DidSetup = true;
        Events.Clear();

        GameEvent_SO[] gameEvents = Resources.LoadAll<GameEvent_SO>($"GameEvent/{path}");

        for (int i = 0; i < gameEvents.Length; ++i)
        {
            string id = gameEvents[i].EventID;
            Events.Add(id, gameEvents[i]);

            gameEvents[i].SetUp();
        }

        DidSetup = true;
    }
}
