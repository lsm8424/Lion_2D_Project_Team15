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
            Debug.LogError($"[IDManager] ��ȿ�� ObjectId�� ã�� �� �����ϴ�. {key}");
            return null;
        }

        return Identifiers[key];
    }

    public void Set(string key, IdentifiableMonoBehavior obj)
    {
        if (Identifiers.ContainsKey(obj.ObjectId))
            Debug.LogError($"[IDManager] ObjectId�� �����մϴ�. {obj.ObjectId}");

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
