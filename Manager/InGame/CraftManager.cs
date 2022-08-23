using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct CraftData
{
    public int m_nGroup;
    public int m_nType;
    public int m_nIndex;

    public Vector3 m_position;

    public CraftData(int nGroup, int nType, int nIndex, Vector3 position)
    {
        m_nGroup = nGroup;
        m_nType = nType;
        m_nIndex = nIndex;
        m_position = position;
    }
}

class CraftElement
{
    public GameObject m_object;
    public long m_lkey;

    public CraftElement(GameObject ob, long key = -1)
    {
        m_object = ob;
        m_lkey = key;
    }
}

class CraftObject
{
    public delegate void FixCallBack(long key, int index);
    public List<CraftElement> m_CraftElement = new List<CraftElement>();
    public GameObject m_Prefabs;
    public int m_nGroup;
    public int m_nType;
    public FixCallBack m_fixCallBack;

    public CraftObject(GameObject Prefabs, int nGroup, int nType)
    {
        m_Prefabs = Prefabs;
        m_nGroup = nGroup;
        m_nType = nType;
    }

    public int UseCount
    {
        get;set;
    }

    public int Count
    {
        get { return m_CraftElement.Count; }
    }

    public void SetActiveAll(bool flag)
    {
        int sz = m_CraftElement.Count;
        for (int i = 0; i < sz; i++)
        {
            m_CraftElement[i].m_object.SetActive(flag);
        }
    }

    public int SetActive(Vector3 vPosition, long key)
    {
        if (m_CraftElement.Count <= UseCount) // 추가적으로 생성이 필요.
        {
            bool bFlag = Sort(); // 재정렬 , true : 재졍렬 시, 오브젝트 공간 남음 false : 추가적인 공간 생성 필요.
            if (bFlag == false) AddSize(m_Prefabs);
        }

        m_CraftElement[UseCount].m_object.transform.position = vPosition;
        m_CraftElement[UseCount].m_lkey = key;
        m_CraftElement[UseCount].m_object.SetActive(true);
        UseCount++;
        return UseCount - 1;
    }  

    public void SetUnActive(int index)
    {
        if (index < 0 || index >= m_CraftElement.Count) return;
        m_CraftElement[index].m_object.SetActive(false);
    }
    
    public bool Sort()
    {
        int endIndex = m_CraftElement.Count;
        for(int i = endIndex - 1; i >= 0; i--)
        {
            if (m_CraftElement[i].m_object.activeSelf == false)
            {
                if(m_CraftElement[endIndex - 1].m_object.activeSelf)
                    m_fixCallBack(m_CraftElement[endIndex - 1].m_lkey, i);
                Module.Swap<CraftElement>(m_CraftElement, i, --endIndex);
            }
        }

        UseCount = endIndex;
        if (UseCount < m_CraftElement.Count) return true;     
        return false;
    }

    public void AddSize(GameObject ob, int addSize = 30)
    {
        for(int i = 0; i <addSize; i++)
        {
            GameObject temp = Module.InstanceObject(ob);
            m_CraftElement.Add(new CraftElement(temp));
            temp.SetActive(false);
        }
    }
}

class CraftType // 오브젝트의 종류
{
    public List<CraftObject> m_CraftObject = new List<CraftObject>();
}

public class CraftManager : MonoBehaviour, ITouchCraftManager  // 크래프팅에 관련된 것들만 처리함.
{
    static public CraftManager instance = null;

    private List<CraftType> m_CraftObjectPool;
    private string[] m_ParamData;

    Dictionary<long, CraftData> m_DicMap = new Dictionary<long, CraftData>();
    Dictionary<long, CraftData> m_TemporaryDic = new Dictionary<long, CraftData>();

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

    public void SelectCrafting(string[] param)
    {
        m_ParamData = param;
    }

    public void CompleteCrafting()
    {
        foreach(KeyValuePair<long,CraftData> temp in m_TemporaryDic)
        {
            m_DicMap[temp.Key] = temp.Value;

            int group = temp.Value.m_nGroup;
            int type = temp.Value.m_nType;
            int index = temp.Value.m_nIndex;

            GlitterEffect tempEffect = m_CraftObjectPool[group].m_CraftObject[type].m_CraftElement[index].m_object.GetComponent<GlitterEffect>();
            tempEffect.UnRegister();
        }
        m_TemporaryDic.Clear();
    }

    public void CancleCrafting()
    {
        foreach (KeyValuePair<long, CraftData> temp in m_TemporaryDic)
        {          
            int group = temp.Value.m_nGroup;
            int type = temp.Value.m_nType;
            int index = temp.Value.m_nIndex;

            GlitterEffect tempEffect = m_CraftObjectPool[group].m_CraftObject[type].m_CraftElement[index].m_object.GetComponent<GlitterEffect>();
            tempEffect.UnRegister();
            m_CraftObjectPool[group].m_CraftObject[type].SetUnActive(index);
        }
        m_TemporaryDic.Clear();
    }

    public void EndCraft()
    {
        CancleCrafting();
        m_ParamData = null;
        GameUIManager.instance.SetActiveCraftingList(false);
    }

    void AwakeInit() // Awake 에서만 초기화
    {
        InitObject();
    }

    void InitObject()
    {
        List<DataManage.CraftStructure> tempStructure = DataManage.InitData.instance.GetStructureList();
        List<DataManage.CraftTower> tempTower = DataManage.InitData.instance.GetTowerList();
        List<DataManage.CraftTrap> tempTrap = DataManage.InitData.instance.GetTrapList();

        m_CraftObjectPool = new List<CraftType>();

        for (int i = 0; i < DefineManager.CRAFT_TYPE; i++)
        {
            m_CraftObjectPool.Add(new CraftType());
        }

        for (int i = 0; i < tempTower.Count; i++)
        {
            m_CraftObjectPool[0].m_CraftObject.Add(new CraftObject(tempTower[i].m_ob, (int)DefineManager.Crafts.TOWER, i));
            List<CraftElement> towerList = new List<CraftElement>();
            for (int k = 0; k < 5; k++)
            {
                towerList.Add(new CraftElement(Instantiate(tempTower[i].m_ob)));
            }

            m_CraftObjectPool[0].m_CraftObject[i].m_CraftElement = towerList;
            m_CraftObjectPool[0].m_CraftObject[i].SetActiveAll(false);
            m_CraftObjectPool[0].m_CraftObject[i].m_fixCallBack = SortKey;
        }

        for (int i = 0; i < tempStructure.Count; i++)
        {
            m_CraftObjectPool[1].m_CraftObject.Add(new CraftObject(tempStructure[i].m_ob, (int)DefineManager.Crafts.STRUCT, i));
            List<CraftElement> towerList = new List<CraftElement>();
            for (int k = 0; k < 50; k++)
            {
                towerList.Add(new CraftElement(Instantiate(tempStructure[i].m_ob)));
            }
            m_CraftObjectPool[1].m_CraftObject[i].m_CraftElement = towerList;
            m_CraftObjectPool[1].m_CraftObject[i].SetActiveAll(false);
            m_CraftObjectPool[1].m_CraftObject[i].m_fixCallBack = SortKey;

        }

        for (int i = 0; i < tempTrap.Count; i++)
        {
            m_CraftObjectPool[2].m_CraftObject.Add(new CraftObject(tempTrap[i].m_ob,(int)DefineManager.Crafts.TRAP, i));
            List<CraftElement> towerList = new List<CraftElement>();
            for (int k = 0; k < 50; k++)
            {
                towerList.Add(new CraftElement(Instantiate(tempTrap[i].m_ob)));
            }
            m_CraftObjectPool[2].m_CraftObject[i].m_CraftElement = towerList;
            m_CraftObjectPool[2].m_CraftObject[i].SetActiveAll(false);
            m_CraftObjectPool[2].m_CraftObject[i].m_fixCallBack = SortKey;
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

    public void OnOneTouch(Vector3 touchPoint)
    {
        //Vector3Int vGridPosition = Module.GetGridInt(Camera.main.ScreenToWorldPoint(touchPoint));
        //long key = vGridPosition.x * 10000 + vGridPosition.y;
        long key = Module.GetHashFunc(Camera.main.ScreenToWorldPoint(touchPoint));

        if (m_TemporaryDic.ContainsKey(key))
        {
            CraftData temp = m_TemporaryDic[key];
            Debug.Log("삭제 : " + key);
            Debug.Log("temp : " + temp.m_nGroup + " " + temp.m_nType + "  " + temp.m_nIndex);
            m_CraftObjectPool[temp.m_nGroup].m_CraftObject[temp.m_nType].SetUnActive(temp.m_nIndex);
            m_CraftObjectPool[temp.m_nGroup].m_CraftObject[temp.m_nType].m_CraftElement[temp.m_nIndex].m_lkey = -1;
            m_TemporaryDic.Remove(key);
            return;
        }

        if (m_DicMap.ContainsKey(key))
        {
            CraftData temp = m_DicMap[key];
            GameUIManager.instance.SetActiveCraftDelete(true);

            PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_PAUSE);

            GameUIManager.instance.SetCraftDeleteOkCallBack((CommunicationTypeDataClass value) =>
            {
                string sValue = value.GetParamIndex(0);
                m_DicMap.Remove(long.Parse(sValue));
                value.GetGameObject().SetActive(false);
                value.GetGameObject().GetComponent<GlitterEffect>().Register();
                GameUIManager.instance.SetActiveCraftDelete(false);
                PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_PAUSE);
            }, new CommunicationTypeDataClass(0, m_CraftObjectPool[temp.m_nGroup].m_CraftObject[temp.m_nType].m_CraftElement[temp.m_nIndex].m_object, new string[] { key.ToString() }));
        }
    }

    public void OnDrag(Vector3 firstTouch, Vector3 touchPoint)
    {
        Vector3 first = Camera.main.ScreenToWorldPoint(firstTouch);
        Vector3 second = Camera.main.ScreenToWorldPoint(touchPoint);
        CameraManager.instance.MoveCamera(first - second);
    }

    public void SetInitCameraPos()
    {
        Vector3 tempPos = PlayerOffline2D.instance.gameObject.transform.position;
        CameraManager.instance.SetPositionXY(tempPos);       
    }

    public void RegistTouchEvnet()
    {
        TouchManager.instance.RegisterEvent(this);
        MouseClickManager.instance.RegisterEvent(this);
    }

    public void DeleteTouchEvent()
    {
        TouchManager.instance.DeleteEvent(this);
        MouseClickManager.instance.DeleteEvent(this);
    }

    public void OnDoubleTouch(Vector3 doubleTouchPoint)
    {
        if (m_ParamData == null) return;
        CreateCraft(doubleTouchPoint);
    }

    void CreateCraft(Vector3 touchPoint)
    {
        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        Vector3 temp = Camera.main.ScreenToWorldPoint(touchPoint);
        long key = Module.GetHashFunc(temp);

        //Vector3 temp = Camera.main.ScreenToWorldPoint(touchPoint);
        //Vector3Int vGridPosition = Module.GetGridInt(Camera.main.ScreenToWorldPoint(touchPoint));
        //long key = vGridPosition.x * 10000 + vGridPosition.y;

        if (m_DicMap.ContainsKey(key) || m_TemporaryDic.ContainsKey(key)) return;

        temp = new Vector3(temp.x, temp.y, -1);
        int index = m_CraftObjectPool[nGroup].m_CraftObject[nType].SetActive(Module.GetGrid(temp), key);
        m_TemporaryDic.Add(key, new CraftData(nGroup, nType, index, Module.GetGrid(temp)));
    }

    public void OnZoom(float fWheel)
    {
        Camera.main.orthographicSize -= fWheel;
        // 작아질수록 확대됨 
        if (Camera.main.orthographicSize < 2f) Camera.main.orthographicSize = 2f;
        if (Camera.main.orthographicSize > 5f) Camera.main.orthographicSize = 5f;
    }

    public bool IsInsideRange(Vector3 vFirst, Vector3 vSecond)
    {
        Vector3 first = Module.GetGrid(Camera.main.ScreenToWorldPoint(vFirst));
        Vector3 second = Module.GetGrid(Camera.main.ScreenToWorldPoint(vSecond));

        if (Mathf.Abs(first.x - second.x) < 0.001f && Mathf.Abs(first.y - second.y) < 0.001f)
            return true;
        return false;
    }

    public void SortKey(long key, int index)
    {
        CraftData temp = m_TemporaryDic[key];
        temp.m_nIndex = index;
        m_TemporaryDic[key] = temp;
    }
}

//public void OnDrag(Vector3 firstTouch, Vector3 touchPoint)
//{
//    Vector3 first = Camera.main.ScreenToWorldPoint(firstTouch);
//    Vector3 second = Camera.main.ScreenToWorldPoint(touchPoint);
//    CameraManager.instance.MoveCamera(first - second);

//    //if (m_ParamData == null) return;

//    //int nGroup = int.Parse(m_ParamData[0]);
//    //int nType = int.Parse(m_ParamData[1]);

//    //Vector3 temp = Camera.main.ScreenToWorldPoint(touchPoint);
//    //m_vObList[nGroup][nType].transform.position = Module.GetGrid(new Vector3(temp.x, temp.y, -1));

//}

//public void OnManyDrag(List<Vector3> touchPoint)
//{
//    //Vector3 maxYPosition = touchPoint[0];
//    //Vector3 minYPosition = touchPoint[1];
//    //if (touchPoint[0].y < touchPoint[1].y)
//    //{
//    //    Vector3 temp = maxYPosition;
//    //    maxYPosition = minYPosition;
//    //    minYPosition = temp;
//    //}

//    //Vector2 vDir = maxYPosition - minYPosition;
//    //transform.Rotate(new Vector3(0, 0, Module.GetAngle(vDir, new Vector3(1, 0))));
//}