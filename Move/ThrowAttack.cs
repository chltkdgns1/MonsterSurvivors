using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : MonoBehaviour
{
    private bool m_bInit = false;
    private GameObject m_ObTargerObject = null;

    [SerializeField]
    private GameObject m_ObThrowObject = null;              // 1. ����Ƽ ������Ʈ���� �־��� ���
                                                            // 2. �ܺ� ���ڷ� �Է¹��� ���

    [SerializeField]
    private int m_nThrowSize = 10;

    private List<GameObject> m_ObjectPool = new List<GameObject>();

    private float   m_fCreateTime;
    private int     m_nCreateIndex;
    private int     m_nObjectSize;


    public void InitThrow(GameObject targetOb)
    {
        m_bInit = true;
        m_ObTargerObject = targetOb;
    }

    public void SetTargetObject(GameObject Ob)
    {
        m_bInit = true;
        m_ObTargerObject = Ob;
    }

    void SetObjectPool(GameObject Ob, int nSize)
    {
        for(int i = 0; i < nSize; i++)
        {
            m_ObjectPool.Add(Instantiate(Ob, transform));
            m_ObjectPool[i].SetActive(false);
        }
    }

    void SetAllStartPosition()
    {
        for(int i = 0; i < m_ObjectPool.Count; i++)
        {
            m_ObjectPool[i].transform.position = new Vector3(0, 0, 0);
            m_ObjectPool[i].SetActive(false);
        }
    }

    private void Awake()
    {
        if (m_ObThrowObject != null)
            SetObjectPool(m_ObTargerObject, m_nThrowSize);
    }

    private void OnEnable()
    {
        SetAllStartPosition();
        m_fCreateTime = 0f;
        m_nCreateIndex = 0;
        m_nObjectSize = m_ObjectPool.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bInit == false)           return;
        if (m_ObThrowObject == null)    return;

        if(m_fCreateTime <= 0f)     // 0 �� �Ǹ� ���ο� ��ų ����
        {
            m_fCreateTime = 1f;
            CreateThrow(m_nCreateIndex++);
            m_nCreateIndex %= m_nObjectSize;
        }

        m_fCreateTime -= Time.deltaTime;
    }

    void CreateThrow(int index)
    {
        m_ObjectPool[index].transform.position = new Vector3(0, 0, 0);
        m_ObjectPool[index].SetActive(true);
    }
    void DeleteThrow(int index)
    {
        m_ObjectPool[index].transform.position = new Vector3(0, 0, 0);
        m_ObjectPool[index].SetActive(false);
    }
}
