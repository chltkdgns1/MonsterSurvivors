using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IAndroidKey
{
    void OnClickHome();
    void OnClickEscape();
    void OnClickMenu();
    void OnClickHomeDown();
    void OnClickHomeUp();
    void OnClickEscapeDown();
    void OnClickEscapeUp();
    void OnClickMenuDown();
    void OnClickMenuUp();
}

public class AndroidKeyManager : MonoBehaviour   // ManagerEvent.cs Âü°í
{
    public static AndroidKeyManager instance;
    public List<IAndroidKey> m_obList = new List<IAndroidKey>();

    private void Awake()
    {
        if(instance == null)    instance = this;   
        else                    Destroy(gameObject);
    }

    public void RegisterEvent(IAndroidKey events)
    {
        m_obList.Add(events);
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            int sz = m_obList.Count;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 0; i < sz; i++)    m_obList[i].OnClickEscapeDown();               
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                for (int i = 0; i < sz; i++)    m_obList[i].OnClickEscapeUp();           
            }
        }
    }
}

