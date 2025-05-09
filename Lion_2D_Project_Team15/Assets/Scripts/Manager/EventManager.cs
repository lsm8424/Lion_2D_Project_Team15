using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, GameEvent_SO> Events { get; private set; } = new();
    public Dictionary<string, Flag> EventFlags { get; private set; } = new();
    public bool DidSetup = false;

    public struct Flag
    {
        public string Type;
        public object Value;
    }


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

    //Flag의 규칙을 정해 겹치지 않도록
    public bool TryGetFlag<T>(string key, out T value)
    {
        if (!EventFlags.ContainsKey(key))
        {
            value = default;
            return false;
        }

        value = (T)EventFlags[key].Value;
        return true;
    }

    public void SetFlag<T>(string key, T flagValue)
    {
        EventFlags[key] = new Flag
        {
            Type = typeof(T).FullName,
            Value = flagValue
        };
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
