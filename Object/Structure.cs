using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour // 모든 구조물 데이터는 StructureData 를 상속받음.
{
    [SerializeField]
    protected float m_fShield;
    [SerializeField]
    protected float m_fHp;
    [SerializeField]
    protected int m_nPrice;
    [SerializeField]
    protected bool m_bGameType = false;   // UI 용으로 쓸 것인지, 인게임용으로 쓸 것인지

    protected float m_fReinforceHp        = 0f;
    protected float m_fReinforceShield    = 0f;
    protected float m_fReinforcePrice     = 0f;

    public void SetShield(float fShield) { m_fShield = fShield; }
    public float GetShield() { return m_fShield; }
    public void SetHp(float fHp) { m_fHp = fHp; }
    public float GetHp() { return m_fHp; }
    public float GetPrice() { return m_nPrice; }
    public void SetPrice(int nPrice) { m_nPrice = nPrice; }
}
