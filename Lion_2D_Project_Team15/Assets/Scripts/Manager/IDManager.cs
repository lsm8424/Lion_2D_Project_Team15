using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IDManager : Singleton<IDManager>
{
    public Dictionary<string, IdentifiableMonoBehavior> Identifiers { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();
        SetUpIdentifiers();
    }

    public IdentifiableMonoBehavior Get(string key)
    {
        if (!Identifiers.ContainsKey(key))
        {
            Debug.LogError($"[IDManager] 유효한 ObjectId를 찾을 수 없습니다. {key}");
            return null;
        }

        return Identifiers[key];
    }

    public void Set(string key, IdentifiableMonoBehavior obj)
    {
        if (Identifiers.ContainsKey(obj.ObjectId))
            Debug.LogError($"[IDManager] ObjectId가 동일합니다. {obj.ObjectId}");

        Identifiers.Add(obj.ObjectId, obj);
    }

    public void SetUpIdentifiers()
    {
        Identifiers.Clear();

        IdentifiableMonoBehavior[] foundObjects = FindObjectsByType<IdentifiableMonoBehavior>(FindObjectsSortMode.None);

        foreach (var obj in foundObjects)
            Set(obj.ObjectId, obj);
    }
}
