using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSizeChange : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private bool m_bReverse = false;

    [SerializeField]
    private float m_fStartWidthChange;

    [SerializeField]
    private float m_fStartHeightChange;

    private RectTransform m_RectTransform;

    [SerializeField]
    private int m_nMoveCnt;

    [SerializeField]
    private bool m_bUseChildResize = true;

    private List<RectTransform> m_ImageChildList = new List<RectTransform>();

    private bool m_bUpDownCheck = false;
    private bool m_bSetStop = false;


    private List<pair<float, float>> m_SizeList = new List<pair<float, float>>();

   
    private void Awake()
    {     
        m_RectTransform = GetComponent<Image>().rectTransform;
        m_ImageChildList.Add(m_RectTransform);
        //m_fHeight = m_RectTransform.rect.height;
        //m_fWidth = m_RectTransform.rect.width;
    }

    public void SetStop(bool bStop)
    {
        m_bSetStop = bStop;
        InitSize();                 // 사이즈를 초기화한다.
    }

    public void InitSize()
    {
        int nSize = m_SizeList.Count;
        for(int i = 0; i < nSize; i++)
        {
            m_ImageChildList[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_SizeList[i].first);
            m_ImageChildList[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_SizeList[i].second);
        }
    }

    private void OnEnable()
    {
        m_bUpDownCheck = false;
        InitSize();
    }

    private void Start()
    {
        if (m_bUseChildResize == true)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Image>())
                    m_ImageChildList.Add(transform.GetChild(i).GetComponent<Image>().rectTransform);
            }
        }

        int nSize = m_ImageChildList.Count;
        for(int i = 0; i < nSize; i++)       
            m_SizeList.Add(new pair<float, float>(m_ImageChildList[i].rect.width, m_ImageChildList[i].rect.height));       
    }

    private void Update()
    {
        if (m_bSetStop == true) return;

        if(m_bUpDownCheck == false)
        {
            m_bUpDownCheck = true;
            StartCoroutine(ChangeSmall());
        }   
    }

    public IEnumerator ChangeSmall()
    {
        if (m_bSetStop) yield break;

        float widthChange = m_fStartWidthChange / 10.0f;
        float heightChange = m_fStartHeightChange / 10.0f;

        int sz = m_ImageChildList.Count;
        for (int i = 1; i <= m_nMoveCnt; i++)
        {
            if (m_bSetStop) yield break;

            for (int k = 0; k < sz; k++)
            {
                if (m_bSetStop) yield break;


                float width = m_ImageChildList[k].rect.width;
                float height = m_ImageChildList[k].rect.height;

                if (m_bReverse)
                {
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width - widthChange);
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + heightChange);
                }
                else
                {
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width - widthChange);
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height - heightChange);
                }
            }
            yield return null;
        }

        StartCoroutine(ChangeBig());
    }

    public IEnumerator ChangeBig()
    {
        if (m_bSetStop) yield break;

        float widthChange = m_fStartWidthChange / 10.0f;
        float heightChange = m_fStartHeightChange / 10.0f;

        int sz = m_ImageChildList.Count;
        for (int i = 1; i <= m_nMoveCnt; i++)
        {
            if (m_bSetStop) yield break;

            for (int k = 0; k < sz; k++)
            {
                if (m_bSetStop) yield break;

                float width = m_ImageChildList[k].rect.width;
                float height = m_ImageChildList[k].rect.height;

                if (m_bReverse)
                {
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + widthChange);
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height - heightChange);
                }
                else
                {
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + widthChange);
                    m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + heightChange);
                }
            }
            yield return null;
        }

        m_bUpDownCheck = false;
    }
}
