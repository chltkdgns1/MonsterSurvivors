using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingGameManager 
{
    static private int m_nGameSate = DefineManager.PLAYING_STATE_NOMAL;
    static private bool m_bOnLine = false;
    static private string m_sCharName = "";
    static private int m_nGameCntStopState = 0;

    static public int GetGameState()
    {
        return m_nGameSate;
    }

    static public void InitGameState()
    {
        m_nGameCntStopState = 0;
        m_nGameSate = DefineManager.PLAYING_STATE_NOMAL;
    }

    static public void SetGameState(int state)
    {
        if (state == DefineManager.PLAYING_STATE_NOMAL)
        {
            m_nGameCntStopState--;
            if(m_nGameCntStopState == 0) m_nGameSate = state;
            if(m_nGameCntStopState < 0)
                Debug.LogError("static public void SetGameState(int state) State Error : " + m_nGameCntStopState);      
        }
        else if(state == DefineManager.PLAYING_STATE_PAUSE)
        {
            m_nGameCntStopState++;
            m_nGameSate = state;
        }
        else
        {
            if (m_nGameCntStopState > 0) Debug.LogError("    static public void SetGameState(int state) No Enemy Error : " + m_nGameCntStopState);
            m_nGameSate = state;
        }
    }

    static public void SetOnLine(bool flag)
    {
        m_bOnLine = flag;
    }

    static public bool GetOnLine()
    {
        return m_bOnLine;
    }

    static public string GetCharName()
    {
        return m_sCharName;
    }

    static public void SetCharName(string name)
    {
        m_sCharName = name;
    }
}
