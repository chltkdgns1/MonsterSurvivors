using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public NetworkManager instance = null;
    private bool m_bDisconnect = false;

    [SerializeField]
    private bool m_bPassedNet = false;

    //private bool m_bConnectChange = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public bool GetConnect()
    {
        return !m_bDisconnect;
    }

    private void Start()
    {
 
    }

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)                        // ����� ������ �ȵ� ����
        {
            if (m_bDisconnect) return;
            m_bDisconnect = true;

            //if (m_bPassedNet == false)
            //{
            //    StartCoroutine(DisconnectApplicationQuit());
            //}
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) // ������       
            m_bDisconnect = false;     
        else
            m_bDisconnect = false;                                                                                            // wifi      
    }

    public void SetPassedNet(bool flag)
    {
        m_bPassedNet = flag;
    }

    IEnumerator DisconnectApplicationQuit()
    {
        yield return new WaitForSeconds(3.0f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
