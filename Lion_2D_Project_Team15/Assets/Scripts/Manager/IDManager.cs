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

    public bool TryGet(string key, out IdentifiableMonoBehavior obj)
    {
        if (!Identifiers.ContainsKey(key))
        {
            // SetUpIdentifiers() 이후의 생성될 수 있으므로, 다시 초기화
            IdentifiableMonoBehavior[] identifers = FindObjectsByType<IdentifiableMonoBehavior>(FindObjectsSortMode.None);

            foreach (var identifier in identifers)
            {
                if (identifier.ObjectID == key)
                {
                    Identifiers.Add(identifier.ObjectID, identifier);
                    obj = identifier;
                    return true;
                }
            }

            Debug.LogError($"[IDManager] 유효한 ObjectId를 찾을 수 없습니다. {key}");
            obj = null;
            return false;
        }

        obj = Identifiers[key];
        return true;
    }

    public void Set(string key, IdentifiableMonoBehavior obj)
    {
        if (Identifiers.ContainsKey(obj.ObjectID))
            Debug.LogError($"[IDManager] ObjectId가 동일합니다. {obj.ObjectID}");

        Identifiers.Add(obj.ObjectID, obj);
    }

    public void SetUpIdentifiers()
    {
        Identifiers.Clear();

        IdentifiableMonoBehavior[] foundObjects = FindObjectsByType<IdentifiableMonoBehavior>(FindObjectsSortMode.None);

        foreach (var obj in foundObjects)
            Set(obj.ObjectID, obj);
    }
}
