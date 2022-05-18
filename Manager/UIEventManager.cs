using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventManager
{
    // Start is called before the first frame update

    static private UIEventManager instance = new UIEventManager();

    delegate void StartFunc(CommunicationTypeDataClass value);

    static private StartFunc[] m_startFuncUp;
    static private StartFunc[] m_startFuncDown;
    void init()
    {
        m_startFuncUp = new StartFunc[201];
        m_startFuncDown = new StartFunc[201];
        for (int i = 0; i < 200; i++)
        {
            m_startFuncUp[i] = null;
            m_startFuncDown[i] = null;
        }

        m_startFuncUp  [40] = ClickedUp;
        m_startFuncUp  [1]  = ClickedJustUp;

        m_startFuncDown[40] = ClickedDown;
        m_startFuncDown[1]  = ClickedJustDown;

        m_startFuncDown[13] = ExitGameExit;
        m_startFuncDown[20] = UnPrintSkillStatus;

        m_startFuncDown[21] = PrintResultDlg;
        m_startFuncDown[22] = PrintCraftingList;
        m_startFuncDown[23] = UnPrintCraftingList;

        m_startFuncDown[24] = CraftSelect;          // 타워 생성 클릭했을 경우

        m_startFuncDown[100] = ClickAccessStop;
        m_startFuncDown[0] = LoadScene;
    }

    static public void PrintResultDlg(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActiveEndGame(true);
        GameUIManager.instance.PrintDeadQuestion(false);
    }

    static public void ClickAccessStop(CommunicationTypeDataClass value)
    {

    }

    private UIEventManager()
    {
        init();
    }
    static public void OnClickEvent(CommunicationTypeDataClass value)  // 개수가 많아지면 관리가 힘들 것 같긴함
    {

    }

    static public void OnClickUpEvent(CommunicationTypeDataClass value)  // 개수가 많아지면 관리가 힘들 것 같긴함
    {
        if (m_startFuncUp[value.GetId()] == null) return;
        m_startFuncUp[value.GetId()](value);
    }


    static public void LoadScene(CommunicationTypeDataClass value)
    {
        string[] param = value.GetParameter();
        float time = 0f;

        if (param.Length >= 3)
        {
            string sLoadingObject = value.GetParameter()[2];

            if (sLoadingObject == "PlayGameLobbyMove")
                GameUIManager.instance.SetActiveGoHomeLoading(true);
        }

        if (param.Length >= 4)
        {
            string sTime = value.GetParameter()[3];
            time = float.Parse(sTime);
        }

        LoadingSceneManager.instance.SetSceneName(value.GetParameter()[0], time);
    }

    static public void UnPrintSkillStatus(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        GameUIManager.instance.SetActiveSkillStatus(false);
    }
    static public void OnClickDownEvent(CommunicationTypeDataClass value)  // 개수가 많아지면 관리가 힘들 것 같긴함 object 를 넘겨주어도 상관없고 오브젝트 사용안해도됨
    {
        if (m_startFuncDown[value.GetId()] == null) return;
        m_startFuncDown[value.GetId()](value);
    }

    static private void ClickedJustDown(CommunicationTypeDataClass value)
    {
        if (value.GetGameObject() == null) return;
        Image UI = value.GetGameObject().GetComponent<Image>();
        if (UI == null) return;

        Color col = UI.color;
        col.a = 0.5f;
        UI.color = col;       
    }

    static private void ClickedJustUp(CommunicationTypeDataClass value)
    {
        if (value.GetGameObject() == null) return;
        Image UI = value.GetGameObject().GetComponent<Image>();
        if (UI == null) return;

        Color col = UI.color;
        col.a = 1f;
        UI.color = col;
    }

    static private void ClickedDown(CommunicationTypeDataClass value)
    {
        if (value.GetGameObject() == null) return;
        Clicked click = value.GetGameObject().GetComponent<Clicked>();
        if (click == null) return;     
        click.MoveSmall();       
    }

    static private void ClickedUp(CommunicationTypeDataClass value)
    {
        if (value.GetGameObject() == null) return;
        Clicked click = value.GetGameObject().GetComponent<Clicked>();
        if (click == null) return;
        click.MoveBig();
    }

    static private void ExitGameExit(CommunicationTypeDataClass value)
    {
        GameUIManager.instance.SetActiveExitPlayGame(false);
    }

    static private void PrintCraftingList(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);
        GameUIManager.instance.SetActiveCraftingList(true);
    }

    static private void UnPrintCraftingList(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        GameUIManager.instance.SetActiveCraftingList(false);
    }

    static private void CraftSelect(CommunicationTypeDataClass value)
    {
        PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        GameUIManager.instance.SetActiveCraftingList(false);

        CraftManager.instance.StartCrafting(value.GetParameter());
    }
}
