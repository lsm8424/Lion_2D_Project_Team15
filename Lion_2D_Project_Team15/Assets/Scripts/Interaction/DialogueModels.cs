/*
DialogueModels.cs

개요:
대화 시스템에서 사용되는 데이터 모델 클래스들을 정의
JSON 파일과 매핑되는 구조체와 실제 게임에서 사용되는 데이터 구조를 포함

클래스 설명:
1. DialogueEntry: JSON 파일과 직접 매핑되는 직렬화 가능한 클래스
2. DialogueOption: 대화 선택지를 나타내는 클래스
3. DialogueLineData: 게임 내에서 실제로 사용되는 대화 데이터 클래스
*/

using System;

[Serializable]
public class DialogueEntry
{
    public string id; // 대화의 고유 식별자
    public string text; // 대화 내용
    public string choices; // 선택지 문자열 (형식: "선택1:next1|선택2:next2")
    public string condition; // 대화 표시 조건 (예: "player.level >= 10")
}

public class DialogueOption
{
    public string text; // 선택지 텍스트
    public string nextId; // 선택 시 이동할 다음 대화의 ID
}

public class DialogueLineData
{
    public string id; // 대화의 고유 식별자
    public string text; // 대화 내용
    public DialogueOption[] options; // 선택지 배열
    public Func<bool> conditionCheck; // 대화 표시 조건을 검사하는 함수
}
