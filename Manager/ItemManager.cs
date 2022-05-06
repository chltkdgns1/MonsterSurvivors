using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance = null;
    private List<List<GameObject>> m_ItemJewel  = new List<List<GameObject>>();         // 기본 1000개 생성
    private List<GameObject> m_ItemMoney        = new List<GameObject>();
    private List<List<GameObject>> m_ItemBox    = new List<List<GameObject>>();         // 기본 50개 생성 

    [SerializeField]
    private GameObject[] m_ItemJewelPrefabs;
    [SerializeField]
    private GameObject[] m_ItemBoxPrefabs;

    [SerializeField]
    private GameObject m_ItemMoneyPrefabs;

    private int m_nJewelKey;
    private int m_nBoxKey;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            CreateObject();
        }
        else Destroy(gameObject);
    }

    void CreateObject()  // 일정 개수만 미리 생성함.
    {
        for (int i = 0; i < 4; i++)
        {
            m_ItemJewel.Add(new List<GameObject>());
            for (int k = 0; k < 300; k++)
            {
                GameObject temp = Instantiate(m_ItemJewelPrefabs[i]);
                m_ItemJewel[i].Add(temp);
                temp.SetActive(false);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            m_ItemBox.Add(new List<GameObject>());
            for (int k = 0; k < 50; k++)
            {
                GameObject temp = Instantiate(m_ItemBoxPrefabs[i]);
                m_ItemBox[i].Add(temp);
                temp.SetActive(false);
            }
        }

        for(int i = 0; i < 300; i++)
        {
            GameObject temp = Instantiate(m_ItemMoneyPrefabs);
            m_ItemMoney.Add(temp);
            temp.SetActive(false);
        }
    }


    public void CreateJewel(int type, Vector3 vPosition)
    {
        type--;
        if (type > 3) return;

        bool flag = false;
        for(int i = 0; i < m_ItemJewel[type].Count; i++)
        {
            if(m_ItemJewel[type][i].activeSelf == false)
            {
                m_ItemJewel[type][i].transform.position = vPosition;
                m_ItemJewel[type][i].SetActive(true);
                flag = true;
                break;
            }
        }

        if(!flag) m_ItemJewel[type].Add(Instantiate(m_ItemJewelPrefabs[type], vPosition, m_ItemJewelPrefabs[type].transform.rotation));
        // 존재하지 않는다면 생성함.
    }

    public void CreateBox(int type, Vector3 vPosition)
    {
        if (type >= 5)
        {
            Debug.LogError("CreateBox 5 이상 수 들어옴");
            return;
        }

        bool flag = false;
        for (int i = 0; i < m_ItemBox[type].Count; i++)
        {
            if (m_ItemBox[type][i].activeSelf == false)
            {
                m_ItemBox[type][i].transform.position = vPosition;
                m_ItemBox[type][i].SetActive(true);
                flag = true;
                break;
            }
        }

        if (!flag) m_ItemBox[type].Add(Instantiate(m_ItemBoxPrefabs[type], vPosition, m_ItemBoxPrefabs[type].transform.rotation));
        // 존재하지 않는다면 생성함.
    }

    public void CreateMoney(int nEx, Vector3 vPosition)
    {
        bool flag = false;
        for (int i = 0; i < m_ItemMoney.Count; i++)
        {
            if (m_ItemMoney[i].activeSelf == false)
            {
                m_ItemMoney[i].transform.position = vPosition;
                m_ItemMoney[i].SetActive(true);
                m_ItemMoney[i].GetComponent<Item>().SetEx(nEx);
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            GameObject tempOb = Instantiate(m_ItemMoneyPrefabs, vPosition, m_ItemMoneyPrefabs.transform.rotation);
            tempOb.GetComponent<Item>().SetEx(nEx);
            m_ItemMoney.Add(tempOb);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
}
