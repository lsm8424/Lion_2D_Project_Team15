using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameObjectEntry = SaveManager.GameObjectEntry;

public abstract class IdentifiableMonoBehavior : MonoBehaviour
{
    [SerializeField] private string _objectId;
    public string ObjectID { get => _objectId; }
    protected readonly EntryBinding _binding = new();

    public GameObjectEntry GetSaveData()
    {
        GameObjectEntry entry = new GameObjectEntry(this);

        string[] types, keys;
        object[] values;
        _binding.GetAll(out keys, out types, out values);

        for (int i = 0; i < types.Length; ++i)
            entry.Add(keys[i], types[i], values[i]);

        return entry;
    }

    public void Load(GameObjectEntry saveInfo)
    {
        transform.position = saveInfo.Position;
        gameObject.SetActive(saveInfo.IsActive);

        string[] keys;
        object[] values;
        saveInfo.GetAll(out keys, out _, out values);

        for (int i = 0; i < keys.Length; ++i)
            _binding.Set(keys[i], values[i]);
    }

    public class EntryBinding
    {
        readonly Dictionary<string, Property> _properties = new();

        public void Assign<T>(string key, Func<object> getter, Action<object> setter)
        {
            _properties[key] = new Property(getter, setter, typeof(T));
        }
        public void Get(string key, out string type, out object value)
        {
            type = default;
            value = default;
            if (!_properties.ContainsKey(key))
            {
                Debug.LogError("유효한 Key가 아닙니다. Key: " + key);
                return;
            }

            type = _properties[key].Type.FullName;
            value = _properties[key].Getter();
        }

        public void GetAll(out string[] keys, out string[] types, out object[] values)
        {
            keys = _properties.Keys.ToArray();
            types = new string[keys.Length];
            values = new object[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                types[i] = _properties[key].Type.FullName;
                values[i] = _properties[key].Getter();
            }
        }

        public void Set(string key, object newValue)
        {
            if (!_properties.ContainsKey(key))
            {
                Debug.LogError("유효하지 않은 Key: " + key);
                return;
            }
            _properties[key].Setter(newValue);
        }

        struct Property
        {
            public Func<object> Getter;
            public Action<object> Setter;
            public readonly Type Type;
            public Property(Func<object> getter, Action<object> setter, Type type)
            {
                Getter = getter;
                Setter = setter;
                Type = type;
            }
        }
    }

    public virtual void Bind() { }
    protected virtual void OnDestroy()
    {
        IDManager.Instance.Identifiers.Remove(ObjectID);
    }
}
