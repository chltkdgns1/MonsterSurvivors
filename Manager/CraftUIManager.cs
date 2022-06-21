using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUIManager : MonoBehaviour
{
 //static public SkillStatusManager instance = null;

    [SerializeField]
    private SubArray[] m_ObPrefabsCraftUI;

    [SerializeField]
    private GameObject m_ObParent;

    private List<List<GameObject>> m_ObCraftUIList = new List<List<GameObject>>();

    private int m_nTagState = 0;

    [SerializeField]
    private GameObject[] m_RadioButtonGroup;

    private Color m_color;
    private Color m_colorSelect;

    private void Awake()
    {
        AwakeInit();
    }

    private void Start()
    {
        StartInit();
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
        UnPrintAllGroup();
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
        InitRadio();
    }

    void InitRadio()
    {
        int index = GroupManager.instance.RegisterGroup(m_RadioButtonGroup);

        float value = 1f / 255;

        m_color = new Color(78f * value, 79f * value, 71f * value, 70f * value);
        m_colorSelect = new Color(116f * value, 238f * value, 134f * value, 50f * value);
        int cnt = m_RadioButtonGroup.Length;
        for(int i = 0; i <cnt; i++)
        {
            RadioButton tempButton = m_RadioButtonGroup[i].GetComponent<RadioButton>();
            tempButton.run = (object []ob) =>
            {
                Image imageTemp = ((GameObject)ob[0]).GetComponent<Image>();
                imageTemp.color = m_colorSelect;

                m_nTagState = (int)ob[1];
                UnPrintAllGroup();
                PrintGroup();
            };

            tempButton.stop = (object []ob) =>
            {
                Image imageTemp = ((GameObject)ob[0]).GetComponent<Image>();
                imageTemp.color = m_color;
            };

            Clicked Clicktemp = m_RadioButtonGroup[i].GetComponent<Clicked>();
            Clicktemp.SetParam(new string[2] { tempButton.GetGId().ToString(), tempButton.GetGIndex().ToString() });
        }

        GroupManager.instance.GroupAction(index, 0);
    }

    void ReInit() // 코드 로직 중에 초기화 필요한 경우
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

    void UnPrintAllGroup()
    {
        int nSize = m_ObCraftUIList.Count;
        for(int i = 0; i < nSize; i++)
        {
            int len = m_ObCraftUIList[i].Count;
            for(int k = 0; k < len; k++)
            {
                m_ObCraftUIList[i][k].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTagState(int nState) { m_nTagState = nState; }
    int GetTagState() { return m_nTagState; }
}
