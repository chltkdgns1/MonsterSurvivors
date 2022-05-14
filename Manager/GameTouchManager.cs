using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTouchManager : MonoBehaviour, ITouchManagerEvent, IMouseClickInterface
{
    // Start is called before the first frame update

    static public GameTouchManager instance = null;

    [SerializeField]
    private float m_fTouchPadRad = 64;

    [SerializeField]
    private float m_fTouchCircleRad = 32;

    [SerializeField]
    private bool m_bDimension = false;      // false 3d 
    private Vector3 m_vFirstPosition;

    private List<ITouchGameEvent> m_touchList = new List<ITouchGameEvent>();

    private bool m_bMouseUseState = false;

    public void RegisterEvent(ITouchGameEvent events)
    {
        m_touchList.Add(events);                        // 해당 터치 데이터에 한해서는 first Position 으로 잡지 않는다.
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        TouchManager.instance.RegisterEvent(this);
        MouseClickManager.instance.Register(this);
    }

    // Update is called once per frame
    public void OnFirstTouch(Vector3 touchPoint)
    {

        int nState = PlayingGameManager.GetGameState();

        if (nState == DefineManager.PLAYING_STATE_PAUSE) return;

        if (nState == DefineManager.PLAYING_STATE_BUILD_STRUCT) // 건물을 생성할 때는 조작이 조금 다름.
        {
            return;
        }

        m_vFirstPosition = touchPoint;
        GameUIManager.instance.SetActiveTouchPad(true);
        GameUIManager.instance.SetPositionTouchPad(touchPoint);
        GameUIManager.instance.SetPositionTouchCircle(touchPoint);
    }


    void SendTargetPosition(Vector3 position)
    {
        int sz = m_touchList.Count;

        Vector3 newPosition;

        if (m_bDimension == DefineManager.D3)   newPosition = new Vector3(position.x, 0f, position.y); // 3d
        else                                    newPosition = position;

        for(int i = 0; i < sz; i++) m_touchList[i].OnTargetPosition(newPosition);  
    }

    

    void SendStopPosition()
    {
        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnStop();
    }

    public void OnFirstTouchDrag(Vector3 touchPoint)
    {
        int nState = PlayingGameManager.GetGameState();

        if (nState == DefineManager.PLAYING_STATE_PAUSE) return;

        //Debug.LogError(">??드레그한다고?");
        // 서클 내부에 있는 원이 움직임. 


        if(nState == DefineManager.PLAYING_STATE_BUILD_STRUCT)
        {

            return;
        }

        float distance = Vector3.Distance(touchPoint, m_vFirstPosition);

        if (distance < m_fTouchPadRad)
        {
            GameUIManager.instance.SetPositionTouchCircle(touchPoint);
            SendTargetPosition(touchPoint - m_vFirstPosition);
        }
        else
        {
            float ratio = m_fTouchPadRad / distance;
            Vector3 nextPosition = m_vFirstPosition + (touchPoint - m_vFirstPosition) * ratio;
            SendTargetPosition(nextPosition - m_vFirstPosition);
            GameUIManager.instance.SetPositionTouchCircle(nextPosition);
        }
    }

    public void OnOtherTouch(List<TouchCircle> touchPoint)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnSkill(touchPoint);
    }

    public void OnFirstTouchEnd()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        if (m_bMouseUseState) return;
        GameUIManager.instance.SetActiveTouchPad(false);
        SendStopPosition();
    }

    public void OnClick(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        m_bMouseUseState = true;
        m_vFirstPosition = clickPosition;
        GameUIManager.instance.SetActiveTouchPad(true);
        GameUIManager.instance.SetPositionTouchPad(clickPosition);
        GameUIManager.instance.SetPositionTouchCircle(clickPosition);
    }

    public void OnClickMove(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        float distance = Vector3.Distance(clickPosition, m_vFirstPosition);

        if (distance < m_fTouchPadRad)
        {
            GameUIManager.instance.SetPositionTouchCircle(clickPosition);
            SendTargetPosition(clickPosition - m_vFirstPosition);
        }
        else
        {
            float ratio = m_fTouchPadRad / distance;
            Vector3 nextPosition = m_vFirstPosition + (clickPosition - m_vFirstPosition) * ratio;
            SendTargetPosition(nextPosition - m_vFirstPosition);
            GameUIManager.instance.SetPositionTouchCircle(nextPosition);
        }
    }

    public void OnClickUp(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        m_bMouseUseState = false;
        GameUIManager.instance.SetActiveTouchPad(false);
        SendStopPosition();
    }
}
