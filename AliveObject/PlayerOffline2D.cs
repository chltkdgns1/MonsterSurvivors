using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// ��Ģ
/// �ϴ� ������ �����ϰ� � �׼��� �߻��ϸ�
/// IsMove �Լ��� �ش� bool ���� üũ�Ѵ�.
/// Update �Լ� ���ο��� �ش� ����� ���� �� ��ġ �Է��� ���� ���� �ؼ��Ѵ�
/// Ex)m_targetPosition = transform.position;
/// </summary>

public class PlayerOffline2D : MonoBehaviour, TouchGameEvent
{
    static public PlayerOffline2D instance = null;

    private Vector3 m_targetPosition;     // ������ ��ġ ������
    private Vector3 m_Direction;

    private Move2D      m_obMove;
    private Animator    m_obAnim;

    //private UseSkill    m_CompUseSkill;

    [SerializeField]
    private float m_fSpeed;

    /// <summary>
    /// �ִϸ��̼� �ɼ�
    /// </summary>
    [SerializeField]
    private string m_sMoveAniOption;
    private string m_sBackMoveAniOption;

    [SerializeField]
    private string m_sDash;
    private string m_sBackDash;

    [SerializeField]
    private string m_sDamageAniOption;
    private string m_sBackDamageAniOption;

    [SerializeField]
    private string m_sDeadAniOption;
    private string m_sBackDeadAniOption;

    [SerializeField]
    private string m_sAttackAniOption;
    private string m_sBackAttackAniOption;

    [SerializeField]
    private string m_sSkillAniOption;
    private string m_sBackSkillAniOption;

    [SerializeField]
    private string m_sDeadObject;

    [SerializeField]
    private float m_nMaxHp            ;
    private float m_nRemainHp         ;

    private int m_nHpKey            = -1;

    private bool m_bCharLeftRight   = false;      // false Left , true Right
    private bool m_bDownUp          = false;             // false Down,  true Up   

    private bool m_bIsDead          = false;                               
    private bool m_bClicked         = false;                            
    private bool m_bRecieveDamage   = false;

    private bool  m_bDeadWait;
    private float m_fDamageMotionTime;
    private float m_fDeadMotionTime;
    private float m_fNoEnemyTime;

    private int m_nLevel;
    private int m_nEx;

    private bool m_bFirst;

    Renderer m_Renderer;

    private GameObject m_ObNearestObject;
    private float m_fDistance;

    private long m_lDamageTime;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        FirstStartInit();
        SetBool();
        initValue();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void FirstStartInit()
    {
        m_nLevel    = 1;
        m_nEx       = 0;

        m_ObNearestObject   = null;
        m_fDistance         = 1e5f;

        m_Direction = new Vector3(0, 0, 0);
        m_obMove    = GetComponent<Move2D>();
        m_obAnim    = GetComponent<Animator>();

        string back = "Back";

        //Debug.Log("m_sAttackAniOption : " + m_sAttackAniOption);

        m_sBackDamageAniOption  = back + m_sDamageAniOption;
        m_sBackDash             = back + m_sDash;
        m_sBackDeadAniOption    = back + m_sDeadAniOption;
        m_sBackMoveAniOption    = back + m_sMoveAniOption;
        m_sBackAttackAniOption  = back + m_sAttackAniOption;
        m_sBackSkillAniOption   = back + m_sSkillAniOption;
        m_Renderer              = GetComponent<Renderer>();
    }

    void initValue()
    {
        m_fDamageMotionTime         = 0f;
        m_fDeadMotionTime           = 0f;
        m_fNoEnemyTime              = 0f;

        m_nMaxHp                    = 100;
        m_nRemainHp                 = 100;

        m_bFirst                    = false;
        m_Renderer.material.color   = Color.white;

        Color temp                  = m_Renderer.material.color;
        temp.a                      = 1f;
        m_Renderer.material.color   = temp;

        SetStateReset();
    }

    void SetStateReset()
    {
        SetBool();

        m_obAnim.SetBool(m_sMoveAniOption,          false);
        m_obAnim.SetBool(m_sBackMoveAniOption,      false);
        m_obAnim.SetBool(m_sDash,                   false);
        m_obAnim.SetBool(m_sBackDash,               false);
        m_obAnim.SetBool(m_sDamageAniOption,        false);
        m_obAnim.SetBool(m_sBackDamageAniOption,    false);
        m_obAnim.SetBool(m_sDeadAniOption,          false);
        m_obAnim.SetBool(m_sBackDeadAniOption,      false);
        m_obAnim.SetBool(m_sAttackAniOption,        false);
        m_obAnim.SetBool(m_sBackAttackAniOption,    false);
    }

    public void SetNearestObject(GameObject ob, float fDistance, bool flag)
    {
        if(ob == m_ObNearestObject && flag)
        {
            //Debug.Log("�����鼭 �ʱ�ȭ��?");
            m_fDistance = 1e6f;
            m_ObNearestObject = null;
            return;
        }

        if (m_ObNearestObject == null || m_ObNearestObject.activeSelf == false)
        {
            m_fDistance = 1e6f;
            m_ObNearestObject = null;
        }
        
        if (fDistance < m_fDistance)
        {
            //Debug.Log("�Ÿ� ���Ӱ� ���� : " + fDistance);
            m_fDistance = fDistance;
            m_ObNearestObject = ob;
        }
    }

    void SetBool()
    {
        m_bClicked          = false;
        m_bIsDead           = false;
        m_bRecieveDamage    = false;
        m_bDeadWait         = false;
    }

    public void SetAnimDamage(bool flag)
    {
        //if (m_bDownUp == false) m_obAnim.SetBool(m_sBackDamageAniOption,    flag);
        //else                    m_obAnim.SetBool(m_sDamageAniOption,        flag);
        m_obAnim.SetBool(m_sDamageAniOption, flag);
    }

    public void SetAnimDead(bool flag)
    {
        //if (m_bDownUp == false) m_obAnim.SetBool(m_sBackDeadAniOption,      flag);
        //else                    m_obAnim.SetBool(m_sDeadAniOption,          flag);
        m_obAnim.SetBool(m_sDeadAniOption, flag);
    }

    public void SetAnimAttack(bool flag)
    {
        if (m_bDownUp == false) m_obAnim.SetBool(m_sBackAttackAniOption,    flag);
        else                    m_obAnim.SetBool(m_sAttackAniOption,        flag);
    }

    public void SetAnimSkill(bool flag)
    {
        if (m_bDownUp == false) m_obAnim.SetBool(m_sBackSkillAniOption,     flag);
        else                    m_obAnim.SetBool(m_sSkillAniOption,         flag);
    }

    public void SetClicked(Vector3 targetPostion, bool clicked)
    {
        SetDirect(targetPostion);
        m_obMove.SetAgoDistance(1e5f);
        this.m_targetPosition = targetPostion;
        this.m_bClicked = clicked;
    }

    void SetCollisionState()
    {
        m_bClicked = false;
        m_targetPosition = transform.position;

        m_obAnim.SetBool(m_sMoveAniOption,      false);
        m_obAnim.SetBool(m_sBackMoveAniOption,  false);
        m_obAnim.SetBool(m_sDash,               false);
        m_obAnim.SetBool(m_sBackDash,           false);

        HpBar.instance.ChangePosition(m_nHpKey, transform.position);
    }

    void SetGameOver()
    {
        gameObject.SetActive(false);
        initValue();
        HpBar.instance.ChangeActive(m_nHpKey, false);
    }

    public void SetRestart()
    {
        GameUIManager.instance.SetRestart(0);
        GameUIManager.instance.PrintDeadQuestion(false);
        gameObject.SetActive(true);

        initValue();

        HpBar.instance.ChangeActive(m_nHpKey, true);
        HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);
        m_targetPosition = transform.position;
        HpBar.instance.ChangePosition(m_nHpKey, transform.position);
        GameUIManager.instance.RestartGameTimer();

        m_Renderer.material.color   = Color.black;
        Color temp                  = m_Renderer.material.color;
        temp.a                      = 0.4f;
        m_Renderer.material.color   = temp;
    }

    void SetDirect(Vector3 targetPosition)
    {
        SetUpDown(targetPosition);
        SetLeftRight(targetPosition);
    }

    void SetUpDown(Vector3 targetPosition)
    {
        if (transform.position.y == targetPosition.y) return;
        m_obAnim.SetBool(m_sMoveAniOption, true);

        //if (transform.position.y == targetPosition.y) return;
        //else if (transform.position.y < targetPosition.y)
        //{
        //    m_obAnim.SetBool(m_sMoveAniOption, false);
        //    m_obAnim.SetBool(m_sBackMoveAniOption, true);
        //    m_bDownUp = false;              // ����
        //}
        //else
        //{
        //    m_obAnim.SetBool(m_sBackMoveAniOption, false);
        //    m_obAnim.SetBool(m_sMoveAniOption, true);
        //    m_bDownUp = true;               // �Ʒ���
        //}
    }

    void SetLeftRight(Vector3 targetPosition)
    {
        if (transform.position.x == targetPosition.x) return;                                 // x �� ������ ������ ������.
        else if (transform.position.x < targetPosition.x) ChangeRotate(DefineManager.RIGHT);
        else ChangeRotate(DefineManager.LEFT);
    }

    void Start()
    {
        GameTouchManager.instance.RegisterEvent(this);
        GameTouchManager.instance.RegisterEvent(this);
        CameraManager.instance.Register(gameObject);
        m_nRemainHp = m_nMaxHp;
        m_nHpKey = HpBar.instance.AddObject(transform.position, (int)m_nMaxHp, (int)m_nRemainHp,0,0.7f);
        HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);
    }

    void DeadUpdate()
    {
        m_fDeadMotionTime += Time.deltaTime;
        if (m_fDeadMotionTime >= 1f)
        {
            m_fDeadMotionTime           = 0f;
            m_bDeadWait                 = true;
            m_targetPosition            = transform.position;
            m_Renderer.material.color   = Color.white;
            Color temp                  = m_Renderer.material.color;
            temp.a                      = 1f;
            m_Renderer.material.color   = temp;
        }
    }

    void DamageUpdate()
    {
        m_fDamageMotionTime += Time.deltaTime;

        if (m_fDamageMotionTime >= 0.25f)
        {
            m_fDamageMotionTime         = 0f;
            m_targetPosition            = transform.position;
            m_Renderer.material.color   = Color.white;
            Color temp                  = m_Renderer.material.color;
            temp.a                      = 1f;
            m_Renderer.material.color   = temp;
            SetStateReset();
        }
    }

    void NoEnemyState()
    {
        m_fNoEnemyTime += Time.deltaTime;
        if(m_fNoEnemyTime >= 1f)
        {
            m_fNoEnemyTime              = 0f;
            m_Renderer.material.color   = Color.white;
            Color temp                  = m_Renderer.material.color;
            temp.a                      = 1f;
            m_Renderer.material.color   = temp;
            PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);
            PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        }
    }

    void Move()
    {
        if (m_bClicked && m_obMove.Run(m_targetPosition,m_fSpeed))
        {
            HpBar.instance.ChangePosition(m_nHpKey, transform.position);                                               // �ִϸ��̼� isMove
        }
        else
        {
            m_bClicked = false;
            m_obAnim.SetBool(m_sMoveAniOption, false);
            m_obAnim.SetBool(m_sBackMoveAniOption, false);
        }
    }

    void FixedUpdate()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;         // Pause

        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY)
        {
            NoEnemyState();
        }


        // ���� ����� ������Ʈ�� ���ų�, ���� ������� ������Ʈ�� ��Ȱ��ȭ ���°� �ȴٸ� �ʱ�ȭ��Ŵ.



        if (m_bFirst == false)
        {
            HpBar.instance.ChangePosition(m_nHpKey, transform.position);
            m_bFirst = true;
        }

        if (m_obMove == null) return;

        if (m_bIsDead == true)                                                                      // �÷��̾� ���   
        {
            DeadUpdate();
            return;
        }

        if (m_bRecieveDamage == true) DamageUpdate();
        

        if (IsPossibleMove() == false) return;  // ��ų�� ������ ���Ƽ� SkillManager �� �ڷ�ƾ���� ó����.
        
  
        Move();
    }

    public bool IsPossibleUse()
    {
        return m_obMove != null && !m_bIsDead;
    }

    bool IsPossibleMove()
    {
        return !m_bIsDead;
    }

    public bool IsReceiveDamage() { return m_bRecieveDamage; }

    public Move2D GetMove()                 {   return m_obMove;    }
    public int GetHpKey()                   {   return m_nHpKey;    }
    public GameObject GetGameObject()       { return gameObject; }

    public GameObject GetNearestObject()
    {
        if (m_ObNearestObject == null || m_ObNearestObject.activeSelf == false) return null;
        return m_ObNearestObject;
    }

    bool GetDamage(int nDamage)
    {
        m_nRemainHp -= nDamage;

        if (m_nRemainHp < 0) m_nRemainHp = 0;

        HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);

        if (m_nRemainHp > 0)
        {
            m_targetPosition = transform.position;

            // ���� �ʰ� �������� ����.
            SetStateReset(); 
            m_bRecieveDamage = true;
            //SetAnimDamage(m_bRecieveDamage);
            return true;
        }
        else
        {
            m_bDeadWait = false;
            m_targetPosition = transform.position;

            // �׾���.
            SetStateReset(); 
            m_bIsDead = true;
            SetAnimDead(m_bIsDead);
        }
        return false;
    }

    public Vector3 GetPosition()    { return transform.position;    }
    public Transform GetTransform() { return transform;             }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;

        if (collision.CompareTag("Monster"))
        {
            m_lDamageTime = DateTime.Now.Ticks;
            DamagedMonster();
        }
        //Damage temp = other.GetComponent<Damage>();
        //if (temp == null) return;       // �ε��� ������Ʈ�� �������� ���� �ִٸ�, �������� ����.

        //int nDamage = temp.GetDamage();
        //bool bRes = GetDamage(nDamage);
        //if (bRes) return;
        //StartCoroutine(CheckEndGame());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;

        if (collision.CompareTag("Monster"))
        {
            if (DateTime.Now.Ticks - m_lDamageTime >= 1000000)
            {
                m_lDamageTime = DateTime.Now.Ticks;
                DamagedMonster();
            }
        }
    }

    void DamagedMonster()
    {
        m_Renderer.material.color = Color.black;
        Color temp = m_Renderer.material.color;
        temp.a = 0.8f;
        m_Renderer.material.color = temp;
        bool bRes = GetDamage(1);
        if (bRes) return;
        StartCoroutine(CheckEndGame());
    }

    IEnumerator CheckEndGame()
    {

        while (m_bDeadWait == false) yield return null;

        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);

        m_bDeadWait = false;

        //Debug.Log("GameUIManager.instance.GetRestart(): " + GameUIManager.instance.GetRestart());

        if (GameUIManager.instance.GetRestart() == DefineManager.PLAYING_MIN_CONTINUE_COUNT)
        {
            ExitGame();
            yield break;
        }

        SetGameOver();
        GameUIManager.instance.PrintDeadQuestion(true);
    }

    public void ExitGame()
    {
        gameObject.SetActive(false);
        HpBar.instance.ChangeActive(m_nHpKey, false);
        string[] param = { "StartScene", "0", "PlayGameLobbyMove", "2" };
        UIClickStartManager.LoadScene(new CommunicationTypeDataClass(0, null, param));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position += (transform.position - m_targetPosition) * 0.001f;
        SetCollisionState();
    }
  
    public bool IsDead()         { return m_bIsDead;            }

    public void OnTargetPosition(Vector3 position)
    {
        if (IsPossibleMove() == false) return;

        // ���� ���⿡���� ĳ������ ������ ��´�. 

        Vector3 tempPosition = transform.position + position;
        m_Direction = tempPosition - transform.position;
        SetClicked(tempPosition, true);
    }

    public void OnStop()
    {
        SetClicked(transform.position, true);
    }

    void ChangeRotate(bool flg)
    {  
        Vector3 tmp = transform.localScale;
        if (flg == false)   tmp.x = Mathf.Abs(tmp.x);   
        else                tmp.x = -Mathf.Abs(tmp.x);
        transform.localScale = tmp;
    }

    public void AddEx(int nEx)
    {
        m_nEx += nEx;

        int maxEx = OptionManager.instance.GetLevelEx(m_nLevel);
        if(m_nEx >= maxEx)
        {
            m_nEx %= maxEx;
            m_nLevel++;                             // ���� ����
            GameUIManager.instance.PlayerLevelUp(); // ������ ����
            // ������ UI ���
        }

    }

    public int GetLevel()   { return m_nLevel; }
    public int GetEx()      { return m_nEx; }

    public void OnSkill(List<TouchCircle> skills) { }

    public float GetNearDist() {
        if (m_ObNearestObject == null || m_ObNearestObject.activeSelf == false) return 1e5f;
        return m_fDistance;
    }

    public void SetPlusHp(float fPlusHp)
    {
        Debug.Log("Plus Hp : " + fPlusHp);
        m_nRemainHp += fPlusHp;
        m_nRemainHp = Mathf.Min(m_nRemainHp, m_nMaxHp);
        HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);
    }
}
