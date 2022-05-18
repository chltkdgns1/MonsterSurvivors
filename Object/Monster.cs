using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster : MonoBehaviour, IMonster, IDamage
{

    [SerializeField]
    private bool m_bCloseRangeMonster = false;  // flase : �ٰŸ� , true : ���Ÿ�

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
    private int m_nMinDamage;
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
    private bool m_bMirror = false; // �������� �ƴ���


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

    //private float m_fDamageLength = 0.1f; // ������ �ٽ� �޴� �ð�
    private long m_lDamageTime;
    private long m_lNuckBackTime;

    private Rigidbody2D m_Rigidbody2D;
    
    // 5�� ���� ������
    [SerializeField]
    private float m_fMovingTime = 2f;
    private float m_fRemainMoveTime;

    [SerializeField]
    private float m_fTargetDistance = 5f;  // �÷��̾ ���� ���� ������ �����ϸ� ���󰣴�. 

    private Vector3 m_vDir;

    private bool m_bTargetMove = false;
    private bool m_bTargetOn = false;       // Ÿ���� ��������, ������ �ð���ŭ�� ���󰣴�.
    private float m_fFixedTargetFollowTime = 10f;
    private float m_fRemainTargetFollowTime;
    
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
        m_bTargetOn = false;
    }

    void Update()
    {
        m_Rigidbody2D.velocity = Vector3.zero;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_CRAFTING) return;
        if (m_nKey == -1) return;               // Ű ���� �������� �ʾҴٸ�, ������ �ʱ�ȭ�� �ȷᰡ �ȵ� ����.

        SetNearestPlayerDistance(m_bDeath);

        if (m_bDeath    == true) return;
        if (m_bDamage   == true) return;

        Move();
    }

    void Move()
    {
        if (!m_bTargetOn && !m_bTargetMove && !GetTargetDistance()) // �� �� �ٰ����� 10�� ���� ���󳪴ϴ� flag , 5�� ������ ������ flag, ��������� ������ flag
        {
            SetDir((transform.position + m_vDir).x);
            MoveRand();                                 // ���� 5�� ����, �÷��̾ ������ ���� �ݰ� ������ ������ �ʴ´ٸ�.
        }
        else
        {
            SetTargetOn();
            SetDir(m_ObPlayer.transform.position.x);
            MoveNormal();
        }
    }

    void SetTargetOn()
    {
        if (!m_bTargetOn)
        {
            m_bTargetOn = true;
            m_fRemainTargetFollowTime = m_fFixedTargetFollowTime;
        }

        if (m_bTargetOn)
        {
            m_fRemainTargetFollowTime -= Time.deltaTime;
        }

        if (m_fRemainTargetFollowTime <= 0f) m_bTargetOn = false;
    }

    void MoveNormal()
    {
        if (m_bCloseRangeMonster)                                                                           // ���Ÿ� ������ ��쿡��
        {
            if (m_fFarRangeDistance <= Vector2.Distance(m_ObPlayer.transform.position, transform.position)) // ������ �Ÿ��� �����ϰ�
                m_Move2D.RunRigid(m_ObPlayer.transform.position);                                 // ������ �Ÿ� �ۿ� �ִٸ� �̵� ����       
        }
        else m_Move2D.RunRigid(m_ObPlayer.transform.position);
       
    }

    void MoveRand()
    {
        m_fRemainMoveTime -= Time.deltaTime;
        if (m_fRemainMoveTime <= 0f)
        {
            RandomPosition();
            m_fRemainMoveTime = m_fMovingTime;
        }
        m_Move2D.RunRigidDir(m_vDir);
    }

    bool GetTargetDistance()
    {
        float fDis = Vector2.Distance(PlayerOffline2D.instance.gameObject.transform.position, transform.position);
        return m_fTargetDistance >= fDis;
    }

    public void RandomAbility()
    {
        m_nHp               = UnityEngine.Random.Range(m_nMinHp, m_nMaxHp);
        m_nEx               = UnityEngine.Random.Range(m_nMinEx, m_nMaxEx);

        int nRandNum = UnityEngine.Random.Range(0, 100);

        m_nBox = -1;
        if (nRandNum < 30) m_nBox = UnityEngine.Random.Range(m_nMinBox, m_nMaxBox);

        m_nDamage           = 1;
        m_bDeath            = false;
        m_lNuckBackTime     = 0;

        Module.GetRandPosition(transform, m_fPositionRange, m_fPositionRange);
        //float xpos          = UnityEngine.Random.Range(-m_fPositionRange, m_fPositionRange);
        //float ypos          = UnityEngine.Random.Range(-m_fPositionRange, m_fPositionRange);
        //transform.position  = new Vector3(xpos, ypos, transform.position.z);

        // ��ġ�� ����־����.

        // ���� �÷��̾� ��ó ����.  ���� �߽ɺο��� �����ϴ°� ���� ��
    }

    private void OnDisable()
    {

    }

    void SetDir(float fXpos)
    {
        if (m_bUseDir == false) return;
        float xpos = transform.position.x - fXpos;
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
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_CRAFTING) return;

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
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_CRAFTING) return;

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

        temp.SetDamageSum(nDamage);
        PlayerOffline2D.instance.SetPlusHp(nDamage* temp.GetBlood());     
        PrintTextManager.instance.SetText(transform.position, temp.GetDamage().ToString());

        if (m_nHp <= 0)
        {
            m_bDeath = true;
            SetNearestPlayerDistance(true);
            initAnim();
            m_Anim.SetBool(m_sRun, true);
            m_Renderer.material.color = Color.white;
            PlayerOffline2D.instance.AddKillMonster();
            MonsterManager.instance.DeadMonster(m_nKey, m_nEx, m_nBox);
            //StartCoroutine(DeadMotion());
        }
        else
        {
            //m_bDamage = true;   
            float fNuckBack = temp.GetNuckBack();
            StartCoroutine(CollisionMotion(fNuckBack));          // ���ο��� m_bDamage ȣ��    
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

    public void SetDamage(int nMin, int nMax)
    {
        m_nMinDamage = nMin;
        m_nMaxDamage = nMax;
    }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(m_nMinDamage, m_nMaxDamage);
    }

    void RandomPosition()
    {
        m_vDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
    }

    public void SetTargetMove(bool flag) { m_bTargetMove = flag; }
}
