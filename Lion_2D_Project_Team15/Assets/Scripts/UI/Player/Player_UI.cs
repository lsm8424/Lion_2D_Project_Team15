using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Slider _healthBar;

    Player _player;

    void Awake()
    {
        _healthBar = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        _player = Player.Instance;
        _player.OnDamaged += () => _healthBar.value = _player.HP / _player.maxHP;
    }
}
