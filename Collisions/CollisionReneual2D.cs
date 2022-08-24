using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid2D // 직사각형
{
    static private int[] m_dx = { -1, -1, 1, 1 };
    static private int[] m_dy = { 1, -1, 1, -1 };

    private List<Vector3> m_Position;
    private Vector3? m_CenterPosition;

    public Solid2D()
    {
        m_Position = new List<Vector3>();
    }

    public Solid2D(Vector3 centerPosition) : this()
    {
        m_CenterPosition = centerPosition;
    }

    public Vector3? GetCenterPoint() { return m_CenterPosition; }

    public void Add(Vector3 vPosition)
    {
        if (m_Position.Count > 3) return;
        m_Position.Add(vPosition);
    }

    public void Sort()
    {
        m_Position.Sort((Vector3 a, Vector3 b) =>
        {
            if (a.x < b.x) return -1;
            else if (a.x > b.x) return 1;
            else
            {
                if (a.y < b.y) return 1;
                return -1;
            }
        });
    }

    public bool IsSolid2D()
    {
        if (m_Position.Count != 4) return false;

        Sort();

        float fFirst = Mathf.Abs(m_Position[0].x - m_Position[1].x);
        float fSecond = Mathf.Abs(m_Position[2].x - m_Position[3].x);

        if (fFirst < 0.0001f && fSecond < 0.0001f) return true;
        return false;       
    }

    public void SortSeq()
    {
        if (m_Position.Count != 4) return;
        Sort();

        if (m_Position[0].y < m_Position[1].y) Swap(m_Position, 0, 1);
        if (m_Position[2].y < m_Position[3].y) Swap(m_Position, 2, 3);
    }

    public bool ReneualSolid(float fRad)
    {
        if (m_Position.Count != 4) return false;
        SortSeq();
        m_Position[0] = new Vector3(m_Position[0].x - fRad, m_Position[0].y + fRad, m_Position[0].z);
        m_Position[1] = new Vector3(m_Position[1].x - fRad, m_Position[1].y - fRad, m_Position[1].z);
        m_Position[2] = new Vector3(m_Position[2].x + fRad, m_Position[2].y + fRad, m_Position[2].z);
        m_Position[3] = new Vector3(m_Position[3].x + fRad, m_Position[3].y - fRad, m_Position[3].z);
        return true;
    }

    public bool ReneualRowSolid(float fRad)
    {
        if (m_Position.Count != 4) return false;
        SortSeq();
        m_Position[0] = new Vector3(m_Position[0].x - fRad, m_Position[0].y, m_Position[0].z);
        m_Position[1] = new Vector3(m_Position[1].x - fRad, m_Position[1].y, m_Position[1].z);
        m_Position[2] = new Vector3(m_Position[2].x + fRad, m_Position[2].y, m_Position[2].z);
        m_Position[3] = new Vector3(m_Position[3].x + fRad, m_Position[3].y, m_Position[3].z);
        return true;
    }

    public bool ReneualColSolid(float fRad)
    {
        if (m_Position.Count != 4) return false;
        SortSeq();
        m_Position[0] = new Vector3(m_Position[0].x, m_Position[0].y + fRad, m_Position[0].z);
        m_Position[1] = new Vector3(m_Position[1].x, m_Position[1].y - fRad, m_Position[1].z);
        m_Position[2] = new Vector3(m_Position[2].x, m_Position[2].y + fRad, m_Position[2].z);
        m_Position[3] = new Vector3(m_Position[3].x, m_Position[3].y - fRad, m_Position[3].z);
        return true;
    }

    public void Swap(List<Vector3> list,int from ,int to)
    {
        Vector3 temp = list[from];
        list[from] = list[to];
        list[to] = temp;
    }

    public void PrintDebug()
    {
        for(int i = 0; i < m_Position.Count; i++)
        {
            Debug.Log("Position[ + " + i + "] : " + m_Position[i]);
        }
    }

    public Solid2D Copy()
    {
        Solid2D newSolid = new Solid2D();
        for(int i = 0; i < m_Position.Count; i++)
        {
            newSolid.Add(m_Position[i]);
        }
        return newSolid;
    }

    public bool IsInside(Vector3 vPosition)
    {
        return m_Position[0].x <= vPosition.x && vPosition.x <= m_Position[2].x && m_Position[1].y <= vPosition.y && vPosition.y <m_Position[0].y;
    }

    public bool IsInside(Vector3 vPosition , float fRad)
    {
        for(int i = 0; i < m_Position.Count; i++)
        {
            float fDis = Vector3.Distance(m_Position[i], vPosition);
            if (fDis <= fRad) return true;
        }
        return false;
    }

    public void SetEdgePoint(float fRad)
    {
        m_Position.Clear();
        if (m_CenterPosition == null) return;
        for(int i = 0; i < 4; i++)
        {

            Vector3 pos = m_CenterPosition.Value;
            m_Position.Add(new Vector3(pos.x + m_dx[i] * fRad, pos.y + m_dy[i] * fRad, pos.z));
        }
    }
}

static public class CollisionReneual2D
{
    public static bool IsCollisionSolid(Solid2D solid, Vector3 vPosition, float fRad)
    {
        Solid2D RowSolid = solid.Copy();
        Solid2D ColSolid = solid.Copy();

        RowSolid.ReneualRowSolid(fRad);
        ColSolid.ReneualColSolid(fRad);

        if (RowSolid.IsInside(vPosition)) return true;
        if (ColSolid.IsInside(vPosition)) return true;
        if (solid.IsInside(vPosition, fRad)) return true;
        return false;
    }
}
