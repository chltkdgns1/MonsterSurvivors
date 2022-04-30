using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillFormat        // 외부에서 사용하기 편한 간단한 스킬 포멧
{
    public int m_nSkill;
    public int m_nType;
    public float m_fValue;

    public SkillFormat(int nSkill,int nType, float fValue)
    {
        m_nSkill    = nSkill;
        m_nType     = nType;
        m_fValue    = fValue;
    }
}

public class SkillManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public SkillManager instance = null;

    public class SkillStruct            // 하나의 스킬을 의미함
    {
        public bool m_bSkillUse;
        public List<GameObject> m_Skill = new List<GameObject>();
        public SkillStruct() { m_bSkillUse = false; }
        public void Add(GameObject ObSkill) { m_Skill.Add(ObSkill); }
        public IEnumerator ExSkillTimer(float fSkillTime)
        {
            for (int i = 0; i < m_Skill.Count; i++) m_Skill[i].SetActive(true);
            yield return new WaitForSeconds(fSkillTime);
            for (int i = 0; i < m_Skill.Count; i++) m_Skill[i].SetActive(false);
        }
    }

    public class SkillData                                                 // 하나의 스킬 데이터이지만, 쿨타임에 의해 여러개를 갖고 있어야 할 수도 있음.
    {
        public float                m_fCoolTime;
        public float                m_fBeginCoolTime;
        public float                m_fCoolTimeValue    = 0;   // 줄어든 쿨타임양
        public float                m_fCoolTimePercent  = 0; // 줄어든 쿨타임 퍼센트

        public float                m_fRemainCoolTime;

        public float                m_fSkillContinueTime;       // 지속시간은 상정안함.

        public int                  m_nSkillKey;
        public int                  m_nSkillManagerIndex;
        public int                  m_nUseIndex;

        public float                m_fSpeed;                   // 스피드도 마찬가지
        public float                m_fBeginSpeed;

        public float                m_fMinDamage;
        public float                m_fBeginMinDamage;
        public float                m_fMaxDamage;
        public float                m_fBeginMaxDamage;
        public float                m_fDamageValue      = 0;
        public float                m_fDamagePercent    = 0;

        Vector2                     m_vLocalScale;
        Vector2                     m_vBeginLocalScale;
        public float                m_fLocalSize        = 0;
        public float                m_fLocalSizePercent = 0;

        public float                m_fNuckBack         ;
        public float                m_fBeginNuckBack    ;
        public float                m_fNuckBackPercent  = 0;

        public float                m_fBloodPercent     = 0;

        public int                  m_nFollowOption      = 0;

        public float                m_fSumDamage;
        public float                m_fMaxDPS;
        public float                m_fDPS;

        // 나중에 Skill SetInit 에 추가해야함. 귀찮아서 지금은 추가 안함.

        public List<SkillStruct>    m_SkillStruct = new List<SkillStruct>();

        public int                  m_nCnt;

        public SkillData(
            float fCoolTime,    float fSkillContinueTime,   int nSkillKey,      int nCnt, 
            int nMinDamage,     int nMaxDamage,             float fSpeed,       GameObject ObSkill, 
            int nFollow,        int nSkillManagerIndex,     float fNuckBack,    float fBlood                                 ) // 새로운 스킬 생성
        {
            m_nCnt                  = 1;
            m_fSkillContinueTime    = fSkillContinueTime;

            m_nSkillKey             = nSkillKey;
            m_nSkillManagerIndex    = nSkillManagerIndex;
            m_nUseIndex             = 0;

            m_fSpeed                = fSpeed;

            m_fBeginMinDamage       = nMinDamage;
            m_fBeginMaxDamage       = nMaxDamage;
            m_fBeginCoolTime        = fCoolTime;
            m_fRemainCoolTime       = m_fBeginCoolTime;

            m_fCoolTime             = m_fBeginCoolTime;
            m_fMinDamage            = m_fBeginMinDamage;
            m_fMaxDamage            = m_fBeginMaxDamage;


            m_fNuckBack             = fNuckBack; 
            m_fBloodPercent         = fBlood;

            m_nFollowOption         = nFollow;

            m_fSumDamage            = 0f;
            m_fMaxDPS               = 0f;
            m_fDPS                  = 0f;

            for (int i = 0; i < nCnt; i++)                  // 새로운 스킬 생성하면서, 새로운 스킬이 동시에 nCnt 개 만큼 존재함.
            {
                GameObject ObNewSkill;

                //if (OptionManager.instance.GetFollowPlayerSkill(nSkillKey))
                //    ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation, PlayerOffline2D.instance.gameObject.transform);
                //else
                //    ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation);

                ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation);
                ObNewSkill.SetActive(false);

                m_vBeginLocalScale  = ObNewSkill.transform.localScale;
                m_vLocalScale       = m_vBeginLocalScale;

                Skill temp = ObNewSkill.GetComponent<Skill>();

                Vector3 vStartPosition = OptionManager.instance.GetCreatePosition(m_nSkillKey, m_nCnt - 1);

                temp.SetInit(m_nSkillManagerIndex, m_fSpeed, m_fBeginMinDamage, m_fBeginMaxDamage, m_vBeginLocalScale, vStartPosition * (m_vLocalScale.x), m_nFollowOption);

                m_SkillStruct.Add(new SkillStruct());
                m_SkillStruct[i].Add(ObNewSkill);
            }      
        }

        public void UpNuckBack(float fValue)
        {
            m_fNuckBackPercent += fValue;
            //m_fNuckBack = m_fBeginNuckBack * (1 + m_fNuckBackPercent);
        }

        public void UpBlood(float fValue)
        {
            m_fBloodPercent += fValue;
        }

        public void Add(GameObject ObSkill)
        {
            m_nCnt++;
            int sz = m_SkillStruct.Count;

            for (int i = 0; i < sz; i++)
            {
                GameObject ObNewSkill;

                //if (OptionManager.instance.GetFollowPlayerSkill(m_nSkillKey))
                //    ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation, PlayerOffline2D.instance.gameObject.transform);
                //else
                //    ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation);

                ObNewSkill = Instantiate(ObSkill, ObSkill.transform.position, ObSkill.transform.rotation);
                ObNewSkill.SetActive(false);

                Skill temp = ObNewSkill.GetComponent<Skill>();

                Vector3 vStartPosition = OptionManager.instance.GetCreatePosition(m_nSkillKey, m_nCnt - 1);
                temp.SetInit(m_nSkillManagerIndex, m_fSpeed, m_fMinDamage, m_fMaxDamage, m_vLocalScale, vStartPosition * (m_vLocalScale.x), m_nFollowOption);

                m_SkillStruct[i].Add(ObNewSkill);
            }
        }

        public SkillStruct GetSkillStruct()
        {      
            SkillStruct temp = m_SkillStruct[m_nUseIndex];
            m_nUseIndex++;
            m_nUseIndex %= m_SkillStruct.Count;
            return m_SkillStruct[m_nUseIndex];
        }

        public void UpSize(float fSize, bool flag)    // 크기 증가.
        {
            // flag = 0 pixel , flag = 1 Percent
            if (flag == false)  m_fLocalSize        += fSize;
            else                m_fLocalSizePercent += fSize;
            
            m_vLocalScale = new Vector2((m_vBeginLocalScale.x + m_fLocalSize / 50) * (1 + m_fLocalSizePercent), (m_vBeginLocalScale.y + m_fLocalSize / 50) * (1 + m_fLocalSizePercent));
            UpdateLocalScale();
            //UpdateValue();
        }

        public void UpCount(int index)         // 개수 증가
        {
            GameObject ObPrefSkill = OptionManager.instance.GetSkill(index);
            Add(ObPrefSkill);
        }

        public void UpDamage(float fDamage, bool flag)   // 데미지 증가
        {
            if (flag == false)  m_fDamageValue      += fDamage;
            else                m_fDamagePercent    += fDamage;

            float fMin = m_fBeginMinDamage + m_fDamageValue;
            float fMax = m_fBeginMaxDamage + m_fDamageValue;
            fMin = Mathf.Max(1, fMin);
            fMax = Mathf.Max(1, fMax);

            m_fMinDamage = fMin * (1f + m_fDamagePercent);
            m_fMaxDamage = fMax * (1f + m_fDamagePercent);
            m_fMaxDamage = Mathf.Max(m_fMaxDamage, m_fMinDamage);

            //UpdateValue();
        }

        public void DownCoolTime(float fTime, bool flag)
        {
            if (flag == false)  m_fCoolTimeValue += fTime;
            else                m_fCoolTimePercent += fTime;

            float fCoolTime = Mathf.Max(0.1f, m_fBeginCoolTime - m_fCoolTimeValue);
            m_fCoolTime = (1 - m_fCoolTimePercent) * fCoolTime;

            //UpdateValue();
        }

        public void UpdateLocalScale()
        {
            int sz = m_SkillStruct.Count;
            if (sz == 0) return;
            int inSz = m_SkillStruct[0].m_Skill.Count;

            for (int i = 0; i < sz; i++)
            {
                for (int k = 0; k < inSz; k++)
                {
                    Skill temp = m_SkillStruct[i].m_Skill[k].GetComponent<Skill>();
                    temp.SetLocalScale(m_vLocalScale);
                }
            }
        }

        public void SetMaxDPS()
        {
            m_fMaxDPS = Mathf.Max(m_fMaxDPS, m_fDPS);
            m_fDPS = 0f;
        }
    }

    private List<SkillData> m_SkillData = new List<SkillData>();        // 각각 다른 스킬들의 모음 인덱스 : 0 , 인덱스 : 1 은 다른 스킬임

    private float m_fSecondTimeCheck = 0f;

    private bool m_bFirst = false;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        m_fSecondTimeCheck = 0f;
    }

    void Start()
    {
        
    }

    public SkillStatusStruct GetSkillStatusData(int index)
    {
        if(index < 0 || m_SkillData.Count <= index)
        {
            Debug.LogError("public SkillStatus GetSkillStatusData(int index) cnt Error");
            return new SkillStatusStruct();
        }

        return GetPackingData(index);
    }

    SkillStatusStruct GetPackingData(int index)
    {
        string sSkillName = OptionManager.instance.GetSkillName(m_SkillData[index].m_nSkillKey);
        string sActivate = OptionManager.instance.GetSkillActive(m_SkillData[index].m_nSkillKey) ? "액티브" : "패시브";
        return new SkillStatusStruct(
            m_SkillData[index].m_nSkillKey,         m_SkillData[index].m_fMaxDamage,        m_SkillData[index].m_fCoolTimePercent,  m_SkillData[index].m_fLocalSizePercent,
            m_SkillData[index].m_fNuckBackPercent,  m_SkillData[index].m_fBloodPercent,     m_SkillData[index].m_nCnt,              sActivate,                              sSkillName,
            m_SkillData[index].m_fSumDamage,        m_SkillData[index].m_fMaxDPS);
    }

    public int GetSkillStatusDataCnt()
    {
        return m_SkillData.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.PLAYING_STATE_NO_ENEMY) return;
        if (m_bFirst == false) { m_bFirst = true; RandFirstSkill(); return; }
        if (IsPossibleUseSkill() == false) return;
        SetMaxDPS();
        UseSkill();
    }

    private void SetMaxDPS()
    {
        m_fSecondTimeCheck += Time.deltaTime;
        if (m_fSecondTimeCheck < 1f) return;

        int sz = m_SkillData.Count;
        for (int i = 0; i < sz; i++) m_SkillData[i].SetMaxDPS();
        m_fSecondTimeCheck = 0f;
    }

    private bool IsPossibleUseSkill()
    {
        return PlayerOffline2D.instance.IsPossibleUse();
    }

    public float GetDamage(int index)
    {
        return (int)Random.Range(m_SkillData[index].m_fMinDamage, m_SkillData[index].m_fMaxDamage); 
    }

    public float GetSpeed(int index)
    {
        return m_SkillData[index].m_fSpeed;
    }

    public float GetBlood(int index)
    {
        return m_SkillData[index].m_fBloodPercent;
    }

    public float GetNuckBack(int index)
    {
        return m_SkillData[index].m_fNuckBackPercent;
    }

    public void SetDamageSum(int index, float fDamage)
    {
        m_SkillData[index].m_fSumDamage += fDamage;
        m_SkillData[index].m_fDPS += fDamage;
    }

    void UseSkill()
    {
        int sz = m_SkillData.Count;
        for (int i = 0; i < sz; i++)
        {
            m_SkillData[i].m_fRemainCoolTime -= Time.deltaTime;
            if (m_SkillData[i].m_fRemainCoolTime <= 0f)
            {
                m_SkillData[i].m_fRemainCoolTime = m_SkillData[i].m_fCoolTime;
                SkillStruct temp = m_SkillData[i].GetSkillStruct();
                StartCoroutine(temp.ExSkillTimer(m_SkillData[i].m_fSkillContinueTime));
            }
        }
    }

    void AddSkill(
        int index,          int nKey,           GameObject ObSkill,     float fCoolTime, 
        float fSkillTime,   int nMinDamage,     int nMaxDamage, 
        float fSpeed,       int nFollowOption,  float fNuckBack = 0f,   float fBlood = 0f)
    {

        int nSkillManagerIndex = m_SkillData.Count;
        if (index == -1)    m_SkillData.Add(new SkillData(fCoolTime, fSkillTime, nKey, 20, nMinDamage, nMaxDamage, fSpeed, ObSkill, nFollowOption, nSkillManagerIndex, fNuckBack, fBlood)); 
        else                m_SkillData[index].Add(ObSkill);       
    }

    public bool IsExistSkill(int nKey)
    {
        int sz = m_SkillData.Count;
        for (int i = 0; i < sz; i++)
            if (m_SkillData[i].m_nSkillKey == nKey) return true;
        return false;
    }

    public int GetSkillIndex(int nKey)
    {
        int sz = m_SkillData.Count;
        int index = -1;
        for (int i = 0; i < sz; i++)
            if (m_SkillData[i].m_nSkillKey == nKey) index = i;
        return index;
    }


    public void RandFirstSkill()
    {
        int sz      = OptionManager.instance.GetSkillSize();
        int nSkill  = Random.Range(0, sz);

        int index = GetSkillIndex(nSkill);

        AddSkill(2, index);
    }

    

    public void SkillJudge(int nSkill, int nType, float fValue)
    {
        int index = GetSkillIndex(nSkill);

        if (index == -1)
        {
            AddSkill(nSkill, index);
            return;
        }

        SelectSkill(nSkill, index, nType, fValue);
    }

    void AddSkill(int nSkill, int index)    // 스킬 오브젝트 추가
    {
        GameObject ObPrefSkill = OptionManager.instance.GetSkill(nSkill);

        switch (nSkill)
        {
            case 0:     AddSkill(index, nSkill, ObPrefSkill, 4f, 1f , 2, 5, 0f, 0   );         break;
            case 1:     AddSkill(index, nSkill, ObPrefSkill, 2f, 0.2f, 3, 6, 0f, 0  );         break;
            case 2:     AddSkill(index, nSkill, ObPrefSkill, 2f, 1f, 2, 4, 0.1f, 1  );         break;
            case 3:     AddSkill(index, nSkill, ObPrefSkill, 2f, 1f, 2, 4, 0f, 0    );         break;
            case 4:     AddSkill(index, nSkill, ObPrefSkill, 5f, 3f, 3, 6, 0.2f, 2  );         break;
        }
    }

    void SelectSkill(int nSkill, int index, int type, float fValue) // 스킬 종류, 선택한 옵션, 해당 옵션 증가율
    {
        switch ((DefineManager.SKILL)type)
        {
            case DefineManager.SKILL.SKILL_SIZE_PERCENT         : m_SkillData[index].UpSize(fValue, true);          break;
            case DefineManager.SKILL.SKILL_COOLTIME_PERCENT     : m_SkillData[index].DownCoolTime(fValue, true);    break;
            case DefineManager.SKILL.SKILL_DAMAGE               : m_SkillData[index].UpDamage(fValue, false);       break;
            case DefineManager.SKILL.SKILL_DAMAGE_PERCENT       : m_SkillData[index].UpDamage(fValue, true);        break;
            case DefineManager.SKILL.SKILL_COUNT                : m_SkillData[index].UpCount(nSkill);               break;
            case DefineManager.SKILL.SKILL_NUCKBACK_PERCENT     : m_SkillData[index].UpNuckBack(fValue);            break;
            case DefineManager.SKILL.SKILL_BLOOD_PERCENT        : m_SkillData[index].UpBlood(fValue);               break;
        }
    }

    public void GetRandSkillStatus(ref int nSkill, ref int nType, ref float fValue, List<int> ListMargin = null)
    { 
        int sz = OptionManager.instance.GetSkillSize();
        nSkill = Random.Range(0, sz);

        if(nSkill < DefineManager.ACTIVE_SKILL_SIZE) // 액티브 스킬이라면.
        {
            if (IsExistSkill(nSkill) == false)  nType = (int)DefineManager.SKILL.SKILL_COUNT;      
            else                                nType = Random.Range(0, DefineManager.ACTIVE_SKILL_TYPE_SIZE);

            int nSkillLimitCount    = OptionManager.instance.GetLimitSize(nSkill);
            int nSkillIndex = GetSkillIndex(nSkill);

            int nMarginCount = 0;
            if (ListMargin != null && ListMargin.Count > nSkill)
                nMarginCount = ListMargin[nSkill];

            if (nSkillIndex != -1 || nMarginCount != 0)
            {
                int nSkillCount = 0;
                if(nSkillIndex != -1) nSkillCount = m_SkillData[nSkillIndex].m_nCnt;
              
                if (nSkillCount + nMarginCount >= nSkillLimitCount)
                {
                    while (true)
                    {
                        nType = Random.Range(0, DefineManager.ACTIVE_SKILL_TYPE_SIZE);
                        if (nType != (int)DefineManager.SKILL.SKILL_COUNT) break;
                    }
                }
            }

            DefineManager.SKILL temp = (DefineManager.SKILL)nType;

            if (temp == DefineManager.SKILL.SKILL_DAMAGE)               fValue = Random.Range(1, 5);
            else if (temp == DefineManager.SKILL.SKILL_BLOOD_PERCENT)   fValue = Random.Range(0.001f, 0.01f);
            else                                                        fValue = Random.Range(0.01f, 0.1f);
        }
        else
        {
            //nType = Random.Range(DefineManager.ACTIVE_SKILL_TYPE_SIZE, DefineManager.ALL_SKILL_TYPE_SIZE);

            //DefineManager.SKILL temp = (DefineManager.SKILL)nType;
            //if (temp == DefineManager.SKILL.SKILL_ALL_COOLTIME)                 fValue = Random.Range(0.05f, 0.1f);
            //else if ((int)temp % 2 == 0)                                        fValue = Random.Range(0.01f, 0.05f);
            //else                                                                fValue = Random.Range(1, 5);
        }
    }

    public float GetAllSumDamage()
    {
        int sz = m_SkillData.Count;
        float fSum = 0;
        for(int i = 0; i< sz; i++) fSum += m_SkillData[i].m_fSumDamage;      
        return fSum;
    }
}
