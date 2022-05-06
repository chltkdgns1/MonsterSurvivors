using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextManager : MonoBehaviour
{
    static public PrintTextManager instance = null;
    private List<GameObject> m_ObTextList = new List<GameObject>();
    private int m_nCur;

    [SerializeField]
    private GameObject m_ObTextPrefabs;
    [SerializeField]
    private GameObject m_Parent;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(instance);

        init();
    }
    private void init()
    {
        m_nCur = 0;
        for (int i = 0; i < 1000; i++)
        {
            GameObject temp = Instantiate(m_ObTextPrefabs, m_Parent.transform);
            m_ObTextList.Add(temp);
            temp.SetActive(false);
        }
    }

    public void SetText(Vector3 vPosition, string sPrint, bool flag = false, string sColor = "white")
    {
        if (!flag) m_ObTextList[m_nCur].transform.position = Camera.main.WorldToScreenPoint(vPosition + new Vector3(0, 0.5f));
        else m_ObTextList[m_nCur].transform.position = vPosition;
        m_ObTextList[m_nCur].SetActive(true);
        m_ObTextList[m_nCur].GetComponent<PrintText>().SetText(sPrint, sColor);
        m_nCur++; m_nCur %= 1000;
    }
}
