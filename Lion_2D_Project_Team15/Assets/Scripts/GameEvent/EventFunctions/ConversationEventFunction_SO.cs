using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationEventFunction_SO", menuName = "Scriptable Objects/EventFunction/ConversationEventFunction_SO")]
public class ConversationEventFunction_SO : EventFunction_SO
{
    public string DialogueID;
    public DialogueCategory Category;

    public override IEnumerator Execute()
    {
        DialogueManager.Instance.StartDialogue(Category, DialogueID);
        yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueCompleted);
    }

    public override void Setup()
    {
    }
}
