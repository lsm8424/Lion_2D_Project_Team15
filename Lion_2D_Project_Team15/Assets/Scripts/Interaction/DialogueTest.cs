using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 대화 데이터 가져오기
            var questDialogue = DialogueDatabase_JSON
                .Instance
                .GetDialogue(DialogueCategory.Quest, "quest_001");
            var normalDialogue = DialogueDatabase_JSON
                .Instance
                .GetDialogue(DialogueCategory.Dialogue, "story_001");

            // 대화 내용 출력
            Debug.Log("퀘스트 대화: " + questDialogue.text);
            Debug.Log("일반 대화: " + normalDialogue.text);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            // 대화 데이터 가져오기
            var interactionDialogue = DialogueDatabase_JSON
                .Instance
                .GetDialogue(DialogueCategory.Interaction, "npc_001");

            // 대화 내용 출력
            Debug.Log("상호작용 대화: " + interactionDialogue.text);
        }
    }
}
