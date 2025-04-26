/*
개요:
게임의 대화 시스템을 관리하는 핵심 클래스
JSON 형식으로 저장된 데이터를 로드하고 관리하며, 게임 내에서 필요한 대화를 제공

흐름:
1. JSON형식으로 저장된 데이터를 읽어 딕셔너리 형태의 변수에 저장
2. 외부에서 필요한 대사만을 GetDialogue()함수를 통해 리턴해줌

주요 기능:
1. JSON 파일에서 대화 데이터 로드 및 관리 (Quest/Dialogue/Interaction 카테고리별)
2. 대화 데이터의 조건부 실행 지원 (특정 조건을 만족할 때만 대화 표시)
3. 다중 선택지가 포함된 대화 지원
4. 씬 전환 시에도 데이터 유지 (DontDestroyOnLoad)

사용 예시:
하이어라키 창에 해당 스크립트를 가진 오브젝트가 존재하여야 함(DialogueManager 등)

// 퀘스트 대화 가져오기
DialogueLineData questDialogue = DialogueDatabase_JSON.Instance.GetDialogue(DialogueCategory.Quest, "quest_001");

// 일반 대화 가져오기
DialogueLineData normalDialogue = DialogueDatabase_JSON.Instance.GetDialogue(DialogueCategory.Dialogue, "npc_001");

파일 구조:
대화 데이터는 Resources/dialogues/ 폴더 아래에 다음과 같이 저장됨
- dialogues/quest.json      : 퀘스트 관련 대화
- dialogues/dialogue.json   : 일반 대화
- dialogues/interaction.json: 상호작용 대화

주의사항:
1. JSON 파일은 반드시 Resources 폴더 내에 위치해야 함
2. 대화 ID는 각 카테고리 내에서 고유해야 함
3. 조건부 대화 사용 시 관련 조건 로직 구현 필요
*/

using System;
using System.Collections.Generic;
using UnityEngine;

// 대사를 3종의 카테고리로 나눔
public enum DialogueCategory
{
    Quest,
    Dialogue,
    Interaction
}

public class DialogueDatabase_JSON : MonoBehaviour
{
    //대사를 필요한 곳에서 호출 할 수 있도록 싱글톤 생성
    public static DialogueDatabase_JSON Instance;

    // 대사 데이터를 저장해둘 변수(allDialogues)
    // 각 카테고리 별 딕서녀리 속 대사 모음 딕셔너리가 존재하는 2중 딕서녀리 구조
    private Dictionary<DialogueCategory, Dictionary<string, DialogueLineData>> allDialogues =
        new Dictionary<DialogueCategory, Dictionary<string, DialogueLineData>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCategory(DialogueCategory.Quest, "dialogues/quest");
            LoadCategory(DialogueCategory.Dialogue, "dialogues/dialogue");
            LoadCategory(DialogueCategory.Interaction, "dialogues/interaction");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //JSON 파일을 로드하여 대사 데이터를 초기화하는 함수
    void LoadCategory(DialogueCategory category, string resourcePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {resourcePath}");
            return;
        }

        DialogueEntry[] entries = JsonHelper.FromJson<DialogueEntry>(jsonFile.text);
        var dict = new Dictionary<string, DialogueLineData>();

        foreach (var e in entries)
        {
            // 조건 함수: 항상 true 반환
            Func<bool> cond = AlwaysTrueCondition;

            // 선택지 파싱
            DialogueOption[] opts = new DialogueOption[0];
            if (!string.IsNullOrEmpty(e.choices))
            {
                opts = ParseChoices(e.choices);
            }

            dict[e.id] = new DialogueLineData
            {
                id = e.id,
                text = e.text,
                options = opts,
                conditionCheck = cond
            };
        }

        allDialogues[category] = dict;
    }

    // 일반적인 조건 함수 표현
    private bool AlwaysTrueCondition()
    {
        return true;
    }

    // 선택지 파싱 함수
    private DialogueOption[] ParseChoices(string choicesRaw)
    {
        var choiceList = new List<DialogueOption>();
        string[] splitChoices = choicesRaw.Split('|');

        foreach (var ch in splitChoices)
        {
            string[] parts = ch.Split(':');
            if (parts.Length == 2)
            {
                DialogueOption option = new DialogueOption { text = parts[0], nextId = parts[1] };
                choiceList.Add(option);
            }
            else
            {
                Debug.LogWarning($"잘못된 선택지 형식: {ch}");
            }
        }

        return choiceList.ToArray();
    }

    // 딕셔너리 속 대사를 반환 해주는 함수(카테고리, 대사 ID를 인자로 받음)
    public DialogueLineData GetDialogue(DialogueCategory category, string id)
    {
        if (
            allDialogues.TryGetValue(category, out var dict)
            && dict.TryGetValue(id, out var line)
            && line.conditionCheck()
        )
        {
            return line;
        }
        return null;
    }
}
