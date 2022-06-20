using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    static public DrawGrid instance = null;

    [SerializeField]
    private GameObject m_RenderLine;

    private List<Vector3> m_vColList = new List<Vector3>();
    private List<Vector3> m_vRowList = new List<Vector3>();
    private int m_nLength;

    private List<LineRenderer> m_ColRenderer = new List<LineRenderer>();
    private List<LineRenderer> m_RowRenderer = new List<LineRenderer>();
    private List<GameObject> m_ObList = new List<GameObject>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Init();
    }

    void Init()
    {
        m_nLength = 0;
    }

    public void SetLinePosition(int length)
    {
        m_vColList.Clear();
        m_vRowList.Clear();

        if(m_ColRenderer.Count < length * 2)
        {
            int dist = length * 2 - m_ColRenderer.Count;
            for(int i = 0; i < dist; i++)
            {
                m_ObList.Add(Instantiate(m_RenderLine));
                m_ObList.Add(Instantiate(m_RenderLine));              
                LineRenderer temp1 = m_ObList[m_ObList.Count - 2].GetComponent<LineRenderer>();
                LineRenderer temp2 = m_ObList[m_ObList.Count - 1].GetComponent<LineRenderer>();
                temp1.startWidth = 0.05f;
                temp1.endWidth = 0.05f;
                temp2.startWidth = 0.05f;
                temp2.endWidth = 0.05f;
                m_ColRenderer.Add(temp1);
                m_RowRenderer.Add(temp2);
            }
        }

        m_nLength = length;

        float fEnd = (float)(m_nLength - 1);

        for (int i = -m_nLength; i < m_nLength; i++)
        {
            m_vColList.Add(new Vector3(i, -m_nLength, -7));
            m_vColList.Add(new Vector3(i, fEnd, -7));
            m_vRowList.Add(new Vector3(-m_nLength, i, -7));
            m_vRowList.Add(new Vector3(fEnd, i, -7));
        }
    }

    public void RenderingGrid()
    {
        int index = 0;   
        for (int i = 0; i < m_nLength * 2; i++)
        {
            m_ColRenderer[i].SetPosition(0, m_vColList[index]);
            m_ColRenderer[i].SetPosition(1, m_vColList[index + 1]);

            m_RowRenderer[i].SetPosition(0, m_vRowList[index]);
            m_RowRenderer[i].SetPosition(1, m_vRowList[index + 1]);

            index += 2;
        }
    }

    public void SetActive(bool flag)
    {
        int cnt = m_ObList.Count;
        for (int i = 0; i < cnt; i++)
        {
            m_ObList[i].SetActive(flag);
        }
    }
}
