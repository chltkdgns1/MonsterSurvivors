using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataManage
{
    public class OptionManager : MonoBehaviour
    {
        // Start is called before the first frame update

        static public OptionManager instance = null;

        private int m_nRobbyBackgroundSound;
        private int m_nInGameBackgroundSound;
        private int m_nInGameSound;

        public AudioClip[] m_ObBackGroundSoundTrackClip;
        public AudioClip[] m_ObAudioClips;

        private bool m_bAllLoadEnd = false;
        private bool m_bFirst = false;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            init();
        }

        public bool GetAllLoad() { return m_bAllLoadEnd; }
        void LoadAudio()
        {
            m_ObBackGroundSoundTrackClip = new AudioClip[6];
            m_ObAudioClips = new AudioClip[4];

            m_ObBackGroundSoundTrackClip[0] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;
            m_ObBackGroundSoundTrackClip[1] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;
            m_ObBackGroundSoundTrackClip[2] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;
            m_ObBackGroundSoundTrackClip[3] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;
            m_ObBackGroundSoundTrackClip[4] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;
            m_ObBackGroundSoundTrackClip[5] = Resources.Load("Sound/BackGroundTrack/Adventure Puzzle Medieval") as AudioClip;

            m_ObAudioClips[0] = Resources.Load("Sound/LittleSound/Item/SpecialPowerup (4)") as AudioClip;
            m_ObAudioClips[1] = Resources.Load("Sound/LittleSound/Item/SpecialPowerup (5)") as AudioClip;
            m_ObAudioClips[2] = Resources.Load("Sound/LittleSound/Item/SpecialPowerup (13)") as AudioClip;
            m_ObAudioClips[3] = Resources.Load("Sound/LittleSound/Item/Ice (1)") as AudioClip;
        }

        private void Update()
        {
            if (m_bFirst == false)
            {
                m_bFirst = true;
                LoadAudio();
                m_bAllLoadEnd = true;
            }
        }

        void init()
        {
            m_nRobbyBackgroundSound = PlayerPrefs.GetInt("m_nRobbyBackgroundSound", 1);
            m_nInGameBackgroundSound = PlayerPrefs.GetInt("m_nInGameBackGroundSound", 1);
            m_nInGameSound = PlayerPrefs.GetInt("m_nInGameSound", 1);
        }

        public int GetRobbyBackgrounSound() { return m_nRobbyBackgroundSound; }

        public int GetInGameBackgroundSound() { return m_nInGameBackgroundSound; }

        public int GetInGameSound() { return m_nInGameSound; }

        public void SetRobbyBackgrounSound(int value)
        {
            m_nRobbyBackgroundSound = value;
            PlayerPrefs.SetInt("m_nRobbyBackgroundSound", value);
            PlayerPrefs.Save();
        }

        public void SetInGameBackGroundSound(int value)
        {
            m_nInGameBackgroundSound = value;
            PlayerPrefs.SetInt("m_nInGameBackGroundSound", value);
            PlayerPrefs.Save();
        }

        public void SetInGameSound(int value)
        {
            m_nInGameSound = value;
            PlayerPrefs.SetInt("m_nInGameSound", value);
            PlayerPrefs.Save();
        }
    }
}
