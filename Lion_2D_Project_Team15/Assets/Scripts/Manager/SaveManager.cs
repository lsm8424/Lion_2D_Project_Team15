using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;

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

    public void Load(int level)
    {
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
    }

    [Serializable]
    public struct GameObjectProperty
    {
        public string Type;
        public string Key;
        public object Value;
    }
}
