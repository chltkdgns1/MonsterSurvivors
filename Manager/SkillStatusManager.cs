using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct SkillStatusStruct
{
    public int      m_nSkillType;
    public float    m_fDamageValue;
    public float    m_fCoolTimeValue;
    public float    m_fSizeUpValue;
    public float    m_fNuckbackValue;
    public float    m_fBloodValue;
    public float    m_fSumDamage;
    public float    m_fDPS;

    public int m_nSkillCnt;
    public string m_sSkillActive;
    public string m_sSkillName;

    public SkillStatusStruct(int nSkillType, float fDamageValue, float fCoolTimeValue, float fSizeUpValue, float fNuckbackValue,
                                float fBloodValue, int nSkillCnt, string sSkillActive, string sSkillName,float fSumDamage = 0f, float fDPS = 0f)
    {
        m_nSkillType        = nSkillType;
        m_fDamageValue      = fDamageValue;
        m_fCoolTimeValue    = fCoolTimeValue;
        m_fSizeUpValue      = fSizeUpValue;
        m_fNuckbackValue    = fNuckbackValue;
        m_fBloodValue       = fBloodValue;
        m_nSkillCnt         = nSkillCnt;
        m_sSkillName        = sSkillName;
        m_sSkillActive      = sSkillActive;
        m_fSumDamage        = fSumDamage;
        m_fDPS              = fDPS;
    }
}

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
            SkillStatusStruct tempStruct = SkillManager.instance.GetSkillStatusData(i);
            m_ObSkillStatusList[i].GetComponent<SkillStatus>().SetData(tempStruct);
            m_ObSkillStatusList[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
