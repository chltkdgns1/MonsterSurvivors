using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class OptionManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public OptionManager instance = null;

    private int m_nRobbyBackgroundSound;
    private int m_nInGameBackgroundSound;
    private int m_nInGameSound;

    private int m_nInGameDifficulty;
    private int[] m_nBestScore;
    private string[] m_sBestScore;

    public AudioClip[] m_ObBackGroundSoundTrackClip;
    public AudioClip[] m_ObAudioClips;

    private bool m_bAllLoadEnd = false;
    private bool m_bFirst = false;

    //static private int m_nEasyBestScore;
    //static private int m_nNormalBestScore;
    //static private int m_nHardBestScore;
    //static private int m_nHellBestScore;

    private int[] m_nLevelEx = new int[150];

    [SerializeField]
    private GameObject[] m_ObSkillPrefabs;
    [SerializeField]
    private string[] m_sSkillImagePath;
    [SerializeField]
    private string[] m_sSkillName;
    [SerializeField]
    private string[] m_sSkillComment;
    [SerializeField]
    private int[] m_nSkillInSpirteImage;
    [SerializeField]
    private int[] m_nSkillLimitCount;                   // 스킬 최대 사이즈
    [SerializeField]
    private bool[] m_bSkillActive;

    private List<List<Vector3>> m_vListDirect = new List<List<Vector3>>();

    [SerializeField]
    private bool[] m_bFollowPlayerSkill;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

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

    public int GetSkillSize()
    {
        return m_ObSkillPrefabs.Length;
    }

    public Vector3 GetCreatePosition(int nSkill, int index)

    {
        int sz = m_vListDirect[nSkill].Count;
        if (sz <= index) return new Vector3(0, 0, 0);
        return m_vListDirect[nSkill][index];
    }

    public int GetLimitSize(int nSkill)
    {
        if (m_nSkillLimitCount.Length <= nSkill || nSkill < 0)
        {
            Debug.LogError("    public int GetLimitSize(int nSkill)");
            return 0;
        }
        return m_nSkillLimitCount[nSkill];
    }

    public bool GetFollowPlayerSkill(int nSkill)
    {
        if (m_bFollowPlayerSkill.Length <= nSkill || nSkill < 0)
        {
            Debug.LogError("public bool GetFollowPlayerSkill(int nSkill)");
            return false;
        }
        return m_bFollowPlayerSkill[nSkill];
    }

    public GameObject GetSkill(int index)
    {
        if (index < 0 || m_ObSkillPrefabs.Length <= index)
        {
            Debug.LogError("    public GameObject GetSkill(int index)");
            return null;
        }
        return m_ObSkillPrefabs[index];
    }

    public string GetSkillImagePath(int index)
    {
        if (m_sSkillImagePath.Length <= index || index < 0)
        {
            Debug.LogError("    public string GetSkillImagePath(int index)");
            return null;
        }
        return m_sSkillImagePath[index];
    }

    public string GetSkillName(int index)
    {
        if (m_sSkillName.Length <= index || index < 0)
        {
            Debug.LogError("    public string GetSkillName(int index)");
            return null;
        }
        return m_sSkillName[index];
    }

    public bool GetSkillActive(int index)
    {
        if(m_bSkillActive.Length <= index || index < 0)
        {
            Debug.LogError("     public bool GetSkillActive(int index)");
            return false;
        }
        return m_bSkillActive[index];
    }

    public string GetSkillComment(int index)
    {
        if (m_sSkillComment.Length <= index || index < 0) return null;
        return m_sSkillComment[index];
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

        //Debug.Log(" m_nRobbyBackgroundSound : " + m_nRobbyBackgroundSound);

        m_nInGameBackgroundSound = PlayerPrefs.GetInt("m_nInGameBackGroundSound", 1);
        m_nInGameSound = PlayerPrefs.GetInt("m_nInGameSound", 1);
        m_nInGameDifficulty = 0;
        m_nBestScore = new int[4];
        m_sBestScore = new string[4];

        m_sBestScore[0] = "m_nEasyBestScore";
        m_sBestScore[0] = "m_nNormalBestScore";
        m_sBestScore[0] = "m_nHardBestScore";
        m_sBestScore[0] = "m_nHellBestScore";
        for (int i = 0; i < 4; i++)m_nBestScore[i] = PlayerPrefs.GetInt(m_sBestScore[i], 0);

        m_nLevelEx[1] = 10;
        m_nLevelEx[2] = 25;
        m_nLevelEx[3] = 50;
        m_nLevelEx[4] = 100;
        m_nLevelEx[5] = 160;
        m_nLevelEx[6] = 230;
        m_nLevelEx[7] = 350;
        m_nLevelEx[8] = 500;
        m_nLevelEx[9] = 700;
        m_nLevelEx[10] = 900;
        m_nLevelEx[11] = 1100;
        m_nLevelEx[12] = 1300;
        m_nLevelEx[13] = 1700;
        m_nLevelEx[14] = 2100;
        m_nLevelEx[15] = 2500;
        m_nLevelEx[16] = 3000;
        m_nLevelEx[17] = 3500;
        m_nLevelEx[18] = 4000;
        m_nLevelEx[19] = 4500;
        m_nLevelEx[20] = 5000;
        m_nLevelEx[21] = 5600;
        m_nLevelEx[22] = 6200;
        m_nLevelEx[23] = 6800;
        m_nLevelEx[24] = 7400;
        m_nLevelEx[25] = 8000;

        for(int i = 0; i < m_ObSkillPrefabs.Length; i++) m_vListDirect.Add(new List<Vector3>());

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

    public int GetRobbyBackgrounSound() {   return m_nRobbyBackgroundSound; }

    public int GetInGameBackgroundSound()    { return m_nInGameBackgroundSound;   }

    public int GetInGameSound(){return m_nInGameSound;}

    public int GetInGameDifficulty(){  return m_nInGameDifficulty; }

    public int GetBestScore(int index)    { return m_nBestScore[index]; }


    public void SetRobbyBackgrounSound(int value) {
        m_nRobbyBackgroundSound = value;
        PlayerPrefs.SetInt("m_nRobbyBackgroundSound", value);
        PlayerPrefs.Save();
    }
      
    public void SetInGameBackGroundSound(int value) {
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

    public void SetInGameDifficulty(int value)
    {
        m_nInGameDifficulty = value;
        PlayerPrefs.SetInt("m_nInGameDifficulty", value);
        PlayerPrefs.Save();
    }
              
    public void SetBestScore(int index, int value)
    {
        m_nBestScore[index] = value;
        PlayerPrefs.SetInt(m_sBestScore[index], value);
        PlayerPrefs.Save();
    }

    public int GetLevelEx(int index)
    {
        return m_nLevelEx[index];
    }

    public int GetSpriteInImage(int index)
    {
        if (m_nSkillInSpirteImage.Length <= index || index < 0) return 0;
        return m_nSkillInSpirteImage[index];
    }
}
