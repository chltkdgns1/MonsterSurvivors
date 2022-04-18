using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class UIEvent : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IPointerDownHandler
{
    // Start is called before the first frame update
    [SerializeField]
    private int m_nId;

    [SerializeField]
    private bool m_bSendObject;

    [SerializeField]
    private int m_nOption = 7; // 모든 기능 다 사용함.

    [SerializeField]
    private string[] parameter;

    private CommunicationTypeDataClass m_value;

    private GameObject m_obParent;
    private GameObject m_obMine = null;

    private void Awake()
    {
        m_obParent = null;

        if (m_bSendObject) // 오브젝트를 보내고 싶다면
        {
            //Debug.Log("자신을 보냄");
            m_obMine = gameObject;
        }

        initCommunicateValue();
    }
    void Start()
    {
      
    }

    void initCommunicateValue()
    {
        m_value = new CommunicationTypeDataClass(m_nId, m_obMine, parameter);
    }

    public void SetParentEvent(GameObject parent)
    {
        m_obParent = parent;
    }

    // Update is called once per frame

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("클릭");
        if ((m_nOption & DefineManager.POINTER_CLICK) != DefineManager.POINTER_CLICK) return;
        UIEventManager.OnClickEvent(m_value);   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("클릭");
        if ((m_nOption & DefineManager.POINTER_UP) != DefineManager.POINTER_UP) return;
        UIEventManager.OnClickUpEvent(m_value);   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("클릭");
        if ((m_nOption & DefineManager.POINTER_DOWN) != DefineManager.POINTER_DOWN) return;
        UIEventManager.OnClickDownEvent(m_value);  
    }
}
