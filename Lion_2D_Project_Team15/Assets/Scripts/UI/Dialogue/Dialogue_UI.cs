using System.Collections;
using System.Collections.Generic;
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

    DialogueManager _dialogueManager;


    Dictionary<string, string> _nicknames = new Dictionary<string, string>
    {
        {"player", "플레이어" },
        {"libram", "리브람" },
    };

    void Start()
    {
        _dialogueManager = DialogueManager.Instance;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _dialogueManager.ProcessPlayerInput();
        }
    }

    public void OnClickTouchPanel()
    {
        _dialogueManager.ProcessPlayerInput();
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

        string[] split = lineData.id.Split('_');

        scriptIdPrefix = split[0];
        scriptIdNumber = split[1];


        SetNickname(scriptIdPrefix);
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
        if (lineData.options.Length > 0)
        {
            _dialogueOptionPanel.gameObject.SetActive(true);
            _dialogueOptionPanel.CreateOptions(lineData.options);
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

    void SetNickname(string speaker)
    {
        if (_nicknames.TryGetValue(scriptIdPrefix, out string nickname))
            _speaker.SetText(nickname);
        else
            _speaker.SetText(scriptIdPrefix);
    }
    
    // IEnumerator PlayScript(ITextEffect textEffect);
}
