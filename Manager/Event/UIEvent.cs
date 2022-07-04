using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class UIEvent : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IPointerDownHandler
{
    // Start is called before the first frame update
    [SerializeField]
    protected int m_nId;

    [SerializeField]
    protected bool m_bSendObject;

    [SerializeField]
    protected int m_nOption = 7; // 모든 기능 다 사용함.

    [SerializeField]
    protected string[] parameter;

    protected CommunicationTypeDataClass m_value;

    protected GameObject m_obParent;
    protected GameObject m_obMine = null;

    private void Awake()
    {
        m_obParent = null;
        if (m_bSendObject)  m_obMine = gameObject;      
        initCommunicateValue();
    }
    void Start()
    {
      
    }

    protected void initCommunicateValue()
    {
        m_value = new CommunicationTypeDataClass(m_nId, m_obMine, parameter);
    }

    public void InitCommunicateValue(string []param)
    {
        m_value.SetParameter(param);
    }

    public void SetParentEvent(GameObject parent)
    {
        m_obParent = parent;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if ((m_nOption & DefineManager.POINTER_CLICK) != DefineManager.POINTER_CLICK) return;
        UIEventManager.OnClickEvent(m_value);   
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if ((m_nOption & DefineManager.POINTER_UP) != DefineManager.POINTER_UP) return;
        UIEventManager.OnClickUpEvent(m_value);   
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if ((m_nOption & DefineManager.POINTER_DOWN) != DefineManager.POINTER_DOWN) return;
        UIEventManager.OnClickDownEvent(m_value);  
    }
}

