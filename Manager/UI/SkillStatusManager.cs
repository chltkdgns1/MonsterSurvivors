using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatusManager : MonoBehaviour
{
    //static public SkillStatusManager instance = null;

    [SerializeField]
    private int m_nCnt = 20;

    [SerializeField]
    private GameObject m_ObPrefabsSkillStatus;

    [SerializeField]
    private GameObject m_ObParent;

    private List<GameObject> m_ObSkillStatusList = new List<GameObject>();


    private void Awake()
    {
        //if (instance == null)   instance = this;
        //else                    Destroy(gameObject);

        AddPrefabs();
    }

    void AddPrefabs()
    {
        for(int i = 0; i < m_nCnt; i++)
        {
            m_ObSkillStatusList.Add(Instantiate(m_ObPrefabsSkillStatus, m_ObParent.transform));
            m_ObSkillStatusList[i].SetActive(false);
        }
    }

    void Start()
    {
       
    }

    private void OnEnable()
    {
        for (int i = 0; i < m_nCnt; i++)
            m_ObSkillStatusList[i].SetActive(false);

        if (SkillManager.instance == null) return;

        int sz = SkillManager.instance.GetSkillStatusDataCnt();
        for(int i = 0; i < sz; i++)
        {
            DataManage.SkillStatusStruct tempStruct = SkillManager.instance.GetSkillStatusData(i);
            m_ObSkillStatusList[i].GetComponent<SkillStatus>().SetData(tempStruct);
            m_ObSkillStatusList[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
