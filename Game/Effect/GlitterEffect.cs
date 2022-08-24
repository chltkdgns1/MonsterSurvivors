using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterEffect : MonoBehaviour, IEffect
{
    private SpriteRenderer m_SpriteRenderer;
    private Color m_NomalColor;
    [SerializeField]
    private Color m_EffectColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    void OnEnable()
    {
        Register();
    }

    private void OnDisable()
    {
        UnRegister();
    }

    public void Register()
    {
        if (GlitterEffectManager.instance == null) return;
        GlitterEffectManager.instance.Register(this);
    }

    public void UnRegister()
    {
        if (GlitterEffectManager.instance == null) return;
        GlitterEffectManager.instance.UnRegister(this);
    }

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_NomalColor = m_SpriteRenderer.material.color;
    }

    void Start()
    {
        //Register();
    }

    public void On()
    {
        m_SpriteRenderer.material.color = m_EffectColor;
    }
    public void Off()
    {
        m_SpriteRenderer.material.color = m_NomalColor;
    }
}
