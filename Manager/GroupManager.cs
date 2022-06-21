using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GroupCallBack(params object []ob);

public interface IGroup
{
    void Run();
    void Stop();
    int GetGId();
    int GetGIndex();
    void SetGroupData(int nGId, int nGIndex);
}

public class GroupManager : MonoBehaviour
{
    public static GroupManager instance = null;
    private List<List<IGroup>> m_groupList = new List<List<IGroup>>();

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
    }

    public int RegisterGroup(GameObject[] objects)
    {
        int len = objects.Length;

        List<IGroup> list = new List<IGroup>();
        int gid = m_groupList.Count;

        for (int i = 0; i < len; i++)
        {
            IGroup temp = objects[i].GetComponent<IGroup>();
            list.Add(temp);
            list[i].SetGroupData(gid, i);
        }

        m_groupList.Add(list);
        return m_groupList.Count - 1;
    }

    public void GroupAction(int nGId, int nGIndex)
    {
        int len = m_groupList[nGId].Count;
        for (int i = 0; i < len; i++)
        {
            if (i == nGIndex)   m_groupList[nGId][i].Run();
            else                m_groupList[nGId][i].Stop();
        }
    }
}
