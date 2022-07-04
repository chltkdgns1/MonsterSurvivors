using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance = null;
    private GameObject m_obCameraFollow = null;
    private bool m_bFollowFlag;
    private Move2D m_CompMove2D;
    private Vector3 ?m_vDir;

    public bool FollowFlag
    {
        get { return m_bFollowFlag; }
        set { m_bFollowFlag = value; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        m_CompMove2D = GetComponent<Move2D>();
        m_vDir = null;
    }

    public bool Register(GameObject ob)
    {
        if (m_obCameraFollow) return false;
        m_obCameraFollow = ob;
        m_bFollowFlag = true;
        return true;
    }

    private void Update()
    {

        if (m_bFollowFlag == false) return;

        if(m_obCameraFollow != null)
        {
            transform.position = new Vector3(m_obCameraFollow.transform.position.x, m_obCameraFollow.transform.position.y, transform.position.z);
        }
    }

    public void MoveCamera(Vector3 vPosition)
    {
        m_CompMove2D.Run(transform.position + new Vector3(vPosition.x, vPosition.y, transform.position.z));
    }
}
