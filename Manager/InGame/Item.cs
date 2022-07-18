using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private int m_nItemType;        // 아이템의 종류
    [SerializeField]
    private int m_nEx;              // 경험치

    private int m_nGold;

    [SerializeField]
    private bool m_bAd = false;

    private bool m_bPicked;

    [SerializeField]
    private float m_fPickedDistance = 4f;

    [SerializeField]
    float m_fSpeed = 20f;

    private void Awake()
    {
        init();
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        init();
    }

    void init()
    {
        m_bPicked = false;
    }

    public void SetEx(int nEx) { m_nEx = nEx; }

    // Update is called once per frame
    void Update()
    {
        GetPlayerDistance();
        if (m_bPicked == true) Move();        // 플레이어에게 피크 됬다면,               
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_nItemType <= 4)
                PlayGameMananger.instance.AddEx(m_nEx);

            else if (m_nItemType <= 9)
                GameUIManager.instance.GetTreasureBox(m_nItemType - 5, m_bAd);

            else
            {
                GetRandGold();
                GameValueManager.instance.AddGold(m_nGold);
            }

            gameObject.SetActive(false);
            // 아이템 매니저에서 관리함.
        }
    }

    void GetRandGold()
    {
        m_nGold = UnityEngine.Random.Range(1, m_nEx * 30);
    }

    void GetPlayerDistance()
    {
        if (PlayerOffline2D.instance == null) return;
        if (5 <= m_nItemType && m_nItemType <= 9) return;
        Vector2 Position = PlayerOffline2D.instance.transform.position;
        float fDist = Vector2.Distance(Position, transform.position);
        if (fDist < m_fPickedDistance) m_bPicked = true;
    }

    void Move()
    {
        Vector3 vDir = (PlayerOffline2D.instance.transform.position - transform.position).normalized;
        vDir = new Vector3(vDir.x, vDir.y, 0);
        transform.position += vDir * m_fSpeed * Time.deltaTime;
    }
}
