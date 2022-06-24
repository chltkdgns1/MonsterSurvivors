using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    static public GoogleLogin instance = null;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
    }

    public async void StartGoogleLogin()
    {
        string sId = GoogleManagers.instance.GetId();

        if (await IsRegisted(sId) == false)
            await WriteBasicData(sId);

        await ReadData(sId);
        GoogleManagers.instance.SetCheckEndLogin(true);
    }

    async Task<bool> IsRegisted(string sId)
    {
        ParmaterPackage pId = new ParmaterPackage("");
        await FirebaseManager.instance.ReadDataValueASync("Users/" + sId + "/NickName", pId);
        if (pId.m_String == "" || pId.m_String.Length == 0) return false;
        return true;
    }

    async Task WriteBasicData(string sId)
    {
        // 기존에 등록할 데이터들 여기다가 넣으면 됨.
        // 현재는 닉네임밖에 없음.

        string sTempId = Module.GetHaxString();
        await FirebaseManager.instance.PushDataASync("Users/" + sId + "/NickName", sTempId);
    }

    async Task ReadData(string sId) // 데이터를 읽어들임.
    {
        ParamterDicPackage pDicPackage = new ParamterDicPackage();
        await FirebaseManager.instance.ReadDataASync("Users/" + sId + "/NickName", pDicPackage);
        DataManage.DataManager.instance.SetNickName((string)pDicPackage.m_Dictionary["NickName"]);
    }

}
