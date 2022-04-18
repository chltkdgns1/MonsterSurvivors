using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update

    private Rigidbody2D m_2DRigidbody;

    private float m_fAgoDistance;   // 이전 거리

    private Vector3 m_targetPosition;

    [SerializeField]
    private bool m_fMoveVersion = false;

    private void Awake()
    {
        m_fAgoDistance = 1e5f;
        m_targetPosition = transform.position;
    }
    void Start()
    {
        m_2DRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Move 스크립트를 사용하는 오브젝트에서 target Position 을 갱신하여 이동함
    //}

    public void SetTargetPosition(Vector3 target)
    {
        m_targetPosition = target;
    }

    public Vector3 GetTargetPosition()
    {
        return m_targetPosition;
    }

    public void SetAgoDistance(float dist)
    {
        m_fAgoDistance = dist;
    }

    //public bool RunRigid(Vector3 targetPos, float speed, float fMargin = 0.0001f)
    //{
    //    targetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    //    float dis = Vector3.Distance(transform.position, targetPos);

    //    if (m_fAgoDistance > dis) m_fAgoDistance = dis;     // 타겟과의 이전 거리가 현재 거리보다 커졌다면, 가까진 것을 의미함.

    //    else                                                // 타겟 거리가 이전보다 멀어졌다면, 타켓 위치를 지나쳤다는 뜻임.
    //    {
    //        m_fAgoDistance = 1e5f;                          // 타겟과의 이전 거리 초기화 후, 움직임 멈춤.
    //        return false;
    //    }

    //    if (dis >= fMargin)                                 // 타겟과 정해진 마진 만큼의 차이가 존재한다면 이동한다.
    //    {
    //        Vector2 temp = (targetPos - transform.position).normalized;
    //        m_2DRigidbody.MovePosition(m_2DRigidbody.position + temp * speed);           
    //        return true;
    //    }
    //    return false;
    //}


    public bool RunRigid(Vector3 targetPos, float speed, float fMargin = 0.0001f)
    {
        targetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        float dis = Vector3.Distance(transform.position, targetPos);

        if (m_fAgoDistance > dis) m_fAgoDistance = dis;

        else
        {
            m_fAgoDistance = 1e5f;
            return false;
        }
        if (dis >= fMargin) // 차이가 아직 있다면
        {
            if (m_fMoveVersion == false)
            {
                transform.Translate((targetPos - transform.position) * speed * Time.deltaTime);
            }

            else
            {
                Vector2 temp = (targetPos - transform.position).normalized;
                m_2DRigidbody.MovePosition(m_2DRigidbody.position + temp * speed);
            }
            return true;
        }
        return false;
    }


    public bool Run(Vector3 targetPos, float speed, float fMargin = 0.0001f)
    {
        targetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        float dis = Vector3.Distance(transform.position, targetPos);

        if (m_fAgoDistance > dis) m_fAgoDistance = dis;

        else
        {
            m_fAgoDistance = 1e5f;
            return false;
        }
        if (dis >= fMargin) // 차이가 아직 있다면
        {
            if (m_fMoveVersion == false)
            {
                transform.Translate((targetPos - transform.position) * speed * Time.deltaTime);
            }

            else
            {
                Vector3 temp = (targetPos - transform.position).normalized;
                transform.position += temp * speed;
            }
            return true;
        }
        return false;
    }
}
