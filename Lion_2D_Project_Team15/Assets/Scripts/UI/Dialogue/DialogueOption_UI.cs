using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOption_UI : MonoBehaviour
{
    public string NextDialogueId { get; private set; }
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Button _button;
    Dialogue_UI _dialogue_UI;

    void Awake()
    {
        _dialogue_UI = GetComponentInParent<Dialogue_UI>();
    }

    public void SetUpOption(DialogueOption option)
    {
        _text.SetText(option.text);
        NextDialogueId = option.nextId;
    }

    public void OnPressed()
    {
        _dialogue_UI.ChooseOption(NextDialogueId);
    }

    void OnEnable()
    {
        _button.onClick.AddListener(OnPressed);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(OnPressed);
    }
}
