using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidKeyUIManager : MonoBehaviour, IAndroidKey
{
    public static AndroidKeyUIManager instance = null;
    private bool m_bEscape;

    void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        init();
    }

    void init()
    {
        m_bEscape = false;
    }

    void Start()
    {
        AndroidKeyManager.instance.RegisterEvent(this);
    }

    public void OnClickHome()
    {

    }
    public void OnClickEscape()
    {
    
    }
    public void OnClickMenu()
    {

    }
    public void OnClickHomeDown()
    {

    }
    public void OnClickHomeUp()
    {

    }
    public void OnClickEscapeDown()
    {
        if (m_bEscape)  GameUIManager.instance.SetActivePauseBackScreen(false);
        else            GameUIManager.instance.SetActivePauseBackScreen(true);
    }
    public void OnClickEscapeUp()
    {

    }
    public void OnClickMenuDown()
    {

    }
    public void OnClickMenuUp()
    {

    }

    public void SetEscape(bool flag) { m_bEscape = flag; }
}
