using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicked : MonoBehaviour            // UI 가 클릭되었을 때
{
    // Start is called before the first frame update
    [SerializeField]
    private float m_fStartWidthChange;

    [SerializeField]
    private float m_fStartHeightChange;

    private MaskableGraphic m_uiObject;

    private RectTransform m_RectTransform;

    [SerializeField]
    private int m_nMoveCnt;

    [SerializeField]
    private int m_nEndStartType;    // 종료 후 어떤 것을 실행할 것인지

    [SerializeField]
    private bool m_bSendObject;

    [SerializeField]
    private string[] parameter;

    // 0 은 아무 타입도 아님
    // 클릭된 UI 의 타입, DefineManager CLICKED_TYPE_DIA 부분 참고
    // 다이아, 루비 일 수 있고 기타 추가되는 부분일 수 있음.

    private GameObject m_obMine = null;

    private Vector3 m_vClickPosition;

    private CommunicationTypeDataClass m_value;

    [SerializeField]
    private bool m_bContinueClick = false;                  // 연속 클리이 가능한 것인지

    private int m_nClicked = 0;

    [SerializeField]
    private bool m_bUseChildResize = true;
    private List<RectTransform> m_ImageChildList = new List<RectTransform>();

    private event EventCallBack m_eventCallBack = null;

    public EventCallBack EventCallBack
    {
        set { m_eventCallBack = value; }
    }

    private void Awake()
    {
        m_obMine = null;
        if (m_bSendObject)
        {
            m_obMine = gameObject;
        }
        m_uiObject = GetComponent<Image>();
        m_nClicked = 0;

        m_RectTransform = GetComponent<Image>().rectTransform;
        m_ImageChildList.Add(m_RectTransform);

        initCommunicateValue();
    }

    private void Start()
    {
        if(m_bUseChildResize == true)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Image>())
                    m_ImageChildList.Add(transform.GetChild(i).GetComponent<Image>().rectTransform);               
            }
        }
    }

    void initCommunicateValue()
    {
        m_value = new CommunicationTypeDataClass(m_nEndStartType, m_obMine, parameter);
    }

    public void InitCommunicateValue(CommunicationTypeDataClass value)
    {
        m_value = value;
    }

    public void MoveSmall()
    {
        m_nClicked++;
        StartCoroutine(ClickDown());
    }

    public void MoveBig()
    {
        StartCoroutine(ClickUp());
    }

    public IEnumerator ClickDown()
    { 
        m_vClickPosition = transform.position;

        float widthChange = m_fStartWidthChange / 10.0f;
        float heightChange = m_fStartHeightChange / 10.0f;

        int sz = m_ImageChildList.Count;
        for (int i = 1; i <= m_nMoveCnt; i++)
        {
            //RectTransform rect = (RectTransform)m_uiObject.rectTransform;

            for (int k = 0; k < sz; k++)
            {
                float width  = m_ImageChildList[k].rect.width;
                float height = m_ImageChildList[k].rect.height;
                m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width - widthChange);
                m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height - heightChange);
            }
            yield return null;
        }
    }

    public IEnumerator ClickUp()
    {
        float widthChange = m_fStartWidthChange / 10.0f;
        float heightChange = m_fStartHeightChange / 10.0f;

        int sz = m_ImageChildList.Count;
        for (int i = 1; i <= m_nMoveCnt; i++)
        {
            //RectTransform rect = (RectTransform)m_uiObject.rectTransform;

            for (int k = 0; k < sz; k++)
            {
                float width = m_ImageChildList[k].rect.width;
                float height = m_ImageChildList[k].rect.height;
                m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + widthChange);
                m_ImageChildList[k].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + heightChange);
            }
            yield return null;
        }

        m_nClicked--;
        if (m_vClickPosition != transform.position) yield break;

        if (m_bContinueClick == false && m_nClicked != 0) yield break;

        //Debug.Log("여기 안옴?");
        Execute();
    }

    public void SetParam(string[] param)
    {
        m_value.SetParameter(param);
    }

    public void Execute()
    {

        if(m_eventCallBack != null)
        {
            m_eventCallBack(m_value);
            return;
        }

        UIClickStartManager.Execute(m_value);
    }
}
