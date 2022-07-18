using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManage
{
    public struct SkillStatusStruct
    {
        public int m_nSkillType;
        public float m_fDamageValue;
        public float m_fCoolTimeValue;
        public float m_fSizeUpValue;
        public float m_fNuckbackValue;
        public float m_fBloodValue;
        public float m_fSumDamage;
        public float m_fDPS;

        public int m_nSkillCnt;
        public string m_sSkillActive;
        public string m_sSkillName;

        public SkillStatusStruct(int nSkillType, float fDamageValue, float fCoolTimeValue, float fSizeUpValue, float fNuckbackValue,
                                    float fBloodValue, int nSkillCnt, string sSkillActive, string sSkillName, float fSumDamage = 0f, float fDPS = 0f)
        {
            m_nSkillType = nSkillType;
            m_fDamageValue = fDamageValue;
            m_fCoolTimeValue = fCoolTimeValue;
            m_fSizeUpValue = fSizeUpValue;
            m_fNuckbackValue = fNuckbackValue;
            m_fBloodValue = fBloodValue;
            m_nSkillCnt = nSkillCnt;
            m_sSkillName = sSkillName;
            m_sSkillActive = sSkillActive;
            m_fSumDamage = fSumDamage;
            m_fDPS = fDPS;
        }
    }

    [System.Serializable]
    public struct SkillFileValue
    {     
        public int m_nSkillInSpirteImage;
        public int m_nSkillLimitCount;                   // 스킬 최대 사이즈
        public bool m_bSkillActive;
        public bool m_bFollowPlayerSkill;
        public string m_sSkillImagePath;
        public string m_sSkillName;
        public string m_sSkillComment;
    }

    [System.Serializable]
    public struct SkillValue
    {
        public int m_nSkill;
        public int m_nMinDamage;
        public int m_nMaxDamage;
        public int m_nFollowOption;

        public float m_fCoolTime;
        public float m_fSkillTime;
        public float m_fSpeed;

        public float m_fNuckBack;
        public float m_fBlood;

        public GameObject m_ObSkill;

        public SkillFileValue m_fileValue;
    }

    [System.Serializable]
    public class CraftStructure
    {
        public float m_fShield;
        public float m_fHp;
        public int m_nPrice;
        public string m_sName;
        public Sprite m_sprite;
        public GameObject m_ob;

        public CraftStructure(CraftStructure temp)
        {
            m_fHp = temp.m_fHp;
            m_fShield = temp.m_fShield;
            m_nPrice = temp.m_nPrice;
            m_sName = temp.m_sName;
            m_sprite = temp.m_sprite;
            m_ob = temp.m_ob;
        }
    }

    [System.Serializable]
    public class CraftTower : CraftStructure
    {
        public float m_fMinDamage;
        public float m_fMaxDamage;
        public float m_fNuckback;
        public float m_fBlood;
        public float m_fShootSpeed;

        public CraftTower(CraftTower temp) : base(temp)
        {
            m_fMinDamage = temp.m_fMinDamage;
            m_fMaxDamage = temp.m_fMaxDamage;
            m_fNuckback = temp.m_fNuckback;
            m_fBlood = temp.m_fBlood;
            m_fShootSpeed = temp.m_fShootSpeed;
        } 
    }

    [System.Serializable]
    public class CraftTrap : CraftStructure
    {
        public CraftTrap(CraftTrap temp) : base(temp)
        {
            
        }
    }

    public class InitData : MonoBehaviour
    {
        static public InitData instance = null;
        private List<List<Vector3>> m_vListDirect = new List<List<Vector3>>();

        [SerializeField]
        private List<SkillValue> m_SkillData = new List<SkillValue>();
        [SerializeField]
        private List<CraftStructure> m_vCraftStructer = new List<CraftStructure>();
        [SerializeField]
        private List<CraftTower> m_vCraftTower= new List<CraftTower>();
        [SerializeField]
        private List<CraftTrap> m_vCraftTrap = new List<CraftTrap>();

        private void Awake()
        {
            if (instance == null)   instance = this;
            else                    Destroy(gameObject);

            Init();
        }

        void Init()
        {
            InitSkillDir();
        }

        public SkillValue? GetSkillValue(int index)
        {
            if (index < 0 || index >= m_SkillData.Count) return null;
            return m_SkillData[index];
        }

        void InitSkillDir()
        {
            for (int i = 0; i < m_SkillData.Count; i++)
                m_vListDirect.Add(new List<Vector3>());

            m_vListDirect[1].Add(new Vector3(3.5f, 0));
            m_vListDirect[1].Add(new Vector3(-3.5f, 0));
            m_vListDirect[1].Add(new Vector3(0, 3.5f));
            m_vListDirect[1].Add(new Vector3(0, -3.5f));

            m_vListDirect[2].Add(new Vector3(0.1f, 0));
            m_vListDirect[2].Add(new Vector3(-0.1f, 0));
            m_vListDirect[2].Add(new Vector3(0, 0.1f));
            m_vListDirect[2].Add(new Vector3(0, -0.1f));

            m_vListDirect[3].Add(new Vector3(1f, 0));
            m_vListDirect[3].Add(new Vector3(-1f, 0));
        }

        public int GetSpriteInImage(int index)
        {
            if (m_SkillData.Count<= index || index < 0) return 0;
            return m_SkillData[index].m_fileValue.m_nSkillInSpirteImage;
        }


        public int GetSkillSize()
        {
            return m_SkillData.Count;
        }

        public Vector3 GetCreatePosition(int nSkill, int index)
        {
            int sz = m_vListDirect[nSkill].Count;
            if (sz <= index) return new Vector3(0, 0, 0);
            return m_vListDirect[nSkill][index];
        }

        public int GetLimitSize(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("    public int GetLimitSize(int nSkill)");
                return 0;
            }
            return m_SkillData[index].m_fileValue.m_nSkillLimitCount;
        }

        public bool GetFollowPlayerSkill(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("public bool GetFollowPlayerSkill(int nSkill)");
                return false;
            }
            return m_SkillData[index].m_fileValue.m_bFollowPlayerSkill;
        }

        public GameObject GetSkill(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("    public GameObject GetSkill(int index)");
                return null;
            }
            return m_SkillData[index].m_ObSkill;
        }

        public string GetSkillImagePath(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("    public string GetSkillImagePath(int index)");
                return null;
            }
            return m_SkillData[index].m_fileValue.m_sSkillImagePath;
        }

        public string GetSkillName(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("    public string GetSkillName(int index)");
                return null;
            }
            return m_SkillData[index].m_fileValue.m_sSkillName;
        }

        public bool GetSkillActive(int index)
        {
            if (m_SkillData.Count <= index || index < 0)
            {
                Debug.LogError("     public bool GetSkillActive(int index)");
                return false;
            }
            return m_SkillData[index].m_fileValue.m_bSkillActive;
        }

        public string GetSkillComment(int index)
        {
            if (m_SkillData.Count <= index || index < 0) return null;
            return m_SkillData[index].m_fileValue.m_sSkillComment;
        }

        public CraftStructure GetCraftStructure(int index)
        {
            if (index < 0 || index >= m_vCraftStructer.Count) return null;
            return new CraftStructure(m_vCraftStructer[index]);
        }

        public CraftTower GetCraftTower(int index)
        {
            if (index < 0 || index >= m_vCraftTower.Count) return null;
            return new CraftTower(m_vCraftTower[index]);
        }

        public List<CraftStructure> GetStructureList()
        {
            return m_vCraftStructer;
        }

        public List<CraftTower> GetTowerList()
        {
            return m_vCraftTower;
        }

        public List<CraftTrap> GetTrapList()
        {
            return m_vCraftTrap;
        }
    }
}
