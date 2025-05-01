using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _speaker;
    [SerializeField] TextMeshProUGUI _content;
    [SerializeField] DialogueOptionPanel_UI _dialogueOptionPanel;

    [SerializeField] string scriptIdNumber;
    [SerializeField] string scriptIdPrefix;

    public float PrintDelay = 0.1f;
    public bool DoSkip = false;
    public bool IsPrintComplete = false;
    [SerializeField] float _timer = 0f;

    DialogueCategory _category;
    DialogueLineData _currentDialogueData;

    void Start()
    {
        // 테스트
        SetUpScripts(DialogueCategory.Interaction, "librarian_001");
    }

    public void OnClickTouchPanel()
    {
        if (!IsPrintComplete)
        {
            DoSkip = true;
            return;
        }

        MoveNext();
    }

    public void SetUpScripts(DialogueCategory category, string path)
    {
        _category = category;
        _currentDialogueData = DialogueDatabase_JSON
                .Instance
                .GetDialogue(category, path);

        if (_currentDialogueData == null)
        {
            Debug.LogError($"스크립트가 로드되지 않았습니다. DialogueCategory: {category}, path: {path}");
            return;
        }

        string[] split = _currentDialogueData.id.Split("_");

        scriptIdPrefix = split[0];
        scriptIdNumber = split[1];

        _speaker.SetText("");
        _content.SetText("");

        StartCoroutine(PlayScript());
    }

    IEnumerator PlayScript()
    {
        // 초기화
        IsPrintComplete = false;
        DoSkip = false;
        _speaker.SetText(scriptIdPrefix);    // 화자의 이름을 어떻게 정할지
        _content.SetText("");

        GameManager gameManager = GameManager.Instance;
        string script = _currentDialogueData.text;

        // 딜레이마다 한 글자씩 출력
        for (int i = 0; i < script.Length; ++i)
        {
            _content.SetText(_content.text + script[i]);

            // 공백인 경우, 딜레이 없이 출력
            if (script[i] == ' ')
                continue;

            float timer = 0;

            while (timer < PrintDelay)
            {
                // TouchPanel을 클릭하면 스킵
                if (DoSkip)
                {
                    _content.SetText(script);
                    i = script.Length;  // Outer Loop 종료조건
                    break;
                }

                timer += Time.unscaledDeltaTime * gameManager.DialogueTimeScale;
                yield return null;
            }
        }

        // 선택지가 존재한다면 출력 후 대기
        if (_currentDialogueData.options != null)
        {
            _dialogueOptionPanel.gameObject.SetActive(true);
            _dialogueOptionPanel.CreateOptions(_currentDialogueData.options);
            yield break;
        }

        IsPrintComplete = true;
    }

    bool MoveNext()
    {
        GetNextId();

        string path = scriptIdPrefix + "_" + scriptIdNumber;

        _currentDialogueData = DialogueDatabase_JSON
                .Instance
                .GetDialogue(_category, path);

        if (_currentDialogueData == null)
        {
            Debug.LogError($"Dialogue가 정상적으로 로드되지 않았습니다. Path: {path}");
            return false;
        }

        return true;
    }

    void GetNextId()
    {
        if (!int.TryParse(scriptIdNumber, out int id))
        {
            Debug.LogError($"유효하지 않은 ${scriptIdPrefix}_${scriptIdNumber}");
            return;
        }

        ++id;
        scriptIdNumber = id.ToString("D3");
    }

    public void ChooseOption(string path)
    {
        _dialogueOptionPanel.ClearOptions();
        _dialogueOptionPanel.gameObject.SetActive(false);
        SetUpScripts(_category, path);
    }
    
    // IEnumerator PlayScript(ITextEffect textEffect);
}
