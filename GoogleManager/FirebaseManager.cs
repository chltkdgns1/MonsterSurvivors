using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Unity;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ParmaterPackage    // ref �� �񵿱⿡���� ����� �� ����.
{
    public string m_String;
    public ParmaterPackage(string str) { m_String = str; }
}

public class ParamterDicPackage
{
    public IDictionary<string, object> m_Dictionary;
    public ParamterDicPackage() { m_Dictionary = new Dictionary<string, object>(); }
}

public class FirebaseManager : MonoBehaviour
{
    static public FirebaseManager instance = null;
    private DatabaseReference reference;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public DatabaseReference GetRootReference()
    {
        return reference;
    }

    public string GetID()
    {
        return GoogleManagers.instance.GetId();
    }

    public void PushData(string path, string data)
    {
        GetRootReference().Child(path).SetValueAsync(data);
    }

    public async Task PushDataASync(string path, string data)
    {
        await GetRootReference().Child(path).SetValueAsync(data);
    }

    public void PushDataJson(string path, string sJsonData)
    {
        GetRootReference().Child(path).SetRawJsonValueAsync(sJsonData);
    }

    public async Task PushDataJsonASync(string path, string sJsonData)
    {
        await GetRootReference().Child(path).SetRawJsonValueAsync(sJsonData);
    }


    public void PushTransectionSingle(string path, string sData)
    {
        GetRootReference().Child(path).RunTransaction(mutableData => {
            mutableData.Value = sData;
            return TransactionResult.Success(mutableData); // ���� �����մϴ�.
        });
    }

    public async Task PushTransectionSingleASync(string path, string sData)
    {
        await GetRootReference().Child(path).RunTransaction(mutableData => {
            mutableData.Value = sData;
            return TransactionResult.Success(mutableData); // ���� �����մϴ�.
        });
    }

    public void PushGoogleId()
    {
        GetRootReference().Child("User").SetValueAsync(GetID());
        return;
    }

   
    public void ReadDataValue(string path, ParmaterPackage data)
    {
        GetRootReference().Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                ApplicationManager.instance.StartQuitAndMessageApp(3.0f, "���ͳ� ������ �Ҿ����մϴ�.");
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null || snapshot.Exists == false) return;
                data.m_String = snapshot.Key.ToString();
            }
        });
        return;
    }

    public async Task ReadDataValueASync(string path, ParmaterPackage data)
    {
        await GetRootReference().Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted) return;
            
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null || snapshot.Exists == false) return;          
                data.m_String = snapshot.Key.ToString();
            }
        });
        return;
    }

    public void ReadData(string path, ParamterDicPackage data)
    {
        GetRootReference().Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted) return;
            
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null || snapshot.Exists == false) return;

                object value = snapshot.Value;

                if(null != (value as IDictionary))  data.m_Dictionary = (IDictionary<string, object>)snapshot.Value;               
                else                                data.m_Dictionary.Add(snapshot.Key, snapshot.Value);
            }
        });
        return;
    }

    public async Task ReadDataASync(string path, ParamterDicPackage data)
    {
        await GetRootReference().Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)  return;
         
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null || snapshot.Exists == false) return;

                object value = snapshot.Value;

                if (null != (value as IDictionary)) data.m_Dictionary = (IDictionary<string, object>)snapshot.Value;
                else                                data.m_Dictionary.Add(snapshot.Key, snapshot.Value);
            }
        });
        return;
    }

    //public void PushData(string path, SerializableInterface data)
    //{
    //    GetRootReference().Child("User/" + GetID() + "/" + path).SetRawJsonValueAsync(data.GetJsonString());    
    //    return;
    //}

    //public async Task PushDataASync(string path, SerializableInterface data)
    //{
    //    await GetRootReference().Child("User/" + GetID() + "/" + path).SetRawJsonValueAsync(data.GetJsonString());
    //    return;
    //}

    //public void IsSignUp()
    //{
    //    if (GetID().Length == DefineManager.EMPTY)
    //    {
    //        ApplicationManager.instance.StartQuitAndMessageApp(3.0f, "���ͳ� ������ �Ҿ����մϴ�.");
    //        return;
    //    }

    //    GetRootReference().Child("User/" + GetID()).GetValueAsync().ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            ApplicationManager.instance.StartQuitAndMessageApp(3.0f, "���ͳ� ������ �Ҿ����մϴ�.");
    //        }
    //        else if (task.IsCompleted)
    //        {

    //            DataSnapshot snapshot = task.Result;
    //            if (snapshot.Exists)    m_bIsSignUp = true;
    //            else                    m_bIsSignUp = false;
    //        }

    //        m_bFinish = true;
    //    });
    //    return;
    //}

    //public async Task IsSignUpASnyc()
    //{
    //    if (GetID().Length == DefineManager.EMPTY)
    //    {
    //        ApplicationManager.instance.StartQuitAndMessageApp(3.0f, "���ͳ� ������ �Ҿ����մϴ�.");
    //        return;
    //    }

    //    await GetRootReference().Child("User/" + GetID()).GetValueAsync().ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            ApplicationManager.instance.StartQuitAndMessageApp(3.0f, "���ͳ� ������ �Ҿ����մϴ�.");
    //            m_bIsSignUp = false;
    //        }
    //        else if (task.IsCompleted)
    //        {

    //            DataSnapshot snapshot = task.Result;
    //            if (snapshot.Exists) m_bIsSignUp = true;
    //            else m_bIsSignUp = false;
    //        }
    //    });
    //    return;
    //}

    //public async Task CapsuleLogin()
    //{
    //    m_nEndCount = false;
    //    await IsSignUpASnyc();
        
    //    if (GetSignUp() == false)                                                                    //ȸ������ ó���̶��,
    //    {                                                                                            //���� ���̵� ����ϰ�
    //        await PushDataASync("CashData", DataManager.GetCashData());                              //���̾�, ĳ�� ������ ���
    //        await PushDataASync("PlayData", DataManager.GetPlayData());                             //�α��� ������
    //        await PushDataASync("UserData", DataManager.GetUserData());                              //���� �÷��� ������
    //        await PushDataASync("LoginData", DataManager.GetLoginData());                             //����������
    //        await PushDataASync("ADData", DataManager.GetADData());                                  //����������
    //    }

    //    await UpdateData();
    //    m_nEndCount = true;
    //    GoogleManagers.instance.SetLoginStatue(DefineManager.GOOGLE_LOGIN_SUCCESS); 
    //}

    //public async Task UpdateData()
    //{
    //    m_nEndCount = false;
    //    await ReadCashDataASync();
    //    await ReadPlayDataASync(); //    Lock();     
    //    await ReadLoginDataASync(); //   Lock();   
    //    await ReadUserDataASync(); //    Lock();       
    //    await ReadADDataASync();   //    Lock();
    //    m_nEndCount = true;
    //}


    //public IEnumerator UpdateDataCoroutin()
    //{
    //    //yield return null;
    //    m_nEndCount = false;
    //    ReadCashData();
    //    while (!m_bFinish) yield return null;
    //    ReadPlayData(); //    Lock();
    //    while (!m_bFinish) yield return null;
    //    ReadLoginData(); //   Lock();
    //    while (!m_bFinish) yield return null;
    //    ReadUserData(); //    Lock();
    //    while (!m_bFinish) yield return null;
    //    ReadADData();   //    Lock();
    //    while (!m_bFinish) yield return null;
    //    m_nEndCount = true;
    //}
}
