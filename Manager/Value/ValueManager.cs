using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManage
{
    public class ValueManager : MonoBehaviour
    {
        static public ValueManager instance = null;

        private int[] m_nLevelEx = new int[301];

        //[SerializeField]
        //private GameObject[] m_ObSkillPrefabs;
        //[SerializeField]
        //private string[] m_sSkillImagePath;
        //[SerializeField]
        //private string[] m_sSkillName;
        //[SerializeField]
        //private string[] m_sSkillComment;
        //[SerializeField]
        //private int[] m_nSkillInSpirteImage;
        //[SerializeField]
        //private int[] m_nSkillLimitCount;                   // 스킬 최대 사이즈
        //[SerializeField]
        //private bool[] m_bSkillActive;
        //[SerializeField]
        //private bool[] m_bFollowPlayerSkill;

        //private List<List<Vector3>> m_vListDirect = new List<List<Vector3>>();

  

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            Init();
        }

        void InitData()
        {
            // 
        }

        void InitLevel()
        {
            m_nLevelEx[0] = 0;
            for (int i = 1; i <= 300; i++) m_nLevelEx[i] = m_nLevelEx[i - 1] + 50;
        }

        void Init()
        {
            InitLevel();
        }

        public int GetLevelEx(int index)
        {
            return m_nLevelEx[index];
        }
    }
}
