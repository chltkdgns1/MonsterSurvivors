using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    static public DamageTextManager instance = null;
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

    public void SetDamageText(Vector3 vPosition,int nPrintDamage, string sColor = "white")
    {
        m_ObTextList[m_nCur].transform.position = Camera.main.WorldToScreenPoint(vPosition + new Vector3(0, 0.5f));
        m_ObTextList[m_nCur].SetActive(true);
        m_ObTextList[m_nCur].GetComponent<DamageText>().SetDamage(nPrintDamage, sColor);
        m_nCur++; m_nCur %= 1000;
    }
}
