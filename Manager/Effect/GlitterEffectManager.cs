using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEffect
{
    void On();
    void Off();
}

public class GlitterEffectManager : MonoBehaviour
{
    public static GlitterEffectManager instance = null;

    private Dictionary<IEffect,bool> m_effectList = new Dictionary<IEffect, bool>();
    [SerializeField]
    private float m_fGlitterTime = 1f;
    private float m_fGiltterSumTime;
    private bool m_bCheck;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
    }

    void Start()
    {
        m_bCheck = false;
        StartCoroutine(Routine());
    }

    public void Register(IEffect effectElement)
    {
        m_effectList.Add(effectElement, true);
    }

    public void UnRegister(IEffect effectElement)
    {
        effectElement.Off();
        m_effectList.Remove(effectElement);
    }

    IEnumerator Routine()
    {
        while (true)
        {
            m_fGiltterSumTime += Time.deltaTime;
            if (m_fGiltterSumTime >= m_fGlitterTime)
            {
                SetEffect();
                m_fGiltterSumTime = 0f;
            }
            yield return null;
        }
    }

    void SetEffect()
    {
        if (m_bCheck == false)
        {
            foreach(IEffect temp in m_effectList.Keys) temp.On();
            m_bCheck = true;
        }
        else
        {
            foreach (IEffect temp in m_effectList.Keys) temp.Off();
            m_bCheck = false;
        }
    }
}
