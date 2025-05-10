using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveManager : Singleton<SaveManager>
{
    string _savePath = "Save";
    public string GetPath() => Path.Combine(Application.persistentDataPath, _savePath, "Save.json");
    public bool HasSave => new FileInfo(GetPath()).Exists;
    void Start()
    {
    }
    public async UniTask SaveAsync()
    {
        await UniTask.WaitUntil(() => Player.Instance.didStart);
        await UniTask.Yield();
        Save();
    }

    public async UniTask LoadAsync()
    {
        await UniTask.WaitUntil(() => Player.Instance.didStart);
        await UniTask.Yield();
        Load();
    }

    public void Save()
    {
        string path = GetPath();
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        var save = new SaveData();
        save.SceneName = SceneManager.GetActiveScene().name;
        var quests = save.Quest;

        foreach (var (questID, progress) in QuestManager.Instance.Progresses)
        {
            var entry = new QuestEntry()
            {
                QuestID = questID,
                ProgressLevel = progress,
            };
            quests.Add(entry);
        }

        var identifiers = IDManager.Instance.Identifiers;
        var gameObjects = save.GameObjects;

        foreach (var (objectID, identifier) in identifiers)
        {
            if (string.IsNullOrWhiteSpace(objectID))
                continue;

            if (identifier == null)
                continue;

            var entry = identifier.GetSaveData();
            gameObjects.Add(objectID, entry);
        }

        string json = JsonConvert.SerializeObject(
            save,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,   // Vector3의 순환참조 문제
            }
        );

        File.WriteAllText(path, json);
        Debug.LogError("Save is complete. Path: " + path);
    }

    public void Load()
    {
        string path = GetPath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("세이브 파일이 존재하지 않습니다: " + path);
            return;
        }

        string json = File.ReadAllText(path);

        SaveData save = JsonConvert.DeserializeObject<SaveData>(
            json,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }
        );

        var quests = QuestManager.Instance.Quests;
        var questProgresses = QuestManager.Instance.Progresses;
        foreach (var quest in save.Quest)
        {
            if (quests.TryGetValue(quest.QuestID, out var questInfo))
            {
                questProgresses[quest.QuestID] = quest.ProgressLevel;
                questInfo.SetTrigger(quest.ProgressLevel);
            }
        }

        var identifiers = IDManager.Instance.Identifiers;
        var objIDs = IDManager.Instance.Identifiers.Keys.ToArray();

        foreach (var objectID in objIDs)
        {
            var targetObj = identifiers[objectID];
            if (save.GameObjects.TryGetValue(objectID, out var entry))
            {
                targetObj.Load(entry);
            }
            else
            {
                identifiers.Remove(objectID);
                Destroy(targetObj);
            }
        }

        Debug.Log("Load is complete.");
        // StageManager 같은 Singleton 계열 Load도 필요할듯
    }

    [Serializable]
    public class SaveData
    {
        public string SceneName;
        public List<QuestEntry> Quest = new();
        public Dictionary<string, GameObjectEntry> GameObjects = new();
    }

    [Serializable]
    public struct QuestEntry
    {
        public string QuestID;
        public int ProgressLevel;
    }

    [Serializable]
    public struct GameObjectEntry
    {
        public Vector3 Position;
        public bool IsActive;
        public Dictionary<string, GameObjectProperty> Properties;
        public GameObjectEntry(in IdentifiableMonoBehavior identifiableMonoBehavior)
        {
            Position = identifiableMonoBehavior.transform.position;
            IsActive = identifiableMonoBehavior.gameObject.activeSelf;
            Properties = new Dictionary<string, GameObjectProperty>();
        }

        public void Reset(in IdentifiableMonoBehavior identifier)
        {
            Position = identifier.transform.position;
            IsActive = identifier.gameObject.activeSelf;
        }

        public void Add<T>(string key, in T value)
        {
            if (typeof(T) == typeof(object))
            {
                Debug.LogError("Object 타입은 들어갈 수 없습니다.");
                return;
            }

            if (!typeof(T).IsSerializable)
            {
                Debug.LogError("직렬화가 안되는 타입은 넣을 수 없습니다.");
                return;
            }

            Properties.Add(key, new GameObjectProperty()
            {
                Type = typeof(T).FullName,
                Value = value
            });
        }
        public void Add(string key, string type, in object value)
        {
            Properties.Add(key, new GameObjectProperty()
            {
                Type = type,
                Value = value
            });
        }
        public readonly T Get<T>(string key)
        {
            if (!Properties.ContainsKey(key))
                return default;

            GameObjectProperty property = Properties[key];

            if (property.Value is T castValue)
            {
                return (T)property.Value;
            }

            return default;
        }

        public readonly bool TryGet<T>(string key, out T value)
        {
            value = default;

            if (!Properties.ContainsKey(key))
                return false;

            GameObjectProperty property = Properties[key];

            if (property.Value is T castValue)
            {
                value = castValue;
                return true;
            }

            return false;
        }

        public readonly void GetAll(out string[] keys, out string[] types, out object[] values)
        {
            keys = new string[Properties.Count];
            types = new string[Properties.Count];
            values = new object[Properties.Count];

            int index = 0;
            foreach (var property in Properties)
            {
                keys[index] = property.Key;
                types[index] = property.Value.Type;
                values[index] = property.Value.Value;
                index++;
            }
        }
    }

    [Serializable]
    public struct GameObjectProperty
    {
        public string Type;
        public object Value;
    }
}
