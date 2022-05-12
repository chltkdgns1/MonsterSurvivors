using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

public class UIClickStartManager            // UI 가 클릭 한 후에 다음 실행 될 시퀀스 처리
{

    static private UIClickStartManager instance = new UIClickStartManager();

    delegate void StartFunc(CommunicationTypeDataClass value);

    static private StartFunc[] m_startFunc;

    void init()
    {
        m_startFunc = new StartFunc[201];
        for (int i = 0; i < 200; i++) m_startFunc[i] = null;
        m_startFunc[1] = LoadScene;     // 대부분의 로딩 씬에 해당하는 부분

        // 2 ~ 10 까지는 로비에서 사용
        m_startFunc[2] = OpenSettings;  // 로비에서의 옵션 세팅 오픈
        m_startFunc[3] = OffSettings;   // 오프

        // 11 ~ 100 인게임에서 사용

        m_startFunc[11] = GoBackGame;
        m_startFunc[12] = ExitGame;
        m_startFunc[13] = ExitGameExit;
        m_startFunc[14] = RestartGame;
        m_startFunc[15] = SelectSkill;
        m_startFunc[16] = OpenTreasureBox;
        m_startFunc[17] = ReOpenTreasurBox;
        m_startFunc[18] = SetApplySkill;
        m_startFunc[19] = PrintSkillStatus;
        m_startFunc[20] = UnPrintSkillStatus;
        m_startFunc[21] = PrintResultDlg;
        m_startFunc[22] = CreateStructure;

        m_startFunc[199] = TestCode;

        //m_startFunc[2] = StartSpaceMemoryGame;
        //m_startFunc[3] = StartReactionSpeedGame;
        //m_startFunc[4] = StartBodyVisionGame;
        //m_startFunc[5] = StartCircleTouch;
    }

    private UIClickStartManager()
    {
        //Debug.Log("초기화 안됨?");
        init();
    }

    static public void Execute(CommunicationTypeDataClass value)
    {
        //Debug.Log("value.GetId() : " + value.GetId());
        if (m_startFunc[value.GetId()] == null) return;
        m_startFunc[value.GetId()](value);
    }

    static public void OpenSettings(CommunicationTypeDataClass value)
    {
        StartManager.instance.SetActiveSetting(true);
    }

    static public void OffSettings(CommunicationTypeDataClass value)
    {
        StartManager.instance.SetActiveSetting(false);
    }

    static void RestartGame(CommunicationTypeDataClass value)
    {
        GoogleAdsManager.instance.FrontShow(RestartAds);
    }

    static void ReOpenTreasurBox(CommunicationTypeDataClass value)
    {
        GoogleAdsManager.instance.FrontShow(ReOpenTreasureBoxCallBack);
    }

    static void RestartAds(bool flag,string []param = null)
    {
        if (flag == true)       PlayerOffline2D.instance.SetRestart();
        else                    PlayerOffline2D.instance.ExitGame();
    }

    static void ReOpenTreasureBoxCallBack(bool flag, string[] param = null)
    {
        if (flag == true) TreasureBoxManager.instance.PrintBox(3);
    }

    public static void LoadScene(CommunicationTypeDataClass value)
    {
        string[] param = value.GetParameter();
        float time = 0f;

        if(param.Length >= 3)
        {
            string sLoadingObject = value.GetParameter()[2];

            if (sLoadingObject == "PlayGameLobbyMove")
                GameUIManager.instance.SetActiveGoHomeLoading(true);
        }

        if(param.Length >= 4)
        {
            string sTime = value.GetParameter()[3];
            time = float.Parse(sTime);
        }

        LoadingSceneManager.instance.SetSceneName(value.GetParameter()[0], time);
    }

    static public void GoBackGame(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActivePauseBackScreen(false);
    }

    static public void ExitGame(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActiveExitPlayGame(true);
    }

    static public void ExitGameExit(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActiveExitPlayGame(false);
    }

    static public void SelectSkill(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.PlayerLevelUpEnd(int.Parse(value.GetParameter()[0]));
    }

    static public void OpenTreasureBox(CommunicationTypeDataClass value)
    {
        TreasureBoxManager.instance.OpenBox(3);
    }

    static public void SetApplySkill(CommunicationTypeDataClass value)
    {
        Debug.Log("abcdefg");
        GameUIManager.instance.SetActiveTreasureBox(false);
        TreasureBoxManager.instance.SetApplySkill(); 
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        TreasureBoxManager.instance.SetTreasureStartState(false);
    }

    static public void PrintSkillStatus(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);
        GameUIManager.instance.SetActiveSkillStatus(true);
    }

    static public void UnPrintSkillStatus(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        GameUIManager.instance.SetActiveSkillStatus(false);
    }

    static public void PrintResultDlg(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActiveEndGame(true);
        GameUIManager.instance.PrintDeadQuestion(false);
    }

    static public void CreateStructure(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);
        GameUIManager.instance.SetActiveSkillStatus(true);
    }

    static public void TestCode(CommunicationTypeDataClass value)
    {
       
        //GameUIManager.instance.SetActivePauseBackScreen(true);
        //GameUIManager.instance.GetTreasureBox(3);
        //GameUIManager.instance.PlayerLevelUp();
        //GameUIManager.instance.SetActiveExitPlayGame(true);
        //GameUIManager.instance.SetActivePauseBackScreen(true);
    }

}
