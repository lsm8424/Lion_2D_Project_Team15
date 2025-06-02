using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestUI_SO", menuName = "Scriptable Objects/EventFunction/QuestUI_SO")]
public class QuestUI_SO : EventFunction_SO
{
    public enum FunctionType
    {
        Add, // 퀘스트 추가
        Update, // 진행도 갱신
        Remove // 퀘스트 삭제
    }

    [SerializeField]
    private FunctionType functionType;

    [SerializeField]
    private string questID;

    [SerializeField]
    private string objective;

    [SerializeField]
    private Quest_UI.QuestType questType;

    [SerializeField]
    private int currentProgress;

    [SerializeField]
    private int maxProgress;

    public override IEnumerator Execute()
    {
        var questUI = Object.FindFirstObjectByType<Quest_UI>();
        if (questUI == null)
        {
            Debug.LogError("[QuestUI_SO] Quest_UI를 찾을 수 없습니다.");
            yield break;
        }

        // questID 체크
        if (string.IsNullOrEmpty(questID))
        {
            Debug.LogError("[QuestUI_SO] QuestID가 설정되지 않았습니다.");
            yield break;
        }

        switch (functionType)
        {
            case FunctionType.Add:
                // objective 체크
                if (string.IsNullOrEmpty(objective))
                {
                    Debug.LogError("[QuestUI_SO] Objective가 설정되지 않았습니다.");
                    yield break;
                }

                // Collection 타입인 경우 진행도 유효성 검사
                if (questType == Quest_UI.QuestType.Collection)
                {
                    if (currentProgress < 0 || maxProgress <= 0)
                    {
                        Debug.LogError(
                            $"[QuestUI_SO] 수집 퀘스트의 진행도가 올바르지 않습니다. Current: {currentProgress}, Max: {maxProgress}"
                        );
                        yield break;
                    }
                }

                questUI.AddQuest(questID, objective, questType, currentProgress, maxProgress);
                break;

            case FunctionType.Update:
                if (currentProgress < 0)
                {
                    Debug.LogError("[QuestUI_SO] 진행도는 0보다 작을 수 없습니다.");
                    yield break;
                }
                questUI.UpdateQuestProgress(questID, currentProgress);
                break;

            case FunctionType.Remove:
                questUI.RemoveQuest(questID);
                break;
        }

        yield break;
    }

    public override void Setup() { }
}
