using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAboutRange : MonoBehaviour
{
    private float[] m_fRange;
    private Vector3 m_vDir;

    private ScoreInterface m_ScoreInter;

    [SerializeField]
    private float m_fSpeed;

    public void MoveStart(float []range, Vector3 Direction, float speed, ScoreInterface scoreInter = null)
    {
        m_vDir = Direction;

        int sz = range.Length;
        m_fRange = new float[sz];

        for(int i = 0; i < sz; i++) m_fRange[i] = range[i];

        if (speed > 0) m_fSpeed = speed;

        m_ScoreInter = scoreInter;

        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (IsInRangePosition())
        {
            transform.position = MovePosition();
            yield return null;
        }

        if(m_ScoreInter != null) m_ScoreInter.ScoreChangeInteger(1, 1);
        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("joystics"))
        {
            if(m_ScoreInter != null) m_ScoreInter.ScoreChangeInteger(0, 1);
            Destroy(gameObject);
        }  
    }

    Vector3 MovePosition()
    {
        return transform.position + m_vDir * m_fSpeed;
    }

    bool IsInRangePosition()
    {
        float fXpos = transform.position.x;
        float fYpos = transform.position.y;
        if (fXpos < m_fRange[0]) return false;
        if (fXpos > m_fRange[1]) return false;
        if (fYpos < m_fRange[2]) return false;
        if (fYpos > m_fRange[3]) return false;
        return true;
    }
}
