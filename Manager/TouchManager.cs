using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=pxkey&logNo=221312986925
// ����

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
        m_touchList.Add(events);                        // �ش� ��ġ �����Ϳ� ���ؼ��� first Position ���� ���� �ʴ´�.
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

        if (Input.touchCount > 0)                           // ��ġ�� �ԷµǾ��� ���
        {
            //Debug.Log("��ġ ��������?");
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
                case TouchPhase.Began       : TouchBegan(i);            break;                  // ��ġ ����
                case TouchPhase.Moved       : TouchMove(i);             break;                  // ��ġ�� ���¿��� ������.
                case TouchPhase.Stationary  : TouchWait(i);             break;                  // ��ġ�� ���¿��� �������� ����.
                case TouchPhase.Ended       : TouchEnd(i);              break;                  // ��ġ�� �� ����
                case TouchPhase.Canceled    : TouchEnd(i);              break;                  // ��ġ�� 5�� �̻� �ԷµǾ� ������ �����.
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
            if (IsOutCircle(Input.GetTouch(index).position) == true)        // �ش� ��ų�̳� �����ϸ� �ȵǴ� Circle ���ο� �ִ°� �ƴ϶��, first ��ġ�� ������.
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
            // ù ��° ��ġ ����� �����̰� �ִ� ���� �Ÿ���. 
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


