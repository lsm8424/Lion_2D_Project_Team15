using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    #region Resource
    [field:SerializeField] public Dialogue_UI Dialogue_UI { get; private set; }
    [SerializeField] GameObject _dialogueCanvas;
    #endregion

    List<DialogueLineData> _dialogueLines = new();
    int _dialogueIndex;
    public bool IsDialogueCompleted { get; private set; } = true;
    public bool IsOneShotCompleted { get; private set; } = true;

    bool _isSkip = false;
    public bool IsSkip
    {
        get => _isSkip;
        set
        {
            Dialogue_UI.SkipToggle.isOn = value;
            _isSkip = value; 
        }
    }
    float _skipTimer = 0f;
    public float SkipDelay = 0.1f;

    bool _isAuto = false;
    public bool IsAuto
    {
        get => _isAuto;
        set
        {
            Dialogue_UI.AutoToggle.isOn = value;
            _isAuto = value;
        }
    }
    float _autoPlayTimer = 0f;
    public float AutoPlayDelay = 1f;

    protected override void Awake()
    {
        // 만약 Prefab이 없다면 Resources/SceneCanvas를 Load하여 사용
        if (_dialogueCanvas == null)
            _dialogueCanvas = Resources.Load<GameObject>("UI/DialogueCanvas");

        if (Dialogue_UI == null)
        {
            Dialogue_UI = Instantiate(_dialogueCanvas, transform).GetComponent<Dialogue_UI>();
            Dialogue_UI.gameObject.SetActive(false);
        }

        Dialogue_UI.AutoToggle.onValueChanged.AddListener(OnAutoToggleChanged);
        Dialogue_UI.SkipToggle.onValueChanged.AddListener(OnSkipToggleChanged);
    }

    void Update()
    {
        if (IsDialogueCompleted || GameManager.Instance.ShouldWaitForDialogue())
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ProcessPlayerInput();
            IsSkip = false;
            IsAuto = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            IsSkip = !IsSkip;
        }

        if (Input.GetKeyDown(KeyCode.A))
            IsAuto = !IsAuto;


        if (IsSkip)
        {
            _skipTimer -= Time.deltaTime;

            if (_skipTimer <= 0f)
            {
                _skipTimer = SkipDelay;
                ProcessPlayerInput();
            }
        }

        if (IsAuto && Dialogue_UI.IsPrintComplete)
        {
            _autoPlayTimer -= Time.deltaTime;

            if (_autoPlayTimer <= 0f)
            {
                _autoPlayTimer = AutoPlayDelay;
                ProcessPlayerInput();
            }
        }
    }

    public void StartDialogue(DialogueCategory category, string dialogueID)
    {
        if (!IsDialogueCompleted)
        {
            Debug.LogError("Dialogue가 완전히 끝나지 않은 상태로 실행되었습니다.");
        }

        IsDialogueCompleted = false;
        LoadDialogueLines(category, dialogueID);
        Dialogue_UI.gameObject.SetActive(true);
        Dialogue_UI.ShowDialogue(_dialogueLines[_dialogueIndex]);
    }

    public void ProcessPlayerInput()
    {
        _autoPlayTimer = AutoPlayDelay;

        if (!Dialogue_UI.IsPrintComplete)
        {
            Dialogue_UI.DoSkip = true;
            return;
        }

        if (!IsOneShotCompleted)
        {
            IsOneShotCompleted = true;
            CloseDialogueUI();
            return;
        }
        else
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
        IsDialogueCompleted = true;
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

    public void PlayOneShot(DialogueCategory category, string dialogueID)
    {
        IsDialogueCompleted = false;
        IsOneShotCompleted = false;
        var line = DialogueDatabase_JSON.Instance.GetDialogue(category, dialogueID);
        if (line == null)
        {
            Debug.LogError($"로드된 Dialogue의 내용이 존재하지 않습니다. {dialogueID}");
        }

        Dialogue_UI.gameObject.SetActive(true);
        Dialogue_UI.ShowDialogue(line);
    }

    #region Event Handlers
    // Toggle의 onValueChanged에 연결되는 이벤트 핸들러
    void OnAutoToggleChanged(bool auto) { _isAuto = auto; }

    void OnSkipToggleChanged(bool skip) { _isSkip = skip; }
    #endregion
}
