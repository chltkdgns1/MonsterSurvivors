using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CraftManager : MonoBehaviour
{

    static public CraftManager instance = null;

    [SerializeField]
    private SubArray [] m_ObPrefabs;

    private List<List<GameObject>> m_ObCraftObject = new List<List<GameObject>>();

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
        PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_CRAFTING);
        m_ParamData = param;

        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        m_ObCraftObject[nGroup][nType].SetActive(true);
        ITouchCraftManager tempTouch = m_ObCraftObject[nGroup][nType].GetComponent<ITouchCraftManager>();
        tempTouch.RegistTouchEvnet();
    }


    public void EndCrafting()
    {
        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        ITouchCraftManager tempTouch = m_ObCraftObject[nGroup][nType].GetComponent<ITouchCraftManager>();
        tempTouch.DeleteTouchEvent();
        m_ObCraftObject[nGroup][nType].SetActive(false);

        PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_CRAFTING);
    }

    void AwakeInit() // Awake 에서만 초기화
    {
        InitObject();
    }

    void InitObject()
    {
        int nGroupSize = m_ObPrefabs.Length;
        for(int i = 0; i < nGroupSize; i++)
        {
            List<GameObject> tempList = new List<GameObject>();
            int nSz = m_ObPrefabs[i].m_subArray.Length;
            for(int k = 0; k < nSz; k++)
            {
                tempList.Add(Instantiate(m_ObPrefabs[i].m_subArray[k]));
                tempList[k].SetActive(false);
            }
            m_ObCraftObject.Add(tempList);
        }
    }

    void EnableInit() // Enable 되었을 때만 초기화
    {

    }

    void StartInit() // Start 에서만 초기화
    {
      
    }

    void ReInit() // 코드 로직 중에 초기화 필요한 경우
    {

    }
}
