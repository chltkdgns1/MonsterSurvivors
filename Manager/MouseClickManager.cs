using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Register(IMouseClickInterface regist)
    {
        m_registList.Add(regist);
    }

    void SendClick(Vector3 clickPosition)
    {
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
    }
    // Update is called once per frame
    void Update()
    {
        bool bClick = Input.GetMouseButtonDown(0);
        bool bClickUp = Input.GetMouseButtonUp(0);
        bool bClickMove = Input.GetMouseButton(0);

        if (bClickUp == true)
            SendClickUp(Input.mousePosition);
        else if (bClick == true)
            SendClick(Input.mousePosition);
        else if (bClickMove == true)
            SendClickMove(Input.mousePosition);

    }
}
