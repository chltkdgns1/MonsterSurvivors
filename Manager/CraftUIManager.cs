using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CraftStruct
{
    public int m_nCraftType;
    public float m_fShieldValue;
    public float m_fDamageValue;
    public float m_fNuckbackValue;
    public float m_fBloodValue;

    public string m_sAdditionalName;
    public string m_sName;

    public CraftStruct(int nCraftType, float fShieldValue, float fDamageValue, float fNuckbackValue, float fBloodValue,  string sName, string sAddName)
    {
        m_nCraftType        = nCraftType;
        m_fShieldValue      = fShieldValue;
        m_fDamageValue      = fDamageValue;
        m_fNuckbackValue    = fNuckbackValue;
        m_fBloodValue       = fBloodValue;

        m_sName             = sName;
        m_sAdditionalName   = sAddName;

    }
}

public class CraftUIManager : MonoBehaviour
{
 //static public SkillStatusManager instance = null;

    [SerializeField]
    private int m_nCnt = 20;

    [SerializeField]
    private GameObject m_ObPrefabsCraftUI;

    [SerializeField]
    private GameObject m_ObParent;

    private List<GameObject> m_ObCraftUIList = new List<GameObject>();


    private void Awake()
    {
        AddPrefabs();
    }

    void AddPrefabs()
    {
        for(int i = 0; i < m_nCnt; i++)
        {
            m_ObCraftUIList.Add(Instantiate(m_ObPrefabsCraftUI, m_ObParent.transform));
            m_ObCraftUIList[i].SetActive(false);
        }
    }

    void Start()
    {
       
    }

    private void OnEnable()
    {
        for (int i = 0; i < m_nCnt; i++)
            m_ObCraftUIList[i].SetActive(false);

        //if (SkillManager.instance == null) return;

        //int sz = SkillManager.instance.GetSkillStatusDataCnt();
        //for(int i = 0; i < sz; i++)
        //{
        //    SkillStatusStruct tempStruct = SkillManager.instance.GetSkillStatusData(i);
        //    m_ObSkillStatusList[i].GetComponent<SkillStatus>().SetData(tempStruct);
        //    m_ObSkillStatusList[i].SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
