using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour, ITouchCraftManager// 실제 설치될 친구
{
    private DataManage.CraftStructure m_Structure;

    [SerializeField]
    protected int m_nType = 0;

    protected int m_nReinforceState = 0;

    public float Shield
    {
        get { return m_Structure.m_fShield; }
        set { m_Structure.m_fShield = value; }
    }
    public float Hp
    {
        get { return m_Structure.m_fHp; }
        set { m_Structure.m_fHp = value; }
    }
    public int Price
    {
        get { return m_Structure.m_nPrice; }
        set { m_Structure.m_nPrice = value; }
    }
    public int ReinforceState
    {
        get { return ReinforceState; }
        set { ReinforceState = value; }
    }
    public string sName
    {
        get { return m_Structure.m_sName; }
        set { m_Structure.m_sName = value; }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        m_Structure = DataManage.InitData.instance.GetCraftStructure(m_nType);
    }

    public void OnOneDrag(Vector3 touchPoint)
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(touchPoint);
        transform.position = Module.GetGrid(new Vector3(temp.x, temp.y, -1));
    }

    public void OnManyDrag(List<Vector3> touchPoint)
    {
        //Vector3 maxYPosition = touchPoint[0];
        //Vector3 minYPosition = touchPoint[1];
        //if (touchPoint[0].y < touchPoint[1].y)
        //{
        //    Vector3 temp = maxYPosition;
        //    maxYPosition = minYPosition;
        //    minYPosition = temp;
        //}

        //Vector2 vDir = maxYPosition - minYPosition;
        //transform.Rotate(new Vector3(0, 0, Module.GetAngle(vDir, new Vector3(1, 0))));
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
}
