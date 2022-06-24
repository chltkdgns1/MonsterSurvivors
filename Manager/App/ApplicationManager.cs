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
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    static IEnumerator QuitAndMessageApp(float fWaitTime,string sMessage)           // 종료의 절반의 시간만 메세지 출력
    {
        yield return new WaitForSeconds(fWaitTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
