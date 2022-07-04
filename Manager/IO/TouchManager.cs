using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=pxkey&logNo=221312986925
// 참고

public class TouchCircle                        
{
    public Vector3 m_position;
    public float m_fRad;
    public int m_id;
    public int m_index;

    public TouchCircle(int id, int index, Vector3 position, float fRad)
    {
        m_id = id;
        m_index = index;
        m_position = position;
        m_fRad = fRad;
    }
}

public class TouchManager : MonoBehaviour
{

    static public TouchManager instance                 = null;
    private List<ITouchManagerEvent> m_touchList        = new List<ITouchManagerEvent>();
    private List<ITouchCraftManager> m_touchCraftList   = new List<ITouchCraftManager>();
  
    private int m_nFirstIndex = DefineManager.INIT;
    private Vector3 m_vFirstPosition;
    private Vector3? m_vFirstDragPosition = null;
 
    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        Init();
    }

 

    void Init()
    {
        m_nFirstIndex = DefineManager.INIT;
    }

    void Start()
    {

    }

    public void RegisterEvent(ITouchManagerEvent events)
    {
        m_touchList.Add(events);                        // 해당 터치 데이터에 한해서는 first Position 으로 잡지 않는다.
    }

    public void DeleteEvent(ITouchManagerEvent events)
    {
        for (int i = 0; i < m_touchCraftList.Count; i++)
        {
            if (m_touchCraftList[i] == events)
            {
                m_touchList.RemoveAt(i);
                return;
            }
        }
    }

    public void RegisterEvent(ITouchCraftManager events)
    {
        m_touchCraftList.Add(events);
    }

    public void DeleteEvent(ITouchCraftManager events)
    {
        for(int i = 0; i < m_touchCraftList.Count; i++)
        {
            if(m_touchCraftList[i] == events)
            {
                m_touchCraftList.RemoveAt(i);
                return;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        if (Input.touchCount > 0) DivideTouchType();
        
        //else
        //{
        //    if(m_nFirstIndex != DefineManager.INIT) SendFirstTouchEnd();
        //    m_nFirstIndex = DefineManager.INIT;
        //    //SendFirstTouchEnd();
        //}
    }

    void DivideTouchType()
    {
        int cnt = Input.touchCount;

        for (int i = 0; i < cnt; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId)) continue;

            switch (Input.GetTouch(i).phase)
            {
                case TouchPhase.Began       : TouchBegan(i);            break;                  // 터치 시작
                case TouchPhase.Moved       : TouchMove(i);             break;                  // 터치한 상태에서 움직임.
                case TouchPhase.Stationary  : TouchWait(i);             break;                  // 터치한 상태에서 움직이지 않음.
                case TouchPhase.Ended       : TouchEnd(i);              break;                  // 터치를 뗀 상태
                case TouchPhase.Canceled    : TouchEnd(i);              break;                  // 터치가 5개 이상 입력되어 추적을 취소함.
            }
        }
    }

    void SendFirstTouch()
    {
        if (m_nFirstIndex == DefineManager.INIT) return;

        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnFirstTouch(m_vFirstPosition);
    }

    void SendFirstMoveTouch()
    {
        if (m_nFirstIndex == DefineManager.INIT) return;

        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnFirstTouchDrag(m_vFirstPosition);
    }

    void SendFirstTouchEnd()
    {
        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnFirstTouchEnd();
    }

    void TouchBegan(int index)
    {
        if (m_nFirstIndex == DefineManager.INIT)
        {
            m_nFirstIndex = index;
            m_vFirstPosition = Input.GetTouch(index).position;

            OnCraftDoubleTouch(m_vFirstPosition);

            m_vFirstDragPosition = m_vFirstPosition;
            SendFirstTouch();
        }
    }


    void TouchMove(int index)
    {
        if (m_nFirstIndex == index)
        {
            m_vFirstPosition = Input.GetTouch(index).position;
            SendFirstMoveTouch();
            SetCraftTouch();
        }
        // 첫 번째 터치 빼고는 움직이고 있는 것을 거른다. 
    }

    void TouchWait(int index)
    {
       
    }

    void TouchEnd(int index)
    {
        if (m_nFirstIndex == index)
        {
            m_nFirstIndex = -1;
            m_vFirstDragPosition = m_vFirstPosition;
            //m_vFirstDragPosition = null;
            SendFirstTouchEnd();
        }
    }

    void SetCraftTouch()
    {
        for (int i = 0; i < m_touchCraftList.Count; i++)
            m_touchCraftList[i].OnDrag(m_vFirstDragPosition ?? m_vFirstPosition, m_vFirstPosition);
    }

    void OnCraftDoubleTouch(Vector3 vPosition)
    {
        for (int i = 0; i < m_touchCraftList.Count; i++)
        {
            if (m_touchCraftList[i].IsInsideRange(m_vFirstDragPosition ?? vPosition, vPosition))
            {
                m_touchCraftList[i].OnDoubleTouch(vPosition);
            }
        }
    }
}


