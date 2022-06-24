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

    }

    void AwakeInit() // Awake ������ �ʱ�ȭ
    {

    }

    void EnableInit() // Enable �Ǿ��� ���� �ʱ�ȭ
    {

    }

    void StartInit() // Start ������ �ʱ�ȭ
    {
        m_Tower = DataManage.InitData.instance.GetCraftTower(m_nType);
    }

    void ReInit() // �ڵ� ���� �߿� �ʱ�ȭ �ʿ��� ���
    {

    }
}
