using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationEventFunction_SO", menuName = "Scriptable Objects/EventFunction/ConversationEventFunction_SO")]
public class ConversationEventFunction_SO : EventFunction_SO
{
    public string[] DialogueIDs;
    public DialogueCategory Category;


    public override IEnumerator Execute()
    {
        foreach (var dialogueID in DialogueIDs)
        {
            DialogueManager.Instance.StartDialogue(Category, dialogueID);
            yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueCompleted);
        }
    }

    public override void Setup()
    {
    }
}
