using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct CraftData
{
    public int m_nGroup;
    public int m_nType;
    public int m_nIndex;

    public CraftData(int nGroup, int nType, int nIndex)
    {
        m_nGroup = nGroup;
        m_nType = nType;
        m_nIndex = nIndex;
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
        if (m_CraftElement.Count <= UseCount) // �߰������� ������ �ʿ�.
        {
            bool bFlag = Sort(); // ������ , true : ������ ��, ������Ʈ ���� ���� false : �߰����� ���� ���� �ʿ�.
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

class CraftType // ������Ʈ�� ����
{
    public List<CraftObject> m_CraftObject = new List<CraftObject>();
}

public class CraftManager : MonoBehaviour, ITouchCraftManager  // ũ�����ÿ� ���õ� �͵鸸 ó����.
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

    void AwakeInit() // Awake ������ �ʱ�ȭ
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

    void EnableInit() // Enable �Ǿ��� ���� �ʱ�ȭ
    {

    }

    void StartInit() // Start ������ �ʱ�ȭ
    {
      
    }

    void ReInit() // �ڵ� ���� �߿� �ʱ�ȭ �ʿ��� ���
    {

    }

    public void OnOneTouch(Vector3 touchPoint)
    {
        // �����ϴ� Ÿ���� �����Ѵ�.
        Vector3Int vGridPosition = Module.GetGridInt(Camera.main.ScreenToWorldPoint(touchPoint));
        long key = vGridPosition.x * 10000 + vGridPosition.y;

        if (m_TemporaryDic.ContainsKey(key))
        {        
            CraftData temp = m_TemporaryDic[key];
            Debug.Log("���� : " + key);
            Debug.Log("temp : " + temp.m_nGroup + " " + temp.m_nType + "  " + temp.m_nIndex);
            m_CraftObjectPool[temp.m_nGroup].m_CraftObject[temp.m_nType].SetUnActive(temp.m_nIndex);
            m_CraftObjectPool[temp.m_nGroup].m_CraftObject[temp.m_nType].m_CraftElement[temp.m_nIndex].m_lkey = -1;
            m_TemporaryDic.Remove(key);
            return;
        }

        //if (m_DicMap.ContainsKey(key))
        //{

        //}
    }

    public void OnDrag(Vector3 firstTouch, Vector3 touchPoint)
    {
        Vector3 first = Camera.main.ScreenToWorldPoint(firstTouch);
        Vector3 second = Camera.main.ScreenToWorldPoint(touchPoint);
        CameraManager.instance.MoveCamera(first - second);
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

        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        Vector3 temp = Camera.main.ScreenToWorldPoint(doubleTouchPoint);

        Vector3Int vGridPosition = Module.GetGridInt(temp);
        long key = vGridPosition.x * 10000 + vGridPosition.y;

        if (m_DicMap.ContainsKey(key) || m_TemporaryDic.ContainsKey(key)) return;

        temp = new Vector3(temp.x, temp.y, -1);
        int index = m_CraftObjectPool[nGroup].m_CraftObject[nType].SetActive(Module.GetGrid(temp), key);
        m_TemporaryDic.Add(key, new CraftData(nGroup, nType, index));
    }

    public void OnZoom(float fWheel)
    {
        Camera.main.orthographicSize -= fWheel;
        // �۾������� Ȯ��� 
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