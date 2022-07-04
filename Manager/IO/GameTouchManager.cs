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

    //private bool m_bMouseUseState = false;

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
        MouseClickManager.instance.RegisterEvent(this);
    }

    // Update is called once per frame
    public void OnFirstTouch(Vector3 touchPoint)
    {
        DefineManager.GameState nState = PlayingGameManager.GetGameState();
        if (nState == DefineManager.GameState.PLAYING_STATE_PAUSE)      return;
        if (nState == DefineManager.GameState.PLAYING_STATE_CRAFTING)   return;
       
        m_vFirstPosition = touchPoint;
        CreateTouchPad(touchPoint);
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
        DefineManager.GameState nState = PlayingGameManager.GetGameState();

        if (nState == DefineManager.GameState.PLAYING_STATE_PAUSE)      return;
        if (nState == DefineManager.GameState.PLAYING_STATE_CRAFTING)   return;
        
        MoveTouchPad(touchPoint);
    }

    public void OnOtherTouch(List<TouchCircle> touchPoint)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE) return;

        int sz = m_touchList.Count;
        for (int i = 0; i < sz; i++) m_touchList[i].OnSkill(touchPoint);
    }

    public void OnFirstTouchEnd()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE) return;

        //if (m_bMouseUseState) return;
        GameUIManager.instance.SetActiveTouchPad(false);
        SendStopPosition();
    }

    public void OnClick(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE)       return;
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_CRAFTING)    return;

        //m_bMouseUseState = true;
        m_vFirstPosition = clickPosition;
        CreateTouchPad(clickPosition);
    }

    public void OnClickMove(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE) return;
        MoveTouchPad(clickPosition);
    }

    public void OnClickUp(Vector3 clickPosition)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE) return;

        //m_bMouseUseState = false;
        GameUIManager.instance.SetActiveTouchPad(false);
        SendStopPosition();
    }

    void CreateTouchPad(Vector3 vPosition)
    {
        GameUIManager.instance.SetActiveTouchPad(true);
        GameUIManager.instance.SetPositionTouchPad(vPosition);
        GameUIManager.instance.SetPositionTouchCircle(vPosition);
    }

    void MoveTouchPad(Vector3 vPosition)
    {
        float distance = Vector3.Distance(vPosition, m_vFirstPosition);

        if (distance < m_fTouchPadRad)
        {
            GameUIManager.instance.SetPositionTouchCircle(vPosition);
            SendTargetPosition(vPosition - m_vFirstPosition);
        }
        else
        {
            float ratio = m_fTouchPadRad / distance;
            Vector3 nextPosition = m_vFirstPosition + (vPosition - m_vFirstPosition) * ratio;
            SendTargetPosition(nextPosition - m_vFirstPosition);
            GameUIManager.instance.SetPositionTouchCircle(nextPosition);
        }
    }
}
