using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputManager : MonoBehaviour
{
    public static KeyInputManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private int keyInputCount;
    private float duration;
    private int maxKeyCount = 5; // 최대 키 입력 개수

    [Header("UI Elements")]
    [SerializeField] private GameObject keyInputPanel;
    [SerializeField] private Image[] keyInputImage; // 키 입력 이미지
    [SerializeField] private TextMeshProUGUI[] keyTexts; // 키 3개 출력용
    [SerializeField] private Image timerBar;

    private List<KeyCode> requiredKeys = new List<KeyCode>();
    private int currentIndex = 0;
    private float timer;

    private bool isActive = false;
    private System.Action onSuccess;
    private System.Action onFail;

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        timerBar.fillAmount = timer / duration;

        if (timer <= 0)
        {
            EndInput(false);
            return;
        }


        if (Input.GetKeyDown(requiredKeys[currentIndex]))
        {
            keyInputImage[currentIndex].gameObject.SetActive(false);
            currentIndex++;

            if (currentIndex >= requiredKeys.Count)
            {
                EndInput(true);
            }
        }
    }

    public void StartKeyInput(int keycount, float keyduration, System.Action successCallback, System.Action failCallback)
    {
        keyInputPanel.SetActive(true);
        keyInputCount = keycount;
        timer = keyduration;
        duration = keyduration;
        isActive = true;
        onSuccess = successCallback;
        onFail = failCallback;

        requiredKeys.Clear();
        currentIndex = 0;

        for (int i = 0; i < keyInputCount; i++)
        {
            KeyCode randomKey = GetRandomKey();
            requiredKeys.Add(randomKey);
            keyTexts[i].text = randomKey.ToString();
            keyTexts[i].color = Color.black;
            keyInputImage[i].gameObject.SetActive(true);
        }

        // 나머지 텍스트 비우기
        for (int i = keyInputCount; i < keyTexts.Length; i++)
        {
            keyInputImage[i].gameObject.SetActive(false);
            keyTexts[i].text = "";
        }
    }

    private KeyCode GetRandomKey()
    {
        int ranindex = Random.Range(0, 12);

        KeyCode key = KeyCode.None;

        switch (ranindex)
        {
            case 0: key = KeyCode.Q; break;
            case 1: key = KeyCode.W; break;
            case 2: key = KeyCode.E; break;
            case 3: key = KeyCode.R; break;
            case 4: key = KeyCode.A; break;
            case 5: key = KeyCode.S; break;
            case 6: key = KeyCode.D; break;
            case 7: key = KeyCode.F; break;
            case 8: key = KeyCode.Z; break;
            case 9: key = KeyCode.X; break;
            case 10: key = KeyCode.C; break;
            case 11: key = KeyCode.V; break;
        }


        return key;
    }

    private void EndInput(bool success)
    {
        isActive = false;
        keyInputPanel.SetActive(false);

        if (success)
            onSuccess?.Invoke();
        else
            onFail?.Invoke();
    }

}
