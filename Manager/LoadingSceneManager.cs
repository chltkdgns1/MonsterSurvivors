using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager instance = null;

    public static string m_sSceneName;
    private float m_fTimer = 2.0f;

    void Awake()
    {
        if(instance == null)    instance = this;       
        else                    Destroy(gameObject);      
    }

    public void SetSceneName(string name,float time, bool flag = false) // ¾À ÀÌ¸§, Å¸ÀÌ¸Ó, ±¤°í Ãâ·Â
    {
        //Debug.Log("SetSceneName Start");
        m_sSceneName = name;
        m_fTimer = time;
        StartCoroutine(LoadScene(flag));
    }

    // Update is called once per frame
    IEnumerator LoadScene(bool adFlag)
    {
        //Debug.Log("    IEnumerator LoadScene(bool adFlag) Start");
        AsyncOperation op = SceneManager.LoadSceneAsync(m_sSceneName);

        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= m_fTimer)
            {            
                if (op.progress >= 0.9f)
                {
                    //Debug.Log("op.progress : " + op.progress);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
