using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Dictionary<string, GameEventSO> EventDictionary { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();

        InitializeEventDictionary();
    }

    public void LoadEvent(string eventId)
    {
        if (!EventDictionary.ContainsKey(eventId))
        {
            Debug.LogError($"유효한 Event ID가 아닙니다. EventID: {eventId}");
            return;
        }

        var functions = EventDictionary[eventId].EventFunctions;

        for (int i = 0; i < functions.Length; ++i)
            functions[i].Execute();
    }

    void InitializeEventDictionary()
    {
        GameEventSO[] gameEvents = Resources.LoadAll<GameEventSO>("GameEvent/.");

        for (int i = 0; i < gameEvents.Length; ++i)
        {
            string id = gameEvents[i].EventId;
            EventDictionary.Add(id, gameEvents[i]);
        }
    }
}
