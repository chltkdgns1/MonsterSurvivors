using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Text m_txtLoadingPercent;
    [SerializeField]
    private Image m_imgProgress;
    [SerializeField]
    private GameObject m_ObLoginSuccess;

    private GameObject m_obDisconnect;
    private Text m_obDialMessage;

    private float sumTime;
    private int pointCnt;
    private string loadingStr;

    private bool checkPointCnt;
    private int strMinSize = 7;
    private int strMaxSize = 12;

    private float sumForNextScene;
    private float timeGap;

    private int m_bEndGoogleLogin;

    private int m_nWaitSecond = 30;

    private string[] m_sMessageFormat;

    [SerializeField]
    private string m_sNextScene;

    private void Awake()
    {
        sumTime = 0.0f;
        pointCnt = 0;
        loadingStr = "Loading";
        checkPointCnt = false;
        timeGap = 0.3f;

        m_nWaitSecond *= 10000000;

        //Screen.orientation = ScreenOrientation.Portrait;
    }
    void Start()
    { 
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        sumTime += Time.deltaTime;
        sumForNextScene += Time.deltaTime;

        if (sumTime >= timeGap)
        {
            sumTime = 0.0f;
            if(checkPointCnt == false)
            {
                loadingStr = loadingStr.Insert(loadingStr.Length, ".");
            }
            else
            {
                loadingStr = loadingStr.Remove(loadingStr.Length - 1);
            }
            if (loadingStr.Length == strMaxSize || loadingStr.Length == strMinSize) checkPointCnt = !checkPointCnt;
            loadingText.text = loadingStr;
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(m_sNextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        m_imgProgress.fillAmount = 0.0f;

        //while (waitTime < 1.0f)
        //{
        //    waitTime += Time.deltaTime;
        //    m_imgProgress.fillAmount = Mathf.Lerp(m_imgProgress.fillAmount, op.progress, Time.deltaTime);

        //    if (m_imgProgress.fillAmount <= timer) timer = 0f;

        //    int percent = (int)(m_imgProgress.fillAmount * 100f);
        //    m_txtLoadingPercent.text = percent.ToString() + "%";
        //    yield return null;
        //}

        bool bLogPath = false;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (bLogPath == true)
            {
                if (op.progress > 0.8f)
                {
                    if (m_imgProgress.fillAmount >= 1.0f && OptionManager.instance.GetAllLoad() && GoogleManagers.instance.CheckLoginStateEnd())
                    {
                        op.allowSceneActivation = true;
                        break;
                    }
                }
            }

            if (bLogPath == false)
            {
                GoogleManagers.instance.StartGoogleLogin();    
                bLogPath = true;
            }

            m_imgProgress.fillAmount += Time.deltaTime / 3; 
            int percent = (int)(m_imgProgress.fillAmount * 100f);
            m_txtLoadingPercent.text = percent.ToString() + "%";
        }
    }
}
