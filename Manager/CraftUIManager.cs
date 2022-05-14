using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class CraftUIManager : MonoBehaviour
{
 //static public SkillStatusManager instance = null;

    [SerializeField]
    private SubArray[] m_ObPrefabsCraftUI;

    [SerializeField]
    private GameObject m_ObParent;

    private List<List<GameObject>> m_ObCraftUIList = new List<List<GameObject>>();

    private int m_nTagState = 0;


    private void Awake()
    {
        AwakeInit();
    }

    void AddPrefabs()
    {
        int nGroupSize = m_ObPrefabsCraftUI.Length;
        for (int i = 0; i < nGroupSize; i++)
        {
            List<GameObject> tempCraftList = new List<GameObject>();
            int sz = m_ObPrefabsCraftUI[i].m_subArray.Length;

            for (int k = 0; k < sz; k++)
            {
                tempCraftList.Add(Instantiate(m_ObPrefabsCraftUI[i].m_subArray[k], m_ObParent.transform));
                tempCraftList[k].SetActive(false);
            }
            m_ObCraftUIList.Add(tempCraftList);
        }
    }

    void AwakeInit() // Awake 에서만 초기화
    {
        AddPrefabs();
    }

    void EnableInit() // Enable 되었을 때만 초기화
    {
        m_nTagState = 0;
        PrintGroup();
    }

    void InitObject()
    {
        int nSubSize = m_ObCraftUIList.Count;
        for (int i = 0; i < nSubSize; i++)
        {
            int sz = m_ObCraftUIList[i].Count;
            for (int k = 0; k < sz; k++)
                m_ObCraftUIList[i][k].SetActive(false);
        }
    }

    void StartInit() // Start 에서만 초기화
    {

    }

    void ReInit() // 코드 로직 중에 초기화 필요한 경우
    {

    }

    void Start()
    {
       
    }

    private void OnEnable()
    {
        EnableInit();
    }

    void PrintGroup()
    {
        int nSize = m_ObCraftUIList[m_nTagState].Count;
        for (int i = 0; i < nSize; i++)
        {
            m_ObCraftUIList[m_nTagState][i].SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTagState(int nState) { m_nTagState = nState; }
    int GetTagState() { return m_nTagState; }
}
