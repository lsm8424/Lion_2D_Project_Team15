using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    [SerializeField] Volume volume;
    private Vignette vignette;

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

        if (volume.profile.TryGet(out Vignette v))
        {
            vignette = v;
            vignette.color.value = Color.red; // 빨간색 설정
        }
    }

    void Start()
    {
        _player = Player.Instance;

        if (_player != null)
        {
            _player.OnDamaged += UpdateHealthBar;
            _player.OnDamaged += UpdateVolume;
            UpdateHealthBar();
            UpdateVolume();
        }

       
    }

    void UpdateHealthBar()
    {
        if (_player != null && _healthBar != null)
        {
            _healthBar.value = _player.HP;
        }
    }

    void UpdateVolume()
    {
        if (vignette == null)
            return;

        float hpPercent = _player.HP / _player.maxHP;

        if(hpPercent < 0.3f)
            vignette.intensity.value = 0.5f; // 체력이 20% 이하일 때 강한 효과
        else if(hpPercent < 0.5f)
            vignette.intensity.value = 0.3f; // 체력이 40% 이하일 때 중간 효과
        else if(hpPercent < 0.7f)
            vignette.intensity.value = 0.1f; // 그 외에는 약한 효과
    }
}
