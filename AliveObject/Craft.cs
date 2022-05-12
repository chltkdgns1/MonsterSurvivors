using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    // hp 부분 HP Component 에서 관리
    [SerializeField]
    private float m_fMinDamage;
    [SerializeField]
    private float m_fMaxDamage;
    [SerializeField]
    private float m_fShield;
    [SerializeField]
    private float m_fNuckback;
    [SerializeField]
    private float m_fBlood;

    [SerializeField]
    private int m_nType;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamage()
    {
        return UnityEngine.Random.Range(m_fMinDamage, m_fMaxDamage);
    }

    public float GetShield()    {   return m_fShield;   }

    public float GetNuckBack()  {   return m_fNuckback; }

    public float GetBlood()     {   return m_fBlood;    }
}
