using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // Start is called before the first frame update

    private bool m_bInitEnd = false;
    private Vector3 m_Init = new Vector3(0, 1, 0);
    private Vector3 m_vStartPosition;

    private int m_nFollowOption = 0;

    private Vector3 m_vDirection;
    private GameObject m_ObTarget = null;

    private int m_nSkillManagerIndex;

    private float m_fAngle = 0f;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.position = new Vector3(PlayerOffline2D.instance.transform.position.x + m_vStartPosition.x, 
            PlayerOffline2D.instance.transform.position.y + m_vStartPosition.y, transform.position.z);

        if (m_nFollowOption == 1)
        {
            GameObject temp = PlayerOffline2D.instance.GetNearestObject();

            if(temp == null)    m_vDirection = new Vector3(1, 0, 0);           
            else                m_vDirection = (temp.transform.position - transform.position).normalized;
        }
    }

    public void SetInit(int nSkillManagerIndex, float speed, float fMinDamage, float fMaxDamage, Vector2 vLocalScale, Vector3 StartPosition, int nFollowOption) {   // flag 는 유도 기능임.
        transform.localScale    = vLocalScale;
        m_nFollowOption         = nFollowOption;
        m_nSkillManagerIndex    = nSkillManagerIndex;

        if (StartPosition != new Vector3(0, 0, 0))
        {
            StartPosition = new Vector3(StartPosition.x, StartPosition.y, 0);
            transform.Rotate(new Vector3(0, 0, Module.GetAngle(m_Init, StartPosition)));
        }
        
        m_vStartPosition        = StartPosition;
        m_bInitEnd              = true;
    }

    public void SetLocalScale(Vector2 vLocalScale)
    {
        transform.localScale = vLocalScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;

        if (m_bInitEnd == false) return;

        float fSpeed = SkillManager.instance.GetSpeed(m_nSkillManagerIndex); 

        if (m_nFollowOption == 1)
        {
            transform.position += m_vStartPosition.normalized * fSpeed;
        }

        else if (m_nFollowOption == 2)
        {
            m_ObTarget = PlayerOffline2D.instance.GetNearestObject();

            Vector3 playerPosition = PlayerOffline2D.instance.transform.position;

            if (PlayerOffline2D.instance.GetNearDist() >= 2f)
            {
                m_fAngle += 3f;
                if (m_fAngle >= 360f) m_fAngle -= 360f;

                Vector2 CyclePos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * m_fAngle), Mathf.Sin(Mathf.Deg2Rad * m_fAngle));
                transform.position = new Vector3(playerPosition.x + CyclePos.x, playerPosition.y + CyclePos.y, transform.position.z);
            }
            else
            {
                Vector3 tempVec = (m_ObTarget.transform.position - transform.position).normalized * fSpeed;
                transform.position += new Vector3(tempVec.x, tempVec.y, 0);
            }
        }
        else transform.position = m_vStartPosition + new Vector3(PlayerOffline2D.instance.transform.position.x, PlayerOffline2D.instance.transform.position.y,transform.position.z);      
    }

    public float GetDamage()
    {
        return SkillManager.instance.GetDamage(m_nSkillManagerIndex);
        //return Random.Range(m_fMinDamage, m_fMaxDamage);
    }

    public float GetBlood()
    {
        return SkillManager.instance.GetBlood(m_nSkillManagerIndex);
    }

    public float GetNuckBack()
    {
        return SkillManager.instance.GetNuckBack(m_nSkillManagerIndex);
    }

    public void SetDamageSum(float fDamage)
    {
        SkillManager.instance.SetDamageSum(m_nSkillManagerIndex, fDamage);
    }


}
