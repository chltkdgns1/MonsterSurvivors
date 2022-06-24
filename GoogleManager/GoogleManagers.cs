using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;
using System.Threading.Tasks;

public class GoogleManagers : MonoBehaviour
{
    static public GoogleManagers instance = null;
    private bool m_bCheckEndLogin = false;

    void Awake()
    {
        if (instance == null)   instance = this;      
        else                    Destroy(gameObject);

        //PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        DontDestroyOnLoad(gameObject);
    }


    public void StartGoogleLogin() // 구글 플레이 서비스 로그인
    {
        Debug.Log("여기 실행은 되지??");
        Social.localUser.Authenticate((bool success) =>
        {

#if UNITY_EDITOR
            success = true;
#else
        return Social.localUser.authenticated;
#endif
            if (success == true)
                GoogleLogin.instance.StartGoogleLogin();
        });      
    }

   
    public string GetId()
    {
#if UNITY_EDITOR
        return "g13527977510020503525";
#else
        return Social.localUser.id;
#endif
    }

    public bool CheckLogin()
    {
#if UNITY_EDITOR
        return true;
#else
        return Social.localUser.authenticated;
#endif
    }

    public void SetCheckEndLogin(bool bLogin) {
        m_bCheckEndLogin = bLogin;
    }

    public bool CheckLoginStateEnd()
    {
        return m_bCheckEndLogin;
    }
}
