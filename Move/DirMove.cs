using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirMove : MonoBehaviour
{
    private bool m_bDirection;
    private int m_nWidth;

    [SerializeField]
    private bool m_bDir;

    [SerializeField]
    private float m_fMoveRangeGap;
   
    private RectTransform m_rectTrans;
    private Vector3 m_vDirPositionA;
    private Vector3 m_vDirPositionB;

    private Move2D m_Move2DComp;

    [SerializeField]
    private GameObject m_canvas;

    private float m_fHeightRatio;

    private void Awake()
    {
        m_rectTrans = GetComponent<RectTransform>();
    }

    void InitMember()
    {
        m_bDirection = false;       // false : left , true : right

        m_rectTrans = GetComponent<RectTransform>();

        if (m_bDir == false)
        {
            m_vDirPositionA = new Vector3(transform.position.x - m_fMoveRangeGap, transform.position.y);
            m_vDirPositionB = new Vector3(transform.position.x + m_fMoveRangeGap, transform.position.y);
        }
        else
        {
            m_vDirPositionA = new Vector3(transform.position.x, transform.position.y - m_fMoveRangeGap);
            m_vDirPositionB = new Vector3(transform.position.x, transform.position.y + m_fMoveRangeGap);
        }

        m_Move2DComp = GetComponent<Move2D>();
    }



    void Start()
    {
        InitMember();
        Module.ChangeDirection(gameObject, m_bDirection);
    }

    void Update()
    {
        MoveDirection();
    }

    void MoveDirection()
    {
        if (!m_Move2DComp) return;

        Vector3 destPosition;
        if (m_bDirection == false)  destPosition = m_vDirPositionA;
        else                        destPosition = m_vDirPositionB;

        if (!m_Move2DComp.Run(destPosition, 1f, 5f))
        {
            m_Move2DComp.SetAgoDistance(1e5f);
            m_bDirection = !m_bDirection;

            if (m_bDir == false) Module.ChangeDirection(gameObject, m_bDirection);          
        }
    }
}
