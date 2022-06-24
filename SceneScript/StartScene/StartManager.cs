using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public StartManager instance = null;

    [SerializeField]
    private GameObject []m_ObToggle;
    private Toggle[] m_CompToggle;

    [SerializeField]
    private AudioSource m_ObAudioSoucre;

    [SerializeField]
    private GameObject m_ObSettings;

    private int m_nBackgroundTrack;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
        //Screen.orientation = ScreenOrientation.Portrait; //���� ������ ��Ÿ���ϴ�.
        //Screen.orientation = ScreenOrientation.PortraitUpsideDown; //��ġ�� ���κ��� �Ʒ��� ���ϴ�, ���� ������ ��Ÿ���ϴ�.
        //Screen.orientation = ScreenOrientation.LandscapeLeft; //���� ������ ��Ÿ����, ���� �������κ��� �� �ð�������� ȸ���� ���¸� ��Ÿ���ϴ�.
        //Screen.orientation = ScreenOrientation.LandscapeRight; //���� ������ ��Ÿ����, ���� �������κ��� �ð�������� ȸ���� ���¸� ��Ÿ���ϴ�.
        //Screen.orientation = ScreenOrientation.AutoRotation; //Ȱ��ȭ�� �������� �ڵ� ȸ�� �ϵ��� �����մϴ�.
    }
    void Start()
    {
        InitUI();
        m_nBackgroundTrack = -1;
    }

    void InitUI()
    {
        m_CompToggle = new Toggle[m_ObToggle.Length];
        for (int i = 0; i < m_ObToggle.Length; i++)
        {
            m_CompToggle[i] = m_ObToggle[i].GetComponent<Toggle>();
        }

        if (DataManage.OptionManager.instance.GetRobbyBackgrounSound() == 0) m_CompToggle[0].isOn = false;
        else m_CompToggle[0].isOn = true;
        if (DataManage.OptionManager.instance.GetInGameBackgroundSound() == 0) m_CompToggle[1].isOn = false;
        else m_CompToggle[1].isOn = true;
        if (DataManage.OptionManager.instance.GetInGameSound() == 0) m_CompToggle[2].isOn = false;
        else m_CompToggle[2].isOn = true;

        m_CompToggle[0].onValueChanged.AddListener(SetLobbyBackgroundTrackChange);
        m_CompToggle[1].onValueChanged.AddListener(SetInGameBackgroundTrackChange);
        m_CompToggle[2].onValueChanged.AddListener(SetInGameSoundChange);

        SetActiveSetting(false);
    }

    public void SetActiveSetting(bool flag)
    {
        m_ObSettings.SetActive(flag);
    }

    void SetLobbyBackgroundTrackChange(bool flag)
    {
        int value = flag == false ? 0 : 1;
        //Debug.Log("value : " + value);
        DataManage.OptionManager.instance.SetRobbyBackgrounSound(value);
    }

    void SetInGameBackgroundTrackChange(bool flag)
    {
        int value = flag == false ? 0 : 1;
        DataManage.OptionManager.instance.SetInGameBackGroundSound(value);
    }

    void SetInGameSoundChange(bool flag)
    {
        int value = flag == false ? 0 : 1;
        DataManage.OptionManager.instance.SetInGameSound(value);
    }

    // Update is called once per frame
    void Update()
    {
       ExAudioState();
    }

    void ExAudioState()
    {
        if (m_nBackgroundTrack != DataManage.OptionManager.instance.GetRobbyBackgrounSound())
        {
            m_nBackgroundTrack = DataManage.OptionManager.instance.GetRobbyBackgrounSound();
            if (m_nBackgroundTrack == 1)    m_ObAudioSoucre.Play();
            else                            m_ObAudioSoucre.Stop();
        }
    }
}
