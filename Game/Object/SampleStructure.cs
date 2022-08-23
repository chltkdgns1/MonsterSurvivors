using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void SampleCallBack();

public class SampleStructure : MonoBehaviour
{
    private SampleCallBack m_callBack = null;
    private Vector3 m_vPosition;

    public SampleCallBack CallBack
    {
        set { m_callBack = value; }
    }

    public Vector3 Position
    {
        set { m_vPosition = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_callBack != null)
                m_callBack();
            return;
        }
    }
}
