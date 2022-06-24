using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManage
{
    public class DataManager : MonoBehaviour
    { 
        static public DataManager instance = null;
        private UserData m_UserData = new UserData();

        private int m_nLevel;
        private int m_nInGameGold;
        private int m_nExe;
        private int m_nKilledMonster;

        private float m_fMaxHp = 100;


        public int Level
        {
            get { return m_nLevel; }
            set { m_nLevel = value; }
        }

        public int InGameGold
        {
            get { return m_nInGameGold; }
            set { m_nInGameGold = value; }
        }

        public int Exe
        {
            get { return m_nExe; }
            set { m_nExe = value; }
        }

        public int KillMonster
        {
            get { return m_nKilledMonster; }
            set { m_nKilledMonster = value; }
        }

        public float MaxHp
        {
            get { return m_fMaxHp; }
            set { m_fMaxHp = value; }
        }

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void OnDestroy()
        {

        }

        public void SetNickName(string sNickName)
        {
            m_UserData.SetNick(sNickName);
        }
    }
}
