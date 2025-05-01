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
    DialogueManager _dialogueManager;

    void Start()
    {
        _dialogueManager = DialogueManager.Instance;
    }

    public void OnClickTouchPanel()
    {
        if (!IsPrintComplete)
        {
            DoSkip = true;
            return;
        }

        _dialogueManager.MoveNext();
    }

    public void ShowDialogue(DialogueLineData lineData)
    {
        StartCoroutine(PlayScript(lineData));
    }

    IEnumerator PlayScript(DialogueLineData lineData)
    {
        // 초기화
        IsPrintComplete = false;
        DoSkip = false;

        string[] split = _currentDialogueData.id.Split("_");

        scriptIdPrefix = split[0];
        scriptIdNumber = split[1];

        _speaker.SetText(scriptIdPrefix);    // 화자의 이름을 어떻게 정할지
        _content.SetText("");

        GameManager gameManager = GameManager.Instance;
        string script = lineData.text;

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

    public void ChooseOption(string dialogueID)
    {
        _dialogueOptionPanel.ClearOptions();
        _dialogueOptionPanel.gameObject.SetActive(false);
        _dialogueManager.JumpTo(dialogueID);
    }
    
    // IEnumerator PlayScript(ITextEffect textEffect);
}
