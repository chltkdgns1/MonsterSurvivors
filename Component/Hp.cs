using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [SerializeField]
    private float m_fMaxHp;
    private float m_fRemainHp;

    private int m_nHpKey = -1;

    [SerializeField]
    private bool m_bUseHpBar = false;


    private void Awake()
    {

    }

    void Start()
    {
        m_fRemainHp = m_fMaxHp;

        if (m_bUseHpBar == false) return;
        AddHp();
        ChangeRemainHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddHp()
    {
        m_nHpKey = HpBar.instance.AddObject(transform.position, (int)m_fMaxHp, (int)m_fRemainHp, 0, 0.7f);
    }

    public void MoveHpBar(Vector3 vPosition)
    {
        HpBar.instance.ChangePosition(m_nHpKey, vPosition);
    }

    public void ChangeRemainHp()
    {
        HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_fRemainHp);
    }

    public void SetMaxHp()
    {
        m_fRemainHp = m_fMaxHp;
    }

    public void SetActive(bool flag)
    {
        HpBar.instance.ChangeActive(m_nHpKey, flag);
    }

    public void SetDamage(float fDamage)
    {
        m_fRemainHp -= fDamage;
        if (m_fRemainHp <= 0f) m_fRemainHp = 0f;
    }

    public void SetPlusHp(float fPlusHp)
    {
        m_fRemainHp += fPlusHp;
        m_fRemainHp = Mathf.Min(m_fRemainHp, m_fMaxHp);
    }

    public float GetRemainHp()
    {
        return m_fRemainHp;
    }

}
