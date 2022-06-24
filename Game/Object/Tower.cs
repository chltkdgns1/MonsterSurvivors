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

    void AwakeInit() // Awake 에서만 초기화
    {

    }

    void EnableInit() // Enable 되었을 때만 초기화
    {

    }

    void StartInit() // Start 에서만 초기화
    {
        m_Tower = DataManage.InitData.instance.GetCraftTower(m_nType);
    }

    void ReInit() // 코드 로직 중에 초기화 필요한 경우
    {

    }
}
