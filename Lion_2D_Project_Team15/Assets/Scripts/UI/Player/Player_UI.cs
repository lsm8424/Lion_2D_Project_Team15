using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Slider _healthBar;

    Player _player;

    private static Player_UI _instance;

    void Awake()
    {
        // Singleton 방식으로 Canvas 유지
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환에도 삭제되지 않음
        }
        else
        {
            Destroy(gameObject);  // 중복 생성 방지
        }

        // Slider를 자식에서 가져오기
        if (_healthBar == null)
        {
            _healthBar = GetComponentInChildren<Slider>();
        }
    }

    void Start()
    {
        _player = Player.Instance;

        if (_player != null)
        {
            _player.OnDamaged += UpdateHealthBar;
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        if (_player != null && _healthBar != null)
        {
            _healthBar.value = _player.HP;
        }
    }
}
