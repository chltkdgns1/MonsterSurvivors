using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureUI : MonoBehaviour, IComponentStyle
{
    private DataManage.CraftStructure m_Structure;


    public float Shield
    {
        get { return m_Structure.m_fShield; }
        set { m_Structure.m_fShield = value; }
    }
    public float Hp
    {
        get { return m_Structure.m_fHp; }
        set { m_Structure.m_fHp = value; }
    }
    public int Price
    {
        get { return m_Structure.m_nPrice; }
        set { m_Structure.m_nPrice = value; }
    }
    public int ReinforceState
    {
        get { return ReinforceState; }
        set { ReinforceState = value; }
    }
    public string sName
    {
        get { return m_Structure.m_sName; }
        set { m_Structure.m_sName = value; }
    }

    [SerializeField]
    protected Image m_ObImage;

    [SerializeField]
    protected Text[] m_ObUIList;

    [SerializeField]
    protected int m_nType = 0;

    protected UIEvent m_Event;

    protected DefineManager.Crafts m_craftType;

    private void Start()
    {
        StartInit();
    }

    private void OnEnable()
    {

    }

    public void AwakeInit() // Awake ������ �ʱ�ȭ
    {
        m_craftType = DefineManager.Crafts.STRUCT;
    }

    public void EnableInit() // Enable �Ǿ��� ���� �ʱ�ȭ
    {

    }

    public void StartInit() // Start ������ �ʱ�ȭ
    {
        m_Structure = DataManage.InitData.instance.GetCraftStructure(m_nType);
        m_Event = GetComponent<UIEvent>();
        SetEventParam();
        SetImage();
        GetData();
        SetString();
        SetUITextData();
    }

    public void ReInit() // �ڵ� ���� �߿� �ʱ�ȭ �ʿ��� ���
    {

    }

    protected void SetImage()
    {
        m_ObImage.sprite = m_Structure.m_sprite;
    }

    protected void SetString()
    {

    }

    protected void SetUITextData()
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

    protected void GetData()
    {
        // ������
        // �˹�
        // ����
        // hp
        // ����
        // � ���� �������� ���׷��̵� ������ ������.
        // �����ͼ� ������.
    }

    protected void SetEventParam()
    {
        string[] param = { ((int)m_craftType).ToString(), m_nType.ToString() };
        m_Event.InitCommunicateValue(param);
    }
}
