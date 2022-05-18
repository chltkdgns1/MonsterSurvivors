using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private List<Vector3> m_touchPositionList               = new List<Vector3>();
   
    private int m_nFirstIndex = DefineManager.INIT;
    private Vector3 m_vFirstPosition;

    private Vector3 m_touchSkill;
    private bool m_isNotEmptyTouchSkill;
    private List<TouchCircle> m_TouchCircleList = new List<TouchCircle>();

    [SerializeField]
    private float m_fMargin = 50f;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        Init();
    }

    public void AddCirclePoint(TouchCircle circleData)
    {
        m_TouchCircleList.Add(circleData);
    }

    void Init()
    {
        //cameraObject = GameObject.FindWithTag("MainCamera");
        m_isNotEmptyTouchSkill = false;
        m_nFirstIndex = DefineManager.INIT;
        //m_vectorList = new List<Vector3>();
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

        if (Input.touchCount > 0)                           // 터치가 입력되었을 경우
        {
            //Debug.Log("터치 들어오나요?");
            DivideTouchType();
        }
        else
        {
            m_nFirstIndex = DefineManager.INIT;
            SendFirstTouchEnd();
        }
    }

    void DivideTouchType()
    {
        int cnt = Input.touchCount;

        m_isNotEmptyTouchSkill = false;

        m_touchPositionList.Clear();

        for (int i = 0; i < cnt; i++)
        {
            switch (Input.GetTouch(i).phase)
            {
                case TouchPhase.Began       : TouchBegan(i);            break;                  // 터치 시작
                case TouchPhase.Moved       : TouchMove(i);             break;                  // 터치한 상태에서 움직임.
                case TouchPhase.Stationary  : TouchWait(i);             break;                  // 터치한 상태에서 움직이지 않음.
                case TouchPhase.Ended       : TouchEnd(i);              break;                  // 터치를 뗀 상태
                case TouchPhase.Canceled    : TouchEnd(i);              break;                  // 터치가 5개 이상 입력되어 추적을 취소함.
            }
        }
        SendOtherTouch();
        SetCraftTouch();
    }

    void SendOtherTouch()
    {
        if (m_isNotEmptyTouchSkill == false) return;
        int sz = m_touchList.Count;

        List<TouchCircle> touchCircleList = new List<TouchCircle>();

        for (int i = 0; i < m_TouchCircleList.Count; i++)
        {
            double dist = Vector3.Distance(m_touchSkill, m_TouchCircleList[i].m_position);

            if (dist <= m_TouchCircleList[i].m_fRad + m_fMargin)  // Margin
            {
                touchCircleList.Add(m_TouchCircleList[i]);
                break;
            }
        }

        if (touchCircleList.Count < 1) return;

        for (int i = 0; i < sz; i++) m_touchList[i].OnOtherTouch(touchCircleList);        
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

    bool IsOutCircle(Vector3 Position)
    {
        int sz = m_TouchCircleList.Count;
        for (int i = 0; i < sz; i++)
        {
            double dist = Vector3.Distance(m_TouchCircleList[i].m_position, Position);
            if (dist <= m_TouchCircleList[i].m_fRad + m_fMargin) return false;  
        }
        return true;
    }

    void TouchBegan(int index)
    {
        if (m_nFirstIndex == DefineManager.INIT)
        {
            if (IsOutCircle(Input.GetTouch(index).position) == true)        // 해당 스킬이나 접근하면 안되는 Circle 내부에 있는게 아니라면, first 터치로 인정함.
            {
                m_nFirstIndex = index;
                m_vFirstPosition = Input.GetTouch(index).position;
                SendFirstTouch();
            }
            else
            {
                m_isNotEmptyTouchSkill = true;
                m_touchSkill = Input.GetTouch(index).position;
            }
        }
        else
        {
            m_isNotEmptyTouchSkill = true;
            m_touchSkill = Input.GetTouch(index).position;
        }
    }

    void TouchMove(int index)
    {
        m_touchPositionList.Add(Input.GetTouch(index).position);

        if (m_nFirstIndex == index)
        {
            m_vFirstPosition = Input.GetTouch(index).position;
            SendFirstMoveTouch();
        }
            // 첫 번째 터치 빼고는 움직이고 있는 것을 거른다. 
    }

    void TouchWait(int index)
    {
        m_touchPositionList.Add(Input.GetTouch(index).position);
    }

    void TouchEnd(int index)
    {
        if (m_nFirstIndex == index)
        {
            m_nFirstIndex = -1;
            SendFirstTouchEnd();
        }
    }

    void SetCraftTouch()
    {
        if (m_touchPositionList.Count == 0) return;

        if(m_touchPositionList.Count == 1)
        {
            for(int i = 0; i < m_touchCraftList.Count; i++)
                m_touchCraftList[i].OnOneDrag(m_touchPositionList[0]);
            return;
        }

        for (int i = 0; i < m_touchCraftList.Count; i++)
            m_touchCraftList[i].OnManyDrag(m_touchPositionList);
    }
}


