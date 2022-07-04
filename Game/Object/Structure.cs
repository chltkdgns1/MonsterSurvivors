using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour// ���� ��ġ�� ģ��
{
    private DataManage.CraftStructure m_Structure;

    [SerializeField]
    protected int m_nType = 0;

    protected int m_nReinforceState = 0;

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

    void Awake()
    {
        
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        m_Structure = DataManage.InitData.instance.GetCraftStructure(m_nType);
    }
}
