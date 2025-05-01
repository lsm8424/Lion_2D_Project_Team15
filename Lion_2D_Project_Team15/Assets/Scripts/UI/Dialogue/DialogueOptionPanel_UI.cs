using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionPanel_UI : MonoBehaviour
{
    public DialogueOption_UI[] DialogueOption_UIs;
    [SerializeField] GameObject _option_UIPrefab;
    [SerializeField] Dialogue_UI _dialogue_UI;
    [SerializeField] VerticalLayoutGroup _verticalLayoutGroup;
    [SerializeField] RectTransform _rectTransform;

    public void CreateOptions(params DialogueOption[] options)
    {
        DialogueOption_UIs = new DialogueOption_UI[options.Length];
        Resize(options.Length);

        for (int i = 0; i < options.Length; ++i)
        {
            GameObject spawnedUI = Instantiate(_option_UIPrefab, transform);
            var script = spawnedUI.GetComponent<DialogueOption_UI>();
            script.SetUpOption(options[i]);
            DialogueOption_UIs[i] = script;
        }

        StartCoroutine(UseLayoutGroup());
    }

    public void Resize(int optionNumber, float padding = 0)
    {
        Vector2 size = _rectTransform.sizeDelta;
        size.y = optionNumber * _option_UIPrefab.GetComponent<RectTransform>().sizeDelta.y + (optionNumber - 1) * padding;
        _rectTransform.sizeDelta = size;
    }

    internal void ClearOptions()
    {
        for (int i = 0; i < transform.childCount; ++i)
            Destroy(transform.GetChild(i).gameObject);
    }

    IEnumerator UseLayoutGroup()
    {
        _verticalLayoutGroup.enabled = true;
        yield return new WaitForEndOfFrame();
        _verticalLayoutGroup.enabled = false;
    }
}
