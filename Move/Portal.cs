using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private Vector3 m_vPosition = new Vector3(0, 0, 0);

    [SerializeField]
    private string []m_sPortalUseObjectTag;

    private HashSet<string> m_hashPortalUseObject = new HashSet<string>();


    private void Awake()
    {
        for(int i = 0; i < m_sPortalUseObjectTag.Length; i++)
        {
            m_hashPortalUseObject.Add(m_sPortalUseObjectTag[i]);
        }
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)                 // 충돌 시, 다른 위치로 날려보내버림.
    {
        if (m_hashPortalUseObject.Contains(collision.tag))
        {
            collision.gameObject.transform.position = m_vPosition;
            return;
        }
    }
}
