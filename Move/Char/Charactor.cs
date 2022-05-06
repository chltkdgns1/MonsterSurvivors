using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charactor : MonoBehaviour
{

    static public Charactor instance = null;

    private bool m_bDirection;
    private int m_nWidth;

    [SerializeField]
    private float m_fSpeed = 1f;

    [SerializeField]
    private float m_fMoveRangeGap;
    private float m_fLeftMaxRange;
    private float m_fRightMaxRange;

    private RectTransform m_rectTrans;
    private Vector3 m_vLeftPosition;
    private Vector3 m_vRightPosition;

    private Move2D m_Move2DComp;

    private Animator m_Anim;

    [SerializeField]
    private string m_sWalkAnim;
    private bool m_bWalk;

    [SerializeField]
    private GameObject m_canvas;

    [SerializeField]
    private GameObject m_ObText;
    private Text m_TextTalk;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
    }

    void InitAnim()
    {
        m_Anim = GetComponent<Animator>();
    }

    void InitAnimMember()
    {
        m_Anim.SetBool(m_sWalkAnim,     false);
    }

    void InitMember()
    {
        m_bDirection        = false;       // false : left , true : right

        m_vLeftPosition     = new Vector3(transform.position.x - m_fMoveRangeGap, transform.position.y);
        m_vRightPosition    = new Vector3(transform.position.x + m_fMoveRangeGap, transform.position.y);

        m_Move2DComp        = GetComponent<Move2D>();

        m_TextTalk          = m_ObText.GetComponent<Text>();

        CharState();
    }
  
    void CharState()
    {    
        m_bWalk     = false;
    }

    void Start()
    {
        InitMember();
        InitAnim();
        InitAnimMember();
        m_Anim.SetBool(m_sWalkAnim, true);
        Module.ChangeDirection(gameObject,m_bDirection);
        Module.ChangeDirection(m_ObText, m_bDirection);
        m_TextTalk.text = "ªÏ∑¡¡‡!";
    }

    void Update()
    {            
        MoveDirection();
    }

    void MoveDirection()
    {
        if (!m_Move2DComp) return;

        Vector3 destPosition;
        if (m_bDirection == false) destPosition = m_vLeftPosition;
        else destPosition = m_vRightPosition;

        if (!m_Move2DComp.Run(destPosition, 5f))
        {
            m_Move2DComp.SetAgoDistance(1e5f);
            m_bDirection = !m_bDirection;
            Module.ChangeDirection(gameObject , m_bDirection);
            Module.ChangeDirection(m_ObText, m_bDirection);
        }
    }
}
