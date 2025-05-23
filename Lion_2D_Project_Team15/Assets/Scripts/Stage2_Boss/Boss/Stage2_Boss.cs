using UnityEngine;
using UnityEngine.UI;

public class Stage2_Boss : Entity
{
    //보스 패턴
    private Stage2_Boss_NormalAttack normalAttack;
    private Stage2_Boss_Pattern1 pattern1;
    private Stage2_Boss_Pattern2 pattern2;
    private Stage2_Boss_Pattern3 pattern3;
    //배경 패턴
    private Stage2_BG_Pattern1 bgPattern1;
    private Stage2_BG_Pattern2 bgPattern2;

    [Header("보스 체력바")]
    [SerializeField] Slider _healthBar;
    [SerializeField] Button DamageBtn;

    private void Start()
    {
        FindComponent();
        PatternOff();

        BossPattenToHP();

        //보스라운드 true
        //Player.Instance.SetBossRound();
        if (DamageBtn != null && GameManager.Instance.EntityTimeScale == 1)
            DamageBtn.onClick.AddListener(() =>
            {
                TakeDamage(Random.Range(50, 100));
            });
    }

    private void FindComponent()
    {
        // 각 패턴 스크립트 초기화
        normalAttack = GetComponent<Stage2_Boss_NormalAttack>();
        pattern1 = GetComponent<Stage2_Boss_Pattern1>();
        pattern2 = GetComponent<Stage2_Boss_Pattern2>();
        pattern3 = GetComponent<Stage2_Boss_Pattern3>();
        bgPattern1 = GetComponentInChildren<Stage2_BG_Pattern1>();
        bgPattern2 = GetComponentInChildren<Stage2_BG_Pattern2>();

    }

    private void PatternOff()
    {
        //패턴 OFF
        normalAttack.isOn = false;
        pattern1.isOn = false;
        pattern2.isOn = false;
        pattern3.isOn = false;
        bgPattern1.isOn = false;
        bgPattern2.isOn = false;
    }


    private void Update()
    {
        if (GameManager.Instance.EntityTimeScale == 0)
        {
            PatternOff();
            return;
        }

        // 체력바 업데이트
        if (_healthBar != null)
        {
            _healthBar.value = HP / maxHP * 100;
        }  
        
        //HP에 따른 보스패턴 변경
        BossPattenToHP();

        //체력 테스트

        if (Input.GetKeyDown(KeyCode.T))
            TakeDamage(Random.Range(50, 100));

    }

    public override void TakeDamage(float value)
    {
        Debug.Log(value + " 데미지 입음");
        HP -= value;

        if (HP <= 0)
        {
            // 보스 처치 로직 추가
            _healthBar.gameObject.SetActive(false);
            DamageBtn.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void BossPattenToHP()
    {
        float hpRatio = HP / maxHP * 100;

        if (hpRatio <= 100)
            normalAttack.isOn = true;
        if (hpRatio < 90)
            pattern1.isOn = true;
        if (hpRatio < 80)
            bgPattern1.isOn = true;
        if (hpRatio < 70)
            pattern2.isOn = true;
        if (hpRatio < 60)
            pattern3.isOn = true;
        if (hpRatio < 50)
            bgPattern2.isOn = true;
    }


}
