using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingGameManager 
{

    static private Stack<DefineManager.GameState> m_stateStack = new Stack<DefineManager.GameState>();

    static private bool m_bOnLine = false;
    static private string m_sCharName = "";

    static public DefineManager.GameState GetGameState()
    {
        if (m_stateStack.Count == 0) return DefineManager.GameState.PLAYING_STATE_NOMAL;
        return m_stateStack.Peek();
    }

    static public void ClearGAmeState()
    {
        m_stateStack.Clear();
    }

    static public void SetGameState(DefineManager.GameState state)
    {
        m_stateStack.Push(state);
    }
    
    static public void SetOutState(DefineManager.GameState state)
    {
        if (m_stateStack.Count == 0)
        {
            Debug.LogError("State Stack Is Empty");
            return;
        }

        if (m_stateStack.Peek() != state)
        {
            Debug.LogError("SetOutState is Not Same");
            return;
        }

        m_stateStack.Pop();
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
