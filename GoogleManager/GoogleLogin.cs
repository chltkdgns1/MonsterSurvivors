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
        // ������ ����� �����͵� ����ٰ� ������ ��.
        // ����� �г��ӹۿ� ����.

        string sTempId = Module.GetHaxString();
        await FirebaseManager.instance.PushDataASync("Users/" + sId + "/NickName", sTempId);
    }

    async Task ReadData(string sId) // �����͸� �о����.
    {
        ParamterDicPackage pDicPackage = new ParamterDicPackage();
        await FirebaseManager.instance.ReadDataASync("Users/" + sId + "/NickName", pDicPackage);
        DataManage.DataManager.instance.SetNickName((string)pDicPackage.m_Dictionary["NickName"]);
    }

}
