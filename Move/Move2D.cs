using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update

    private Rigidbody2D m_2DRigidbody;

    private float m_fAgoDistance;   // ���� �Ÿ�

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
    //    // Move ��ũ��Ʈ�� ����ϴ� ������Ʈ���� target Position �� �����Ͽ� �̵���
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

    //    if (m_fAgoDistance > dis) m_fAgoDistance = dis;     // Ÿ�ٰ��� ���� �Ÿ��� ���� �Ÿ����� Ŀ���ٸ�, ������ ���� �ǹ���.

    //    else                                                // Ÿ�� �Ÿ��� �������� �־����ٸ�, Ÿ�� ��ġ�� �����ƴٴ� ����.
    //    {
    //        m_fAgoDistance = 1e5f;                          // Ÿ�ٰ��� ���� �Ÿ� �ʱ�ȭ ��, ������ ����.
    //        return false;
    //    }

    //    if (dis >= fMargin)                                 // Ÿ�ٰ� ������ ���� ��ŭ�� ���̰� �����Ѵٸ� �̵��Ѵ�.
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
        if (dis >= fMargin) // ���̰� ���� �ִٸ�
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
        if (dis >= fMargin) // ���̰� ���� �ִٸ�
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
