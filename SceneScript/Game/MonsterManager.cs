using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    static public MonsterManager instance = null;

    private List<GameObject> m_ObMonster = new List<GameObject>();      // ���� ���� ������Ʈ�� ������ ģ��
    private List<float> m_fMonsterRePrintTime = new List<float>();

    [SerializeField]
    private GameObject[] m_MonsterPrefabs;                              // ���� ������ , �����ϴ� ��� ������ ����.
    [SerializeField]        
    private int[] m_nMonsterNum;                                        // �ʱ� ���� ���귮, �� ó�� initMonster���� 
    [SerializeField]
    private float[] m_fMonsterCreatePeriodic;                           // ���� �߰� ���� �ֱ�,  Update ���� ���� �ֱ� �ľ�
    [SerializeField]
    private int[] m_nMonsterCreateNum;                                  // ���� �߰��� ���� �ֱ�, Update ���� ����ŭ ����
    [SerializeField]
    private float[] m_fRePrintTime;                                     // �װ� ���� ����� �ð�.   �����ϴ� ��� ������ ���

    private float[] m_fMonsterCreatePeriodicSum;                        // ���� ���� �ֱ�           Update ���� ���� �ֱ� ����           

    private int m_nKey;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        init();
        InitMonster();
    }

    void init()
    {
        m_nKey = 0;
        m_fMonsterCreatePeriodicSum = new float[m_fMonsterCreatePeriodic.Length];

        Debug.Log("m_fMonsterCreatePeriodicSum : " + m_fMonsterCreatePeriodicSum.Length);

        for (int i = 0; i < m_fMonsterCreatePeriodicSum.Length; i++)
            m_fMonsterCreatePeriodicSum[i] = 0f;
    }

    void InitMonster()
    {                           
        // �� ó������ ���㸸 ������.
        int monNumSize = m_nMonsterNum.Length;

        for (int i = 0; i < monNumSize; i++)
        {
            for (int k= 0; k < m_nMonsterNum[i]; k++)
                CreateMonster(m_MonsterPrefabs[i], m_fRePrintTime[i]);
        }
    }

    void CreateMonster(GameObject ObMonster, float fRePrintTime)    // ������ �־��ָ� ������.
    {
        GameObject tempOb = Instantiate(ObMonster, ObMonster.transform.position, Quaternion.identity);
        MonsterInterface tempComp = tempOb.GetComponent<MonsterInterface>();
        tempComp.SetKey(m_nKey++);
        m_ObMonster.Add(tempOb);
        m_fMonsterRePrintTime.Add(fRePrintTime);
    }

    public void DeadMonster(int nKey, int nEx, int nBox)
    {
        CreateItem(nKey, nEx, nBox);
        StartCoroutine(WaitCreateMonster(nKey));
    }

    void CreateItem(int nKey, int nEx, int nBox)
    {
        if(nBox != -1)   ItemManager.instance.CreateBox(nBox, m_ObMonster[nKey].transform.position);
        else             ItemManager.instance.CreateJewel(nEx, m_ObMonster[nKey].transform.position);
    }
    

    IEnumerator WaitCreateMonster(int nKey)
    {
        float fWaitTime = 0f;
        m_ObMonster[nKey].SetActive(false);
        while (fWaitTime <= m_fMonsterRePrintTime[nKey]) fWaitTime += Time.deltaTime;
        m_ObMonster[nKey].SetActive(true);
        yield break;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;

        for (int i = 0; i < m_fMonsterCreatePeriodicSum.Length; i++)
        {
            m_fMonsterCreatePeriodicSum[i] += Time.deltaTime;

            if (m_fMonsterCreatePeriodicSum[i] >= m_fMonsterCreatePeriodic[i])
            {
                m_fMonsterCreatePeriodicSum[i] = 0;
                StartCoroutine(CreatePeriodMonster(i, m_MonsterPrefabs[i], m_nMonsterCreateNum[i], m_fRePrintTime[i]));
            }
        }
    }

    IEnumerator CreatePeriodMonster(int index, GameObject ObMonster,int nCreateNumber, float fRePrintTime) 
    {
        for(int i = 0; i < nCreateNumber; i++)
        {
            CreateMonster(ObMonster, fRePrintTime);
        }
        Debug.Log("���� ������ : " + m_ObMonster.Count);
        yield break;
    }
}
