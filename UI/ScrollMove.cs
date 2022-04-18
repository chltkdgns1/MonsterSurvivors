using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private int m_nScrollType;

    [SerializeField]
    private int m_nMoveCount;

    [SerializeField]
    private float m_fScrollSlide;

    [SerializeField]
    private string m_bNeedStandartPositionTag;

    [SerializeField]
    private GameObject m_obSizeObject = null;

    private Vector3 m_vNowPositon;
    private Vector3 m_vStartPosition;
    private Vector3 m_vEndPosition;

    private float m_fWidth;
    private float m_fHeight;

    private float m_canvasWidth;
    private float m_cnavasHeight;

    private RectTransform m_RectTransform;

    private GameObject m_obEnd;
    private float m_fHalfSize;
    private float m_fCanvasHalfSize;
    private float m_fLength;

    private float m_fRatioWidth;
    private float m_fRatioHeight;

    private float m_fObSizeHeight;

    void Start()
    {
        //Debug.Log("드래그함?");
        Init();
    }

    void Init()
    {
        m_RectTransform = GetComponent<RectTransform>();

        m_fHeight = (transform.position.y * 2);
        m_fWidth  = (transform.position.x * 2);

        m_canvasWidth = m_RectTransform.rect.width;
        m_cnavasHeight = m_RectTransform.rect.height;

        m_fRatioWidth =   m_canvasWidth / (transform.position.x * 2);
        m_fRatioHeight =  m_cnavasHeight / (transform.position.y * 2);

        m_obEnd = GameObject.FindGameObjectWithTag(m_bNeedStandartPositionTag);

        InitEndPosition();


        m_fObSizeHeight = 0;

        if(m_obSizeObject != null) m_fObSizeHeight = m_obSizeObject.GetComponent<RectTransform>().rect.height;
    }

    Vector3 TransVectorPosition(Vector3 position)
    {
        return new Vector3(position.x * m_fRatioWidth, position.y * m_fRatioHeight);
    }

    void InitEndPosition()
    {
        m_obEnd = GameObject.FindGameObjectWithTag(m_bNeedStandartPositionTag);
        RectTransform rect1 = m_obEnd.GetComponent<RectTransform>();
        RectTransform rect2 = GetComponent<RectTransform>();

        if (m_nScrollType == DefineManager.SCROLL_VERTICLE)
        {
            m_fHalfSize = rect1.rect.height / 2.0f / m_fRatioHeight;
            m_fCanvasHalfSize = rect2.rect.height / 2.0f;
            m_fLength = transform.position.y - m_obEnd.transform.position.y;
        }
        else
        {
            m_fHalfSize = rect1.rect.width / 2.0f / m_fRatioWidth;
            m_fCanvasHalfSize = rect2.rect.width / 2.0f;
            m_fLength = m_obEnd.transform.position.x - transform.position.x;
        }
    }
    Vector3 GetTransformPosition()  // 이동하거나 옮길때는 이 함수를 안쓰는게 좋음. 다만, 유니티 에디터의 xy 좌표를 비교하기 위해서 사용
    {
        return transform.position - new Vector3(m_fWidth / 2, m_fHeight / 2);
    }

    Vector3 GetTransPosition(Vector3 position)
    {
        return position - new Vector3(m_fWidth / 2, m_fHeight / 2); 
    }

    float GetTopPosition(Vector3 position)
    {
        if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return position.y + m_fHeight / 2;
        else                                                return position.x - m_fWidth  / 2;
    }

    float GetBottomPosition(Vector3 position)
    {
        if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return position.y - (m_fLength + m_fHalfSize + m_fObSizeHeight / m_fRatioHeight);
        else                                                return position.x + (m_fLength + m_fHalfSize);
    }

    float GetCanvasTopPosition()
    {
        if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return m_fHeight;       // 상하 스크롤일 때는 m_fHeight 일때가 제일 높은 곳
        else                                                return 0f; // 맨 왼쪽이 제일 높은 곳
    }

    float GetCanvasBottomPosition()
    {
        if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return 0f;       // 상하 스크롤일 때는 m_fHeight 일때가 제일 높은 곳
        else                                                return m_fWidth; // 맨 왼쪽이 제일 높은 곳
    }

    void SetScrollPosition(ref Vector3 from, ref Vector3 to)
    {
        if (m_nScrollType == DefineManager.SCROLL_VERTICLE)
        {
            to = new Vector3(transform.position.x, from.y, transform.position.z);
        }
        else if (m_nScrollType == DefineManager.SCROLL_HORIZON)
        {
            to = new Vector3(from.x, transform.position.y, transform.position.z);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mousePoint = eventData.position;

        m_vNowPositon = mousePoint;
        SetScrollPosition(ref mousePoint, ref m_vStartPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePoint = eventData.position;

        Vector3 vMoveDir = mousePoint - m_vNowPositon;

        if (m_nScrollType == DefineManager.SCROLL_VERTICLE)
        {
            transform.position = transform.position + new Vector3(0, vMoveDir.y, 0);
            m_vNowPositon = mousePoint;
        }

        else if (m_nScrollType == DefineManager.SCROLL_HORIZON)
        {
            transform.position = transform.position + new Vector3(vMoveDir.x, 0, 0);
            m_vNowPositon = mousePoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 mousePoint = eventData.position;

        SetScrollPosition(ref mousePoint, ref m_vEndPosition);

        //Debug.Log("trasn position : " + transform.position.x + " " + transform.position.y);

        float dist = 1280 - transform.position.y;

        //Debug.Log("dist : " + dist );
        //Debug.Log("dist : " + dist * m_fRatioWidth);

        StartCoroutine(MoveSlide());
    }

    Vector2 GetInSizePosition(int type,Vector3 position)
    {
        //position = TransVectorPosition(position);

        float fMove;
        if (type == DefineManager.TOP_OUT_POSITION)
        {
            fMove = GetCanvasTopPosition() - GetTopPosition(position);
            if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return new Vector3(0, fMove);
            else return new Vector2(fMove, 0);

            // 절대값을 취해준다.
            // Verscroll 의 경우 빼줘야한다. top 포지션에 한해서
            // 횡스크롤은 추가하여 처리 귀찮.
        }
        else if (type == DefineManager.BOTTOM_OUT_POSITION)
        {

            fMove = GetCanvasBottomPosition() - GetBottomPosition(position);
            if (m_nScrollType == DefineManager.SCROLL_VERTICLE) return new Vector3(0, fMove);
            else return new Vector2(fMove, 0);

            // 절대값을 취해준다.
            // Verscroll 의 경우 더해줘야함. top 포지션에 한해서
            // 횡스크롤은 추가하여 처리 귀찮.
        }
        return new Vector2(0, 0);
    }

    public IEnumerator MoveInSidePosition(int flag)
    {
        if (flag == DefineManager.INSIDE_POSITION) yield break;

        //Debug.Log("m_RectTransform.transform.position : " + m_RectTransform.transform.position.x + " " + m_RectTransform.transform.position.y);

        Vector3 position = transform.position;

       // Debug.Log("Position : " + position.x + " " + position.y);

        Vector2 offSet = GetInSizePosition(flag, position);

        offSet = new Vector2(offSet.x * m_fRatioWidth, offSet.y * m_fRatioHeight);

       // Debug.Log("offSet : " + offSet.x + " " + offSet.y);

        offSet /= m_nMoveCount;

        for (int i = 0; i < m_nMoveCount; i++)
        {
            m_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition + offSet;
            yield return null;
        }
    }

    public IEnumerator MoveSlide()
    {
        Vector3 vDir = (m_vEndPosition - m_vStartPosition) * m_fScrollSlide;

        Vector3 expPosition = vDir + transform.position;

        int flag = CheckOutSidePosition(expPosition);


        if (flag != DefineManager.INSIDE_POSITION)
        {
            StartCoroutine(MoveInSidePosition(flag));
            yield break;
        }

        vDir /= m_nMoveCount;

        for (int i = 0; i < m_nMoveCount; i++)
        {
            transform.position = transform.position + vDir;
            yield return null;
        }

        flag = CheckOutSidePosition(transform.position);

        StartCoroutine(MoveInSidePosition(flag));
    }

    int CheckOutSidePosition(Vector3 position) // 아웃일 경우 true , 아닐 경우 false
    {
        //position = TransVectorPosition(position);

        //Debug.Log("Check Position : " + position.x + " " +  position.y);

        if (m_nScrollType == DefineManager.SCROLL_VERTICLE)
        {
            //Debug.Log("GetTopPosition(position) : " + GetTopPosition(position));
            //Debug.Log("GetCanvasTopPosition() : " + GetCanvasTopPosition());
            if (GetTopPosition(position) < GetCanvasTopPosition())          return DefineManager.TOP_OUT_POSITION;
            if (GetBottomPosition(position) > GetCanvasBottomPosition())    return DefineManager.BOTTOM_OUT_POSITION;
        }
        else
        {
            if (GetTopPosition(position) > GetCanvasTopPosition())          return DefineManager.TOP_OUT_POSITION;
            if (GetBottomPosition(position) < GetCanvasBottomPosition())    return DefineManager.BOTTOM_OUT_POSITION;
        }
        return DefineManager.INSIDE_POSITION;
    }
}
