using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : StructureUI
{
    private DataManage.CraftTower m_Tower;
    private DataManage.CraftTower m_ReinforceTower;

    private string m_sHp;
    private string m_sShield;
    private string m_sDamage;
    private string m_sNuckback;
    private string m_sBlood;
    private string m_sShootSpeed;
    private string m_sName;

    bool m_bStartFalg; // 실행 흐름 조절

    public float fMinDamage
    {
        get { return m_Tower.m_fMinDamage; }
        set { m_Tower.m_fMinDamage = value; }
    }

    public float fMaxDamage
    {
        get { return m_Tower.m_fMaxDamage; }
        set { m_Tower.m_fMaxDamage = value; }
    }
    public float fNuckback
    {
        get { return m_Tower.m_fNuckback; }
        set { m_Tower.m_fNuckback = value; }
    }
    public float fBlood
    {
        get { return m_Tower.m_fBlood; }
        set { m_Tower.m_fBlood = value; }
    }
    public float fShootSpeed
    {
        get { return m_Tower.m_fShootSpeed; }
        set { m_Tower.m_fShootSpeed = value; }
    }

    private void Awake()
    {
        m_bStartFalg = false;
    }

    void Start()
    {
        StartInit();
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void AwakeInit() // Awake 에서만 초기화
    {
        m_craftType = DefineManager.Crafts.TOWER;
    }

    public new void EnableInit() // Enable 되었을 때만 초기화
    {

    }

    public new void StartInit() // Start 에서만 초기화
    {
        m_Tower = DataManage.InitData.instance.GetCraftTower(m_nType); 
        m_Event = GetComponent<UIEvent>();
        SetEventParam();
        SetImage();
        GetData();
        SetString();
        SetUITextData();
    }

    public new void ReInit() // 코드 로직 중에 초기화 필요한 경우
    {

    }

    protected new void SetImage()
    {
        m_ObImage.sprite = m_Tower.m_sprite;
    }

    protected new void SetString()
    {
        m_sHp           = Module.GetHpText(m_Tower.m_fHp);
        m_sShield       = Module.GetDamageText((int)m_Tower.m_fShield);
        m_sDamage       = Module.GetDamageText((int)m_Tower.m_fMinDamage) + " ~ " + Module.GetDamageText((int)m_Tower.m_fMaxDamage);
        m_sNuckback     = Module.GetPercentText((int)m_Tower.m_fNuckback);
        m_sShootSpeed   = Module.GetPercentText(m_Tower.m_fBlood);
        m_sBlood        = Module.GetCountText((int)m_Tower.m_fShootSpeed);
        m_sName         = m_Tower.m_sName;
    }


    protected new void SetUITextData()
    {
        //if (m_fReinforceHp >= 1f) sReinHp = "(+" + Module.GetHpText(m_fReinforceHp) + ")";
        //if (m_fReinforceShield >= 1f) sReinShield = "(+" + Module.GetDamageText((int)m_fReinforceShield) + ")";
        //if (m_fReinforceDamage >= 1f) sReinDamage = "(+" + Module.GetDamageText((int)m_fReinforceDamage) + ")";
        //if (m_fReinforceNuckback >= 1f) sReinNuckback = "(+" + Module.GetPercentText((int)m_fReinforceNuckback) + ")";
        //if (m_fReinforceShootSpeed >= 1f) sReinShootSpeed = "(+" + Module.GetCountText((int)m_fReinforceShootSpeed) + ")";
        //if (m_fReinforceBlood >= 0.1f) sReinBlood = "(+" + Module.GetPercentText(m_fReinforceBlood) + ")";

        m_ObUIList[0].text = m_sHp;
        m_ObUIList[1].text = m_sShield;
        m_ObUIList[2].text = m_sDamage;
        m_ObUIList[3].text = m_sNuckback;
        m_ObUIList[4].text = m_sBlood;
        m_ObUIList[5].text = m_sShootSpeed;
        m_ObUIList[6].text = m_sName;
        m_ObUIList[7].text = Module.GetMoneyString(m_Tower.m_nPrice) + "G";
    }

    protected new void GetData()
    {
       // 강화된 정도의 데이터를 의미함.
    }



}
