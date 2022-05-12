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

    private void Start()
    {
        
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
            if(success == true) LoginSequence();

        });      
    }

    async void LoginSequence()
    {
        string sId = GetId();

        if(await IsRegisted(sId) == false)
            await WriteBasicData();

        await ReadData();

        m_bCheckEndLogin = true;
    }

    async Task<bool> IsRegisted(string sId)
    {
        ParmaterPackage pId = new ParmaterPackage("");
        await FirebaseManager.instance.ReadDataValueASync("Users/" + GetId() + "/NickName", pId);
        if (pId.m_String == "" || pId.m_String.Length == 0) return false;
        return true;
    }

    async Task WriteBasicData()
    {
        // 기존에 등록할 데이터들 여기다가 넣으면 됨.
        // 현재는 닉네임밖에 없음.
        string sTempId = Module.GetHaxString();
        await FirebaseManager.instance.PushDataASync("Users/" + GetId() + "/NickName", sTempId);


    }

    async Task ReadData() // 데이터를 읽어들임.
    {
        ParamterDicPackage pDicPackage = new ParamterDicPackage();
        await FirebaseManager.instance.ReadDataASync("Users/" + GetId() + "/NickName", pDicPackage);
        ValueManager.instance.SetNickName((string)pDicPackage.m_Dictionary["NickName"]);
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

    public bool CheckLoginStateEnd()
    {
        return m_bCheckEndLogin;
    }
}
