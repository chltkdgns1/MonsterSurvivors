using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Monster : MonoBehaviour, MonsterInterface
{

    [SerializeField]
    private bool m_bCloseRangeMonster = false;  // flase : 근거리 , true : 원거리

    [SerializeField]
    private float m_fFarRangeDistance = 3f;

    private int m_nKey = -1;

    [SerializeField]
    private int m_nMaxHp;
    [SerializeField]
    private int m_nMinHp;
    [SerializeField]
    private int m_nMaxEx;
    [SerializeField]
    private int m_nMinEx;
    [SerializeField]
    private int m_nMaxDamage;
    [SerializeField]
    private float m_fSpeed;
    [SerializeField]
    private int m_nMinBox;
    [SerializeField]
    private int m_nMaxBox;

    private int m_nHp;
    private int m_nEx;
    private int m_nBox;
    private int m_nDamage;

    private bool m_bDeath;
    private bool m_bDamage;

    [SerializeField]
    private bool m_bUseDir = true;

    [SerializeField]
    private bool m_bMirror = false; // 반전인지 아닌지


    [SerializeField]
    private string m_sPlayer;
    private GameObject m_ObPlayer;

    private Move2D m_Move2D;

    [SerializeField]
    private float m_fPositionRange = 20f;

    private Animator m_Anim;
    private string m_sRun = "Run";
    private string m_sStop = "Stop";

    private SpriteRenderer m_Renderer;

    //private float m_fDamageLength = 0.1f; // 데미지 다시 받는 시간
    private long m_lDamageTime;
    private long m_lNuckBackTime;

    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        init();
    }

    void init()
    {
        m_ObPlayer = GameObject.Find(m_sPlayer);
        m_Move2D = GetComponent<Move2D>();
        initState();
        m_Anim = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        initState();
        initAnim();
        RandomAbility();
        SetNearestPlayerDistance(true);
    }

    void Start()
    {
        SetNearestPlayerDistance(true);
    }

    void initAnim()
    {
        m_Anim.SetBool(m_sRun, false);
        m_Anim.SetBool(m_sStop, false);
    }

    void initState()
    {
        m_bDeath = false;
        m_bDamage = false;
    }

    void Update()
    {
        m_Rigidbody2D.velocity = Vector3.zero;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;
        if (m_nKey == -1) return;               // 키 값이 정해지지 않았다면, 게임이 초기화가 안료가 안된 것임.
        SetNearestPlayerDistance(m_bDeath);
        if (m_bDeath == true) return;
        if (m_bDamage == true) return;
        SetDir();

        if (m_bCloseRangeMonster)                                                                           // 원거리 몬스터일 경우에는
        {
            if (m_fFarRangeDistance <= Vector2.Distance(m_ObPlayer.transform.position, transform.position)) // 정해진 거리만 접근하고
            {
                m_Move2D.RunRigid(m_ObPlayer.transform.position, m_fSpeed);                                 // 정해진 거리 밖에 있다면 이동 가능
            }
        }
        else
        {
            m_Move2D.RunRigid(m_ObPlayer.transform.position, m_fSpeed);
        }
    }

    public void RandomAbility()
    {
        m_nHp               = UnityEngine.Random.Range(m_nMinHp, m_nMaxHp);
        m_nEx               = UnityEngine.Random.Range(m_nMinEx, m_nMaxEx);
        m_nBox              = UnityEngine.Random.Range(m_nMinBox, m_nMaxBox);
        m_nDamage           = 1;
        m_bDeath            = false;
        m_lNuckBackTime     = 0;

        Module.GetRandPosition(transform, m_fPositionRange, m_fPositionRange);
        //float xpos          = UnityEngine.Random.Range(-m_fPositionRange, m_fPositionRange);
        //float ypos          = UnityEngine.Random.Range(-m_fPositionRange, m_fPositionRange);
        //transform.position  = new Vector3(xpos, ypos, transform.position.z);

        // 위치를 잡아주어야함.

        // 굳이 플레이어 근처 말고.  센터 중심부에서 생성하는게 좋을 듯
    }

    private void OnDisable()
    {

    }

    void SetDir()
    {
        if (m_bUseDir == false) return;
        float xpos = transform.position.x - m_ObPlayer.transform.position.x;
        bool flag = xpos <= 0 ? true : false;
        if (m_bMirror) flag = !flag;
        Module.ChangeDirection(gameObject, flag);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;

        if (collision.CompareTag("Skill")) 
        {
            m_lDamageTime = DateTime.Now.Ticks;
            DamagedSkill(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;

        if (collision.CompareTag("Skill"))
        {
            if (DateTime.Now.Ticks - m_lDamageTime >= 3000000)
            {
                m_lDamageTime = DateTime.Now.Ticks;
                DamagedSkill(collision);
            }
        }
    }

    void DamagedSkill(Collider2D collision)
    {
        Skill temp = collision.GetComponent<Skill>();

        float nDamage = temp.GetDamage();
        m_nHp -= (int)nDamage;

        PlayerOffline2D.instance.SetPlusHp(nDamage* temp.GetBlood());     
        DamageTextManager.instance.SetDamageText(transform.position, (int)temp.GetDamage());

        if (m_nHp <= 0)
        {
            m_bDeath = true;
            SetNearestPlayerDistance(true);
            initAnim();
            m_Anim.SetBool(m_sRun, true);
            m_Renderer.material.color = Color.white;
            MonsterManager.instance.DeadMonster(m_nKey, m_nEx, m_nBox);
            //StartCoroutine(DeadMotion());
        }
        else
        {
            //m_bDamage = true;   
            float fNuckBack = temp.GetNuckBack();
            StartCoroutine(CollisionMotion(fNuckBack));          // 내부에서 m_bDamage 호출    
        }
    }

    IEnumerator DeadMotion()
    {
        initAnim();
        m_Anim.SetBool(m_sStop, true);
        Vector3 vBackDir = (transform.position - m_ObPlayer.transform.position).normalized * 2;
        vBackDir = vBackDir / 40f;
        vBackDir = new Vector3(vBackDir.x, vBackDir.y, 0);
        m_Renderer.material.color = Color.black;
        for (int i = 0; i < 10; i++)
        {
            transform.position += vBackDir;
            yield return null;
        }
        initAnim();
        m_Anim.SetBool(m_sRun, true);
        m_Renderer.material.color = Color.white;
        MonsterManager.instance.DeadMonster(m_nKey, m_nEx, m_nBox);
    }
    IEnumerator CollisionMotion(float fNuckBack)
    {
        if (m_bDamage) yield break;
        m_bDamage = true;
        initAnim();
        m_Anim.SetBool(m_sStop, true);
        Vector3 vBackDir = (transform.position - m_ObPlayer.transform.position).normalized;
        vBackDir = vBackDir * (1 + fNuckBack) * 0.025f;
        vBackDir = new Vector3(vBackDir.x, vBackDir.y, 0);
        m_Renderer.material.color = Color.black;
        for (int i = 0; i < 10; i++)
        {
            transform.position += vBackDir;
            yield return null;
        }
        initAnim();
        m_Anim.SetBool(m_sRun, true);
        m_bDamage = false;
        m_Renderer.material.color = Color.white;
    }

    public void SetKey(int key) { m_nKey = key; }
    public int GetKey() { return m_nKey; }

    void SetNearestPlayerDistance(bool flag)
    {
        if (PlayerOffline2D.instance == null) return;

        Vector2 playerPos = PlayerOffline2D.instance.transform.position;
        Vector2 monPos = transform.position;
        PlayerOffline2D.instance.SetNearestObject(gameObject, Vector2.Distance(playerPos, monPos),flag);
    }

    public bool GetCloserRange() { return m_bCloseRangeMonster; }
}
