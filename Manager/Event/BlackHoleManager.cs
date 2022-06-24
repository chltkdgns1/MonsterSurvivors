using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    private List<GameObject> m_ObBlackHoleList = new List<GameObject>();

    [SerializeField]
    private GameObject m_ObBlackHoleOb = null;

    [SerializeField]
    private int m_nBlackHoleCnt;

    [SerializeField]
    private float m_fWaitTime;
    private float m_fRemainWaitTime;

    void Awake()
    {
        if (m_ObBlackHoleOb == null) return;

        for(int i = 0; i < m_nBlackHoleCnt; i++)
        {
            m_ObBlackHoleList.Add(Instantiate(m_ObBlackHoleOb, transform.position, transform.rotation));
            m_ObBlackHoleList[i].SetActive(false);
        }

        SetWaitTime();
    }

    void SetWaitTime()
    {
        m_fRemainWaitTime = m_fWaitTime;
    }
    // Update is called once per frame
    void Update()
    {
        m_fWaitTime -= Time.deltaTime;
        if (m_fWaitTime > 0f) return;

        SetWaitTime();
        RandPosition();
    }

    void RandPosition()
    {
        for(int i = 0; i < m_nBlackHoleCnt; i++)
        {
            Module.GetRandPosition(m_ObBlackHoleList[i].transform, 10f, 10f);
        }
    }
}
