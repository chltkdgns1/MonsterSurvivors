using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tower : Structure 
{
    DataManage.CraftTower m_Tower;

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

    //[SerializeField]
    //private Image m_ObImage;

    //[SerializeField]
    //private Text[] m_ObUIList;

    //[SerializeField]
    //private Text m_ObPriceText;

    //private UIEvent m_Event;


    private void Awake()
    {
        AwakeInit();
    }

    private void Start()
    {
        StartInit();
    }

    private void OnEnable()
    {
        //EnableInit();
        //if (m_bGameType) return;
        //GetData();
        //SetUITextData();
    }

    void AwakeInit() // Awake 에서만 초기화
    {
        //m_Event = GetComponent<UIEvent>();
        m_Tower = DataManage.InitData.instance.GetCraftTower(m_nType);
    }

    void EnableInit() // Enable 되었을 때만 초기화
    {

    }

    void StartInit() // Start 에서만 초기화
    {
        //SetEventParam();                        // event 파람 정해놓음.
    }

    void ReInit() // 코드 로직 중에 초기화 필요한 경우
    {

    }

    void SetUITextData()
    {
        //string sReinHp          = "";
        //string sReinShield      = "";
        //string sReinDamage      = "";
        //string sReinNuckback    = "";
        //string sReinBlood       = "";
        //string sReinShootSpeed  = "";

        //if (m_fReinforceHp          >= 1f)      sReinHp          = "(+" + Module.GetHpText(m_fReinforceHp)                   + ")";
        //if (m_fReinforceShield      >= 1f)      sReinShield      = "(+" + Module.GetDamageText((int)m_fReinforceShield)      + ")";
        //if (m_fReinforceDamage      >= 1f)      sReinDamage      = "(+" + Module.GetDamageText((int)m_fReinforceDamage)      + ")";
        //if (m_fReinforceNuckback    >= 1f)      sReinNuckback    = "(+" + Module.GetPercentText((int)m_fReinforceNuckback)   + ")";
        //if (m_fReinforceShootSpeed  >= 1f)      sReinShootSpeed  = "(+" + Module.GetCountText((int)m_fReinforceShootSpeed)   + ")";
        //if (m_fReinforceBlood       >= 0.1f)    sReinBlood       = "(+" + Module.GetPercentText(m_fReinforceBlood)           + ")";

        //m_ObUIList[0].text = Module.GetHpText(m_fHp)                            + sReinHp;
        //m_ObUIList[1].text = Module.GetDamageText((int)m_fShield)               + sReinShield;
        //m_ObUIList[2].text = Module.GetDamageText((int)m_fMinDamage)            + sReinDamage + " ~ " + Module.GetDamageText((int)m_fMaxDamage) + sReinDamage;
        //m_ObUIList[3].text = Module.GetPercentText((int)m_fNuckback)            + sReinNuckback;
        //m_ObUIList[4].text = Module.GetPercentText(m_fBlood)                    + sReinBlood;
        //m_ObUIList[5].text = Module.GetCountText((int)m_fShootSpeed)            + sReinShootSpeed;

        //m_ObPriceText.text = Module.GetMoneyString(m_nPrice) + "G";
    }

    void GetData()
    {
        // 데미지
        // 넉백
        // 블러드
        // hp
        // 쉴드
        // 등에 대한 전반적인 업그레이드 데이터 가져옴.
        // 가져와서 적용함.
    }

    //public void SetEventParam()
    //{
    //    string[] param = {  m_nGroupType.ToString(),    m_nTowerType.ToString(),    m_fMinDamage.ToString(),    m_fMaxDamage.ToString(),
    //                        m_fNuckback.ToString(),     m_fBlood.ToString(),        m_fShootSpeed.ToString()       };
    //    m_Event.InitCommunicateValue(param);
    //}
}
