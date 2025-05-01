using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : Singleton<DialogueManager>
{
    public Dialogue_UI Dialogue_UI;
    List<DialogueLineData> _dialogueLines = new();
    int _dialogueIndex;
    
    public void StartDialogue(DialogueCategory category, string dialogueID)
    {
        LoadDialogueLines(dialogueID);
        Dialogue_UI.gameObject.SetActive(true);
        // Dialogue_UI.ShowDialogue();
    }

    public bool MoveNext()
    {
        var lineData = GetNextLine();

        if (lineData == null)
            return false;

        Dialogue_UI.ShowDialogue(lineData);
        return true;
    }

    DialogueLineData GetNextLine()
    {
        ++_dialogueIndex;
        if (_dialogueIndex >= _dialogueLines.Count)
        {
            CloseDialogueUI();
            return null;
        }

        return _dialogueLines[_dialogueIndex];
    }

    public DialogueLineData JumpTo(string dialogueID)
    {
        _dialogueIndex = _dialogueLines.FindIndex(line => line.id == dialogueID);
        if (_dialogueIndex == -1)
        {
            Debug.LogError($"유효한 DialogueID가 아닙니다 ${dialogueID}");
            return null;
        }

        return _dialogueLines[_dialogueIndex];
    }

    public void CloseDialogueUI()
    {
        Dialogue_UI.gameObject.SetActive(false);
        //Player.Instance.interaction.EndDialogue();
        // NPC의 대화종료 이벤트가 필요하다면 코드 수정 필요
        _dialogueLines = null;
    }

    void LoadDialogueLines(string dialogueID)   // 데이터 정보에 따라 수정 필요
    {
        List<DialogueLineData> lines = new();
        DialogueLineData line;

        do
        {
            line = DialogueDatabase_JSON.Instance.GetDialogue(DialogueCategory.Dialogue, dialogueID);
            lines.Add(line);
        } while (line == null);

        _dialogueLines = lines;
        _dialogueIndex = 0;
    }
}
