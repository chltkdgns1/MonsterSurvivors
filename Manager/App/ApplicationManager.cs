using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public ApplicationManager instance = null;

    
    void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 120;
#else
        Application.targetFrameRate = 120;
#endif
        if (instance == null)   instance = this;     
        else                    Destroy(gameObject);
    }

    public void StartQuitApp(float fWaitTime)
    {
        StartCoroutine(QuitApp(fWaitTime));
    }

    public void StartQuitAndMessageApp(float fWaitTime, string sMessage)
    {
        StartCoroutine(QuitAndMessageApp(fWaitTime, sMessage));
    }

    static IEnumerator QuitApp(float fWaitTime)
    {
        yield return new WaitForSeconds(fWaitTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }

    static IEnumerator QuitAndMessageApp(float fWaitTime,string sMessage)           // ������ ������ �ð��� �޼��� ���
    {
        yield return new WaitForSeconds(fWaitTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
