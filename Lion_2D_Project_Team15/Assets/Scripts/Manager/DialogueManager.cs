using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    public Dialogue_UI Dialogue_UI;
    List<DialogueLineData> _dialogueLines = new();
    int _dialogueIndex;

    public void StartDialogue(DialogueCategory category, string dialogueID)
    {
        LoadDialogueLines(category, dialogueID);
        Dialogue_UI.gameObject.SetActive(true);
        Dialogue_UI.ShowDialogue(_dialogueLines[_dialogueIndex]);
    }

    public void ProcessPlayerInput()
    {
        if (!Dialogue_UI.IsPrintComplete)
        {
            Dialogue_UI.DoSkip = true;
            Debug.Log("DOSKIP");
            return;
        }

        Debug.Log("MOVENEXT");
        MoveNext();
    }

    public bool MoveNext()
    {
        var lineData = GetNextLine();

        if (lineData == null)
        {
            CloseDialogueUI();
            return false;
        }

        Dialogue_UI.ShowDialogue(lineData);
        return true;
    }

    DialogueLineData GetNextLine()
    {
        ++_dialogueIndex;

        if (_dialogueIndex >= _dialogueLines.Count)
            return null;

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

    void LoadDialogueLines(DialogueCategory category, string dialogueID)   // 데이터 정보에 따라 수정 필요
    {
        List<DialogueLineData> lines = new();
        DialogueLineData line;
        var db = DialogueDatabase_JSON.Instance;

        do
        {
            line = db.GetDialogue(category, dialogueID);
            if (line == null)
            {
                Debug.LogError($"로드된 Dialogue의 내용이 존재하지 않습니다. {dialogueID}");
                break;
            }

            lines.Add(line);

            if (!TryGetNextID(dialogueID, out string nextID))
                break;
            dialogueID = nextID;

        } while (!line.isEndOfDialogue);

        _dialogueLines = lines;
        _dialogueIndex = 0;
    }

    bool TryGetNextID(string dialogueID, out string nextID)
    {
        nextID = null;
        string[] split = dialogueID.Split("_");

        string prefix = split[0];
        string number = split[1];
        
        if (!int.TryParse(number, out int num))
        {
            Debug.LogError($"숫자 변환 오류 {number}");
            return false;
        }
        ++num;
        nextID = prefix + "_" + num.ToString("D3");

        return true;
    }
}
