using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CraftManager : MonoBehaviour  // ũ�����ÿ� ���õ� �͵鸸 ó����.
{
    static public CraftManager instance = null;

    private List<List<GameObject>> m_vObList = new List<List<GameObject>>();

    private string[] m_ParamData;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        AwakeInit();
    }

    private void OnEnable()
    {
        EnableInit();
    }

    void Start()
    {
        StartInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCrafting(string[] param)
    {

        if(PlayingGameManager.GetGameState() != DefineManager.GameState.PLAYING_STATE_CRAFTING)
            PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_CRAFTING);

        m_ParamData = param;

        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        m_vObList[nGroup][nType].SetActive(true);
        ITouchCraftManager tempTouch = m_vObList[nGroup][nType].GetComponent<ITouchCraftManager>();
        tempTouch.RegistTouchEvnet();
    }


    public void EndCrafting()
    {
        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        m_vObList[nGroup][nType].SetActive(false);
        ITouchCraftManager tempTouch = m_vObList[nGroup][nType].GetComponent<ITouchCraftManager>();
        tempTouch.DeleteTouchEvent();
        m_vObList[nGroup][nType].SetActive(false);

        PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_CRAFTING);
    }

    void AwakeInit() // Awake ������ �ʱ�ȭ
    {
        InitObject();
    }

    void InitObject()
    {
        List<DataManage.CraftStructure> tempStructure   = DataManage.InitData.instance.GetStructureList();
        List<DataManage.CraftTower> tempTower           = DataManage.InitData.instance.GetTowerList();
        List<DataManage.CraftTrap> tempTrap             = DataManage.InitData.instance.GetTrapList();

        for(int i = 0; i < 3; i++)
        {
            m_vObList.Add(new List<GameObject>());
        }


        for (int i = 0; i < tempTower.Count; i++)
        {
            m_vObList[0].Add(Instantiate(tempTower[i].m_ob));
            m_vObList[0][i].SetActive(false);
        }

        for (int i = 0; i < tempStructure.Count; i++)
        {
            m_vObList[1].Add(Instantiate(tempStructure[i].m_ob));
            m_vObList[1][i].SetActive(false);
        }

        for (int i = 0; i < tempTrap.Count; i++)
        {
            m_vObList[2].Add(Instantiate(tempTrap[i].m_ob));
            m_vObList[2][i].SetActive(false);
        }
    }

    void EnableInit() // Enable �Ǿ��� ���� �ʱ�ȭ
    {

    }

    void StartInit() // Start ������ �ʱ�ȭ
    {
      
    }

    void ReInit() // �ڵ� ���� �߿� �ʱ�ȭ �ʿ��� ���
    {

    }
}
