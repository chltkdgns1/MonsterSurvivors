using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private List<Transform> m_ObSuckedUpArray = new List<Transform>();

    [SerializeField]
    private bool m_bInOutSide = false;

    [SerializeField]
    private float m_fDistance;

    [SerializeField]
    private float m_fSuckUpPower;

    [SerializeField]
    private string m_sNomalSuckedUpObject;
    // 기본적인 빨아들일 오브젝트, 플레이어.

    void Awake()
    {
        m_ObSuckedUpArray.Add(GameObject.FindGameObjectWithTag(m_sNomalSuckedUpObject).transform);
    }
    
    void Update()
    {
        for(int i = 0; i < m_ObSuckedUpArray.Count; i++)
        {
            float fDistance = Vector3.Distance(transform.position, m_ObSuckedUpArray[i].transform.position);
            if (fDistance <= m_fDistance)
            {
                Vector3 vDir = (transform.position - m_ObSuckedUpArray[i].transform.position).normalized * m_fSuckUpPower;
                vDir = new Vector3(vDir.x, vDir.y, 0);
                if (m_bInOutSide == true) vDir = -vDir; // 바깥쪽으로 밀어낸다. 

                m_ObSuckedUpArray[i].transform.position += vDir;
            }
        }
    }

    public void SetSuckUpObject(Transform ObTransform)
    {
        m_ObSuckedUpArray.Add(ObTransform);
    }

    public void SetSuckUpClear()
    {
        m_ObSuckedUpArray.Clear();
    }

    public float GetSuckUpPower()
    {
        return m_fSuckUpPower;
    }

    public void SetSuckUpPower(float fSuckPower)
    {
        m_fSuckUpPower = fSuckPower;
    }
}
