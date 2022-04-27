using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Throw
{
    void SetTargetObject(GameObject Ob);
    void SetThrowTime(float fTime);
    float GetDamage();
}


public class ThrowObject : MonoBehaviour, Throw
{

    [SerializeField]
    private float m_fDamage;

    private GameObject m_TargetObject;
    private float m_fThrowTime;
    private float m_fRemainThrowTime;

    private bool m_bInit = false;
    private Vector3 m_vDir = new Vector3(0, 1, 0);
    private Vector3 m_vTargetPosition;
    private Vector3 m_vTargetDir;

    private Move2D m_Move2d;

    [SerializeField]
    private float m_fSpeed;

    private void Awake()
    {
        m_Move2d = GetComponent<Move2D>();
    }
    public void SetThrowTime(float fTime)
    {
        m_fThrowTime = fTime;
    }

    public void SetTargetObject(GameObject Ob)
    {
        m_TargetObject = Ob;
    }

    public float GetDamage()
    {
        return m_fDamage;
    }

    private void OnEnable()
    {
        if (m_bInit == true)
        {
            m_fRemainThrowTime = m_fThrowTime;
            m_vTargetPosition = m_TargetObject.transform.position;
            float fAngle = Module.GetAngle(m_vDir, m_vTargetPosition);
            transform.Rotate(new Vector3(0, 0, fAngle));
            m_vTargetDir = (m_TargetObject.transform.position - transform.position).normalized;
        }
    }
    private void Start()
    {
        m_bInit = true;
        m_fRemainThrowTime = m_fThrowTime;
        m_vTargetPosition = m_TargetObject.transform.position;
        float fAngle = Module.GetAngle(m_vDir, m_vTargetPosition);
        transform.Rotate(new Vector3(0, 0, fAngle));
        m_vTargetDir = (m_TargetObject.transform.position - transform.position).normalized; 
    }

    // Update is called once per frame
    void Update()
    {
        m_fRemainThrowTime -= Time.deltaTime;
        m_Move2d.RunRigidDir(m_vTargetDir, m_fSpeed);
        if (m_fRemainThrowTime <= 0f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_TargetObject)
        {        
            gameObject.SetActive(false);
            // 데미지를 준다.
        }
    }




}
