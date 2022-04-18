using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    static public HpBar instance = null;

    [SerializeField]
    private GameObject m_goPrefabs = null;

    private int m_nSequenceNumber = 0;

    [SerializeField]
    private GameObject m_ObParent;

    [SerializeField]
    private float m_fXpos = 0f;
    [SerializeField]
    private float m_fYpos = 0.5f;
    [SerializeField]
    private float m_fZpos = 0.5f;

    class HpBarStruct
    {
        public GameObject m_hpBar;
        public Image m_hpBarProgress;
        public Text m_hpBarText;
        public int m_nTotalHp;
        public int m_nRemainHp;

        public float m_fXpos;
        public float m_fYpos;
        public float m_fZpos;

        public HpBarStruct(GameObject obHpBar, Image obProgress, Text obText, int totalHp, int remainHp, float xpos, float ypos, float zpos)
        {
            m_hpBar = obHpBar;
            m_hpBarProgress = obProgress;
            m_hpBarText = obText;
            m_nTotalHp = totalHp;
            m_nRemainHp = remainHp;
            m_fXpos = xpos;
            m_fYpos = ypos;
            m_fZpos = zpos;
        }
    }

    private Dictionary<int, HpBarStruct> m_DicManager = new Dictionary<int, HpBarStruct>();
    private Camera m_cam = null;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        m_cam = Camera.main;
    }

    public int AddObject(Vector3 position, int total, int remain, float xpos = 0f, float ypos = 0.5f, float zpos = 0.5f)                    // 새롭게 HPBar 로 관리되어야할 오브젝트 추가
    {
        if (m_goPrefabs == null) return DefineManager.RETURN_ERROR;

        GameObject tempObject = Instantiate(m_goPrefabs, m_cam.WorldToScreenPoint(position + new Vector3(xpos, ypos, zpos)), Quaternion.identity, transform);

        Image image = tempObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        Text text = tempObject.transform.GetChild(1).gameObject.GetComponent<Text>();

        text.text = "" + remain;
        m_DicManager.Add(m_nSequenceNumber, new HpBarStruct(tempObject, image, text, total, remain, xpos, ypos, zpos));
        return m_nSequenceNumber++;
    }

    public int AddObject2D(Vector3 position, int total, int remain, float xpos = 0f, float ypos = 200f, float zpos = 0f)                    // 새롭게 HPBar 로 관리되어야할 오브젝트 추가
    {
        if (m_goPrefabs == null) return DefineManager.RETURN_ERROR;

        Vector3 Position = (position + new Vector3(xpos, ypos));
        //Debug.Log("Position : " + Position);

        GameObject tempObject = Instantiate(m_goPrefabs, Position, Quaternion.identity, m_ObParent.transform);

        Image image = tempObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        Text text = tempObject.transform.GetChild(1).gameObject.GetComponent<Text>();

        text.text = "" + remain;
        m_DicManager.Add(m_nSequenceNumber, new HpBarStruct(tempObject, image, text, total, remain, xpos, ypos, zpos));
        ChangePosition(m_nSequenceNumber, position);
        return m_nSequenceNumber++;
    }

    public bool DeleteObject(int key)
    {
        if (m_DicManager.ContainsKey(key) == false)
        {
            //Debug.LogError("public bool DeleteObject(int key): " + key);
        }
        Destroy(m_DicManager[key].m_hpBar);
        return m_DicManager.Remove(key);
    }

    public void ChangeRemainHp(int key, int remain)
    {
        if (m_DicManager.ContainsKey(key) == false)
        {
            //Debug.LogError("public void ChangeActive(int key, bool flag): " + key);
        }
        HpBarStruct temp = m_DicManager[key];
        temp.m_nRemainHp = remain;
        temp.m_hpBarText.text = remain.ToString();
        temp.m_hpBarProgress.fillAmount = (float)(remain) / temp.m_nTotalHp;
    }

    public void ChangeActive(int key, bool flag)
    {
        if (m_DicManager.ContainsKey(key) == false)
        {
            //Debug.LogError("public void ChangeActive(int key, bool flag) : " + key);
        }

        m_DicManager[key].m_hpBar.SetActive(flag);
    }

    public void ChangActiveAll(bool flag)
    {
        foreach(HpBarStruct value in m_DicManager.Values)
        {
            value.m_hpBar.SetActive(flag);
        }
    }

    public void ChangePosition2D(int key, Vector3 position)
    {
        if (m_DicManager.ContainsKey(key) == false)
        {
            //Debug.LogError("public void ChangePosition2D(int key, Vector3 position): " + key);
        }
        HpBarStruct temp = m_DicManager[key];
        temp.m_hpBar.transform.position = position + new Vector3(temp.m_fXpos, temp.m_fYpos, temp.m_fZpos);
    }

    public void ChangePosition(int key, Vector3 position)
    {
        if (m_DicManager.ContainsKey(key) == false)
        {
            //Debug.LogError(" public void ChangePosition(int key, Vector3 position): " + key);
        }
        //Debug.Log("key : " + key);
        HpBarStruct temp = m_DicManager[key];
        temp.m_hpBar.transform.position = m_cam.WorldToScreenPoint(position + new Vector3(temp.m_fXpos, temp.m_fYpos, temp.m_fZpos));
    }
}
