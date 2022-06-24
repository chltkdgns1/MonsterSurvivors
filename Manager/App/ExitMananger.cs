using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMananger : MonoBehaviour, IAndroidKey
{
    private bool m_bEscape;

    [SerializeField]
    private GameObject m_obEscape;

    [SerializeField]
    private float m_fTimeLength;

    private void Awake()
    {
        m_bEscape = false;
    }
    void Start()
    {
        AndroidKeyManager.instance.RegisterEvent(this);
        m_obEscape.SetActive(false);
    }

    // Update is called once per frame
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
        //Debug.Log("여기 들어옴?");
        if (m_bEscape)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 어플리케이션 종료
#endif
        }
        StartCoroutine(ExEscape());
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

    IEnumerator ExEscape()
    {
        m_obEscape.SetActive(true);
        m_bEscape = true;
        yield return new WaitForSeconds(m_fTimeLength);      
        m_bEscape = false;
        m_obEscape.SetActive(false);
    }
}
