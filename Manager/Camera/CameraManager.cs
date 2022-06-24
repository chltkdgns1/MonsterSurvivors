using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance = null;
    private GameObject m_obCameraFollow = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool Register(GameObject ob)
    {
        if (m_obCameraFollow) return false;
        m_obCameraFollow = ob;
        return true;
    }

    private void Update()
    {
        if(m_obCameraFollow != null)
        {
            transform.position = new Vector3(m_obCameraFollow.transform.position.x, m_obCameraFollow.transform.position.y, transform.position.z);
        }
    }
}
