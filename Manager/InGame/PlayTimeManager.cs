using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TimerCallBack();

public class PlayTimeManager : MonoBehaviour
{
    public  static PlayTimeManager instance = null;
    private float m_fTime;

    [SerializeField]
    private float m_fEndGameTime = 1800f;

    [SerializeField]
    private float m_fInitialTime = 300f;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
        m_fTime = 0f;
    }

    void Update()
    {
        if (DefineManager.GameState.PLAYING_STATE_NOMAL != PlayingGameManager.GetGameState()) return;
        m_fTime += Time.deltaTime;
    }

    public float GetTime() { return m_fTime; }

    public string GetTimeText()
    {
        int nTime = (int)m_fTime;
        int nMinute = nTime / 60;
        int nSecond = nTime % 60;
        return GetTimeNum(nMinute) + ":" + GetTimeNum(nSecond);
    }

    public string GetTimeNum(int num)
    {
        return num >= 10 ? num.ToString() : "0" + num;
    }

    public bool IsTimeEnd()
    {
        int nSecond = (int)m_fTime;
        if (nSecond >= m_fEndGameTime) return true;
        return false;
    }

    public bool IsInitialTimeEnd()
    {
        int nSecond = (int)m_fTime;
        if (nSecond >= m_fInitialTime) return true;
        return false;
    }

    public float GetInitialTime() { return m_fInitialTime; }
    public float GetEndGameTime() { return m_fEndGameTime; }

    public void SetTimer(float fTimeLimit, TimerCallBack func)
    {
        StartCoroutine(TimerRoutine(fTimeLimit, func));
    }

    IEnumerator TimerRoutine(float fTimeLimit, TimerCallBack func)
    {
        while(fTimeLimit > m_fTime) yield return null;
        func();
    }
}
