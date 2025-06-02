using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quest_UI : MonoBehaviour
{
    [SerializeField]
    private GameObject questUIObject;

    [SerializeField]
    private TMP_Text tmp;

    public enum QuestType
    {
        Normal,
        Collection
    }

    private class QuestData
    {
        public string QuestID { get; set; }
        public string Objective { get; set; }
        public QuestType Type { get; set; }
        public int CurrentProgress { get; set; }
        public int MaxProgress { get; set; }

        public QuestData(
            string questID,
            string objective,
            QuestType type,
            int currentProgress = 0,
            int maxProgress = 0
        )
        {
            QuestID = questID;
            Objective = objective;
            Type = type;
            CurrentProgress = currentProgress;
            MaxProgress = maxProgress;
        }
    }

    private List<QuestData> questList = new List<QuestData>();

    public void AddQuest(
        string questID,
        string objective,
        QuestType type,
        int currentProgress = 0,
        int maxProgress = 0
    )
    {
        // 이미 존재하는 퀘스트인지 확인
        if (questList.Exists(q => q.QuestID == questID))
        {
            Debug.LogWarning($"[Quest_UI] 이미 존재하는 퀘스트입니다: {questID}");
            return;
        }

        // 타입에 따른 유효성 검사
        if (type == QuestType.Collection && (currentProgress < 0 || maxProgress <= 0))
        {
            Debug.LogError($"[Quest_UI] 수집 퀘스트의 진행도가 올바르지 않습니다: {questID}");
            return;
        }

        questList.Add(new QuestData(questID, objective, type, currentProgress, maxProgress));
        UpdateQuestUIVisibility();
        UpdateUI();
    }

    public void RemoveQuest(string questID)
    {
        questList.RemoveAll(q => q.QuestID == questID);
        UpdateQuestUIVisibility();
        UpdateUI();
    }

    public void UpdateQuestProgress(string questID, int currentProgress)
    {
        var quest = questList.Find(q => q.QuestID == questID);
        if (quest != null && quest.Type == QuestType.Collection)
        {
            quest.CurrentProgress = currentProgress;
            UpdateUI();
        }
    }

    private void UpdateQuestUIVisibility()
    {
        if (questUIObject != null)
        {
            questUIObject.SetActive(questList.Count > 0);
        }
    }

    private void UpdateUI()
    {
        if (tmp == null)
        {
            Debug.LogError("[Quest_UI] TMP_Text component is missing!");
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var quest in questList)
        {
            if (quest.Type == QuestType.Collection)
            {
                string questText =
                    quest.CurrentProgress >= quest.MaxProgress
                        ? $"<s>{quest.Objective}({quest.CurrentProgress}/{quest.MaxProgress})</s>"
                        : $"{quest.Objective}({quest.CurrentProgress}/{quest.MaxProgress})";

                sb.AppendLine(questText);
            }
            else // Normal type
            {
                sb.AppendLine(quest.Objective);
            }
        }

        tmp.text = sb.ToString().TrimEnd();
    }

    void Start()
    {
        if (tmp == null)
            Debug.LogError("[Quest_UI] TMP_Text component is missing!");

        UpdateQuestUIVisibility();
    }

    void Update() { }
}
