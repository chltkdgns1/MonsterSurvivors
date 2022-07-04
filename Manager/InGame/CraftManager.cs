using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour, ITouchCraftManager  // ũ�����ÿ� ���õ� �͵鸸 ó����.
{
    static public CraftManager instance = null;

    private List<List<GameObject>> m_vObList = new List<List<GameObject>>();
    private List<List<GameObject>> m_vObjectPool = new List<List<GameObject>>();

    //private List<List<List<GameObject>>> m_vObRealList = new List<List<List<GameObject>>>(); // ���������� ������ ���� ������Ʈ

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
        //RegistTouchEvnet();
        if (PlayingGameManager.GetGameState() != DefineManager.GameState.PLAYING_STATE_CRAFTING)
            PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_CRAFTING);

        m_ParamData = param;

        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);

        m_vObList[nGroup][nType].SetActive(true);
    }


    public void EndCrafting()
    {
        //DeleteTouchEvent();
        int nGroup = int.Parse(m_ParamData[0]);
        int nType = int.Parse(m_ParamData[1]);
        m_ParamData = null;

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

    public void OnDrag(Vector3 firstTouch, Vector3 touchPoint)
    {
        Vector3 first = Camera.main.ScreenToWorldPoint(firstTouch);
        Vector3 second = Camera.main.ScreenToWorldPoint(touchPoint);
        CameraManager.instance.MoveCamera(first - second);

        //if (m_ParamData == null) return;

        //int nGroup = int.Parse(m_ParamData[0]);
        //int nType = int.Parse(m_ParamData[1]);

        //Vector3 temp = Camera.main.ScreenToWorldPoint(touchPoint);
        //m_vObList[nGroup][nType].transform.position = Module.GetGrid(new Vector3(temp.x, temp.y, -1));

    }

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
        m_vObList[nGroup][nType].transform.position = Module.GetGrid(new Vector3(temp.x, temp.y, -1));
    }

    public void OnZoom(Vector3 zoomPoint)
    {

    }

    public bool IsInsideRange(Vector3 vFirst, Vector3 vSecond)
    {
        Vector3 first = Module.GetGrid(Camera.main.ScreenToWorldPoint(vFirst));
        Vector3 second = Module.GetGrid(Camera.main.ScreenToWorldPoint(vSecond));

        if (Mathf.Abs(first.x - second.x) < 0.001f && Mathf.Abs(first.y - second.y) < 0.001f)
            return true;
        return false;
    }


}
