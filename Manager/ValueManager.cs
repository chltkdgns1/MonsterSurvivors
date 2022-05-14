using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ValueManager : MonoBehaviour
{

    static public ValueManager instance = null;

    private int[] m_nLevelEx = new int[301];
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

    private UserData m_UserData = new UserData();

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        Init();
    }

    void InitData()
    {
        // 
    }

    void InitLevel()
    {
        m_nLevelEx[0] = 0;
        for(int i = 1; i<= 300; i++) m_nLevelEx[i] = m_nLevelEx[i - 1] + 50;    
    }

    void InitSkillDir()
    {
        for (int i = 0; i < m_ObSkillPrefabs.Length; i++) m_vListDirect.Add(new List<Vector3>());

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

    void Init()
    {
        InitLevel();
        InitSkillDir();
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
        if (m_bSkillActive.Length <= index || index < 0)
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

    public void SetNickName(string sNickName)
    {
        m_UserData.SetNick(sNickName);
    }

    //public void SetUID(+-++)

}
