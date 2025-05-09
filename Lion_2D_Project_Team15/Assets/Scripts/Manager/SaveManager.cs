using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveManager : Singleton<SaveManager>
{
    string _savePath = "Save";

    public async UniTask SaveAsync()
    {
        await UniTask.Yield();
    }

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, _savePath, "TestSave.json");
        Debug.LogError("Save Path " + path);
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

            identifier.SaveData.Reset(identifier);
            var entry = identifier.SaveData;
            gameObjects.Add(objectID, entry);
        }

        string json = JsonConvert.SerializeObject(
            save,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }
        );


        File.WriteAllText(path, json);
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, _savePath, "TestSave.json");

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

        // StageManager 같은 Singleton 계열 Load도 필요할듯
    }

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
        public List<GameObjectProperty> Properties;
        public GameObjectEntry(in IdentifiableMonoBehavior identifiableMonoBehavior)
        {
            Position = identifiableMonoBehavior.transform.position;
            IsActive = identifiableMonoBehavior.gameObject.activeSelf;
            Properties = new();
        }

        public void Reset(in IdentifiableMonoBehavior identifier)
        {
            Position = identifier.transform.position;
            IsActive = identifier.gameObject.activeSelf;
        }

        public void Add<T>(in T value)
        {
            Properties.Add(new GameObjectProperty()
            {
                Type = typeof(T).FullName,
                Key = nameof(value),
                Value = value
            });
        }
    }

    [Serializable]
    public struct GameObjectProperty
    {
        public string Type;
        public string Key;
        public object Value;
    }
}
