using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public interface IMouseClickInterface
{
    void OnClick(Vector3 clickPosition);

    void OnClickMove(Vector3 touchPoint);
    void OnClickUp(Vector3 clickPosition);
}

public class MouseClickManager : MonoBehaviour
{
    static public MouseClickManager instance = null;
    private List<IMouseClickInterface> m_registList = new List<IMouseClickInterface>();
    private List<ITouchCraftManager> m_MouseCraftList = new List<ITouchCraftManager>();

    private Vector3? m_vFirstClick = null;
    private Vector3? m_vDoubleClick = null;

    private float m_fDoubleClickTime;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_fDoubleClickTime = DefineManager.DOUBLE_CLICK_TILE;
    }

    public void RegisterEvent(ITouchCraftManager events)
    {
        m_MouseCraftList.Add(events);
    }

    public void DeleteEvent(ITouchCraftManager events)
    {
        for (int i = 0; i < m_MouseCraftList.Count; i++)
        {
            if (m_MouseCraftList[i] == events)
            {
                m_MouseCraftList.RemoveAt(i);
                return;
            }
        }
    }

    public void RegisterEvent(IMouseClickInterface regist)
    {
        m_registList.Add(regist);
    }

    void SendClick(Vector3 clickPosition)
    {
        OnCraftOneClick(clickPosition);
        OnCraftDoubleClick(clickPosition);

        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnClick(clickPosition);
        }
    }

    void SendClickUp(Vector3 clickPosition)
    {
        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnClickUp(clickPosition);
        }
    }

    void SendClickMove(Vector3 clickPosition)
    {
        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnClickMove(clickPosition);
        }

        sz = m_MouseCraftList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_MouseCraftList[i].OnDrag(m_vFirstClick ?? clickPosition, clickPosition);
        }
    }
    // Update is called once per frame
    void Update()
    {
        InputMouse();
        InputMouseWheel();
    }

    void InputMouseWheel()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput > 0f || wheelInput < 0f)
        {
            int sz = m_MouseCraftList.Count;
            for (int i = 0; i < sz; i++)
            {
                m_MouseCraftList[i].OnZoom(wheelInput);
            }
        }
    }

    void InputMouse()
    {
        bool bClick = Input.GetMouseButtonDown(0);
        bool bClickUp = Input.GetMouseButtonUp(0);
        bool bClickMove = Input.GetMouseButton(0);

        if (m_vDoubleClick != null)
        {
            m_fDoubleClickTime -= Time.deltaTime;
            if (m_fDoubleClickTime <= 0f)
            {
                m_fDoubleClickTime = DefineManager.DOUBLE_CLICK_TILE;
                m_vDoubleClick = null;
            }
        }

        if (bClickUp == true)
        {
            m_vFirstClick = Input.mousePosition; // 마우스를 뗐을 위치를 더블클릭의 첫번째 위치로 설정
            SendClickUp(Input.mousePosition);
        }
        else if (bClick == true)
        {
            if (EventSystem.current.IsPointerOverGameObject() == true) return;
            SendClick(Input.mousePosition);
            m_vFirstClick = Input.mousePosition; // 마우스 첫 터치를 더블클릭의 첫번째 위치로 선정
        }
        else if (bClickMove == true)
        {
            if (EventSystem.current.IsPointerOverGameObject() == true) return;
            SendClickMove(Input.mousePosition);
        }
    }

    void OnCraftDoubleClick(Vector3 vPosition)
    {
        if (m_vDoubleClick == null)
        {
            m_fDoubleClickTime = DefineManager.DOUBLE_CLICK_TILE;
            m_vDoubleClick = vPosition;
            return;
        }

        int mSize = m_MouseCraftList.Count;
        for (int i = 0; i < mSize; i++)
        {
            if (m_MouseCraftList[i].IsInsideRange(m_vDoubleClick.Value, vPosition))
            {
                m_MouseCraftList[i].OnDoubleTouch(vPosition);
            }
        }

        m_fDoubleClickTime = DefineManager.DOUBLE_CLICK_TILE;
        m_vDoubleClick = null;
    }

    void OnCraftOneClick(Vector3 vPosition)
    {
        int mSize = m_MouseCraftList.Count;
        for (int i = 0; i < mSize; i++)
        {
            m_MouseCraftList[i].OnOneTouch(vPosition);          
        }
    }

    void OnCraftZoom()
    {

    }
}
