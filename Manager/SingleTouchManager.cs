using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SingleTouchInterface
{
    void OnTouch(Vector3 touchPoint);
    void OnTouchMove(Vector3 touchPoint);
    void OnTouchUp(Vector3 touchPoint);  
}

public class SingleTouchManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public SingleTouchManager instance = null;
    private List<SingleTouchInterface> m_registList = new List<SingleTouchInterface>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    public void Register(SingleTouchInterface regist)
    {
        m_registList.Add(regist);
    }

    void SendTouch(Vector3 touchPosition)
    {
        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnTouch(touchPosition);
        }
    }

    void SendTouchUp(Vector3 touchPosition)
    {
        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnTouchUp(touchPosition);
        }
    }

    void SendTouchMove(Vector3 touchPosition)
    {
        int sz = m_registList.Count;
        for (int i = 0; i < sz; i++)
        {
            m_registList[i].OnTouchMove(touchPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)         SendTouch(touch.position);
        else if (touch.phase == TouchPhase.Ended)   SendTouchUp(touch.position);
        else if (touch.phase == TouchPhase.Moved)   SendTouchMove(touch.position);
    }
}
