using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 규칙
/// 일단 움직임 제외하고 어떤 액션이 발생하면
/// IsMove 함수에 해당 bool 값을 체크한다.
/// Update 함수 내부에서 해당 모션을 끝낸 후 터치 입력이 들어온 것을 해소한다
/// Ex)m_targetPosition = transform.position;
/// </summary>

public class PlayerOffline2D : MonoBehaviour, TouchGameEvent
{
    static public PlayerOffline2D instance = null;

    private Vector3 m_targetPosition;     // 목적지 위치 포지션

    private Move2D      m_obMove;
    private Animator    m_obAnim;

    /// <summary>
    /// 애니메이션 옵션
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

    private int m_nKillMonster;

    private Hp m_Hp;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        m_nKillMonster = 0;
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

        m_obMove    = GetComponent<Move2D>();
        m_obAnim    = GetComponent<Animator>();
        m_Hp        = GetComponent<Hp>();

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
            //Debug.Log("죽으면서 초기화함?");
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
            //Debug.Log("거리 새롭게 갱신 : " + fDistance);
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

        m_Hp.MoveHpBar(transform.position);
        //HpBar.instance.ChangePosition(m_nHpKey, transform.position);
    }

    void SetGameOver()
    {
        gameObject.SetActive(false);
        initValue();
        m_Hp.SetActive(false);
        //HpBar.instance.ChangeActive(m_nHpKey, false);
    }

    public void SetRestart()
    {
        GameValueManager.instance.SetRestart(0);
        GameUIManager.instance.PrintDeadQuestion(false);
        gameObject.SetActive(true);

        initValue();

        m_Hp.SetActive(true);
        m_Hp.SetMaxHp();
        m_Hp.ChangeRemainHp();
        m_targetPosition = transform.position;
        m_Hp.MoveHpBar(transform.position);
        GameUIManager.instance.RestartGameTimer();

        //HpBar.instance.ChangeActive(m_nHpKey, true);
        //HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);
        //m_targetPosition = transform.position;
        //HpBar.instance.ChangePosition(m_nHpKey, transform.position);
       // GameUIManager.instance.RestartGameTimer();

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
        //    m_bDownUp = false;              // 위쪽
        //}
        //else
        //{
        //    m_obAnim.SetBool(m_sBackMoveAniOption, false);
        //    m_obAnim.SetBool(m_sMoveAniOption, true);
        //    m_bDownUp = true;               // 아래쪽
        //}
    }

    void SetLeftRight(Vector3 targetPosition)
    {
        if (transform.position.x == targetPosition.x) return;                                 // x 가 같으면 현상태 유지함.
        else if (transform.position.x < targetPosition.x) ChangeRotate(DefineManager.RIGHT);
        else ChangeRotate(DefineManager.LEFT);
    }

    void Start()
    {
        GameTouchManager.instance.RegisterEvent(this);
        GameTouchManager.instance.RegisterEvent(this);
        CameraManager.instance.Register(gameObject);
       // m_nRemainHp = m_nMaxHp;
       // m_nHpKey = HpBar.instance.AddObject(transform.position, (int)m_nMaxHp, (int)m_nRemainHp,0,0.7f);
       // HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);
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
        if (m_bClicked && m_obMove.Run(m_targetPosition))
        {
            m_Hp.MoveHpBar(transform.position);
            //HpBar.instance.ChangePosition(m_nHpKey, transform.position);                                               // 애니메이션 isMove
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


        // 가장 가까운 오브젝트가 없거나, 가장 가까웠던 오브젝트가 비활성화 상태가 된다면 초기화시킴.



        if (m_bFirst == false)
        {
            m_Hp.MoveHpBar(transform.position);
            //HpBar.instance.ChangePosition(m_nHpKey, transform.position);
            m_bFirst = true;
        }

        if (m_obMove == null) return;

        if (m_bIsDead == true)                                                                      // 플레이어 사망   
        {
            DeadUpdate();
            return;
        }

        if (m_bRecieveDamage == true) DamageUpdate();
        

        if (IsPossibleMove() == false) return;  // 스킬은 종류가 많아서 SkillManager 의 코루틴에서 처리함.
        
  
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
    public GameObject GetGameObject()       { return gameObject; }

    public GameObject GetNearestObject()
    {
        if (m_ObNearestObject == null || m_ObNearestObject.activeSelf == false) return null;
        return m_ObNearestObject;
    }

    bool GetDamage(int nDamage)
    {
        m_Hp.SetDamage((float)nDamage);
        m_Hp.ChangeRemainHp();

        //HpBar.instance.ChangeRemainHp(m_nHpKey, (int)m_nRemainHp);

        if (m_Hp.GetRemainHp() > 0f)
        {
            m_targetPosition = transform.position;

            // 죽지 않고 데미지만 입음.
            SetStateReset(); 
            m_bRecieveDamage = true;
            //SetAnimDamage(m_bRecieveDamage);
            return true;
        }
        else
        {
            m_bDeadWait = false;
            m_targetPosition = transform.position;

            // 죽었음.
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
            DamageInterface temp = collision.GetComponent<DamageInterface>();
            DamagedMonster(temp);
        }

        if (collision.CompareTag("ThrowAttack"))
        {
            DamageInterface temp = collision.GetComponent<DamageInterface>();
            DamagedMonster(temp);
        }

        //Damage temp = other.GetComponent<Damage>();
        //if (temp == null) return;       // 부딪힌 오브젝트가 데미지를 갖고 있다면, 데미지를 입음.

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
                DamageInterface temp = collision.GetComponent<DamageInterface>();
                DamagedMonster(temp);
            }
        }
    }

    void DamagedMonster(DamageInterface dDamageInter)
    {
        int nDamage = 1;
        if (dDamageInter != null) nDamage = dDamageInter.GetDamage();

        m_Renderer.material.color = Color.black;
        Color temp = m_Renderer.material.color;
        temp.a = 0.8f;
        m_Renderer.material.color = temp;
        bool bRes = GetDamage(nDamage);
        if (bRes)
        {
            //DamageTextManager.instance.SetDamageText(transform.position, nDamage, "red");
            return;
        }
        StartCoroutine(CheckEndGame());
    }

    IEnumerator CheckEndGame()
    {

        while (m_bDeadWait == false) yield return null;

        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);

        m_bDeadWait = false;

        //Debug.Log("GameUIManager.instance.GetRestart(): " + GameUIManager.instance.GetRestart());

        if (GameValueManager.instance.GetRestart() == DefineManager.PLAYING_MIN_CONTINUE_COUNT)
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

        m_Hp.SetActive(false);
        //HpBar.instance.ChangeActive(m_nHpKey, false);

        GameUIManager.instance.SetGameOverFunc(() =>
        {
            GameUIManager.instance.SetActiveEndGame(true);
            GameUIManager.instance.SetActiveGameOver(false);
        });
        GameUIManager.instance.SetActiveGameOver(true);

        //GameUIManager.instance.SetActiveEndGame(true);

        //string[] param = { "StartScene", "0", "PlayGameLobbyMove", "2" };
        //UIClickStartManager.LoadScene(new CommunicationTypeDataClass(0, null, param));
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

        // 오직 여기에서만 캐릭터의 방향을 잡는다. 

        Vector3 tempPosition = transform.position + position;
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

        int maxEx = ValueManager.instance.GetLevelEx(m_nLevel);
        if(m_nEx >= maxEx)
        {
            m_nEx %= maxEx;
            m_nLevel++;                             // 레벨 증가
            GameUIManager.instance.PlayerLevelUp(); // 레벨업 해줌
            // 레벨업 UI 출력
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
        m_Hp.SetPlusHp(fPlusHp);
        m_Hp.ChangeRemainHp();
    }

    public int GetKillMonster() { return m_nKillMonster; }
    public void AddKillMonster() { m_nKillMonster++; }
}

// 딱 한번만 초기화되어야하는 변수
// 여러번 초기화하는 변수