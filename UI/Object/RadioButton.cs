using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButton : MonoBehaviour, IGroup
{
    private int m_nGId = -1;
    private int m_nGIndex = -1;

    public GroupCallBack run
    {
        private get; set;
    }

    public GroupCallBack stop
    {
        private get; set;
    }

    public void Run()
    {
        run(gameObject, m_nGIndex);
    }

    public void Stop()
    {
        stop(gameObject, m_nGIndex);
    }

    public int GetGId()
    {
        return m_nGId;
    }

    public int GetGIndex()
    {
        return m_nGIndex;
    }

    public void SetGroupData(int nGId, int nGIndex)
    {
        m_nGId = nGId;
        m_nGIndex = nGIndex;
    }
}
