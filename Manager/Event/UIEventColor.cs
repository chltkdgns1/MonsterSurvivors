using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventColor : UIEvent
{
    // Start is called before the first frame update

    [SerializeField]
    protected Color m_ClickColor;
    [SerializeField]
    protected Color m_NormalColor;

    private Image m_Image = null;

    private void Awake()
    {
        m_obParent = null;
        if (m_bSendObject) m_obMine = gameObject;
        initCommunicateValue();
        Init();
    }

    void Init()
    {
        m_Image = GetComponent<Image>();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        SetColor(true);
        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        SetColor(false);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    void SetColor(bool flag)
    {
        if (m_Image == null) return;
        if (flag == false)  m_Image.color = m_ClickColor;
        else                m_Image.color = m_NormalColor;
    }
}
