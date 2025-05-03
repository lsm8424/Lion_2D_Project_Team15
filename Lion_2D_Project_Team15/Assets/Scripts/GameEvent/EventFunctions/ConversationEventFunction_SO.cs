using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationEventFunction_SO", menuName = "Scriptable Objects/EventFunction/ConversationEventFunction_SO")]
public class ConversationEventFunction_SO : EventFunction_SO
{
    public ConversationInfo[] Conversations;

    [Serializable]
    public struct ConversationInfo
    {
        public DialogueCategory Category;
        public string DialogueID;

        public void Deconstruct(out DialogueCategory category, out string dialgoueID)
        {
            category = Category;
            dialgoueID = DialogueID;
        }
    }

    public override IEnumerator Execute()
    {
        foreach (var (category, dialogueID) in Conversations)
        {
            DialogueManager.Instance.PlayOneShot(category, dialogueID);
            yield return new WaitUntil(() => DialogueManager.Instance.IsOneShotCompleted);
        }
    }

    public override void Setup()
    {
    }
}
