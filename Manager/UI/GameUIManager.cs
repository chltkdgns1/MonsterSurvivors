using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    static public GameUIManager instance = null;

    private GameObject      m_obPauseBackScreen;
    private GameObject      m_obExitPlayGame;
    private GameObject      m_obGoHomeLoading;
    private GameObject      m_obDeadQuestion;
    private GameObject      m_obRestartScreen;
    private GameObject      m_obEndGame;
    private Text            m_obRestartText;
    private GameObject      m_obTouchPad;
    private GameObject      m_obTouchCircle;
    private GameObject      m_obLevelUp;
    private GameObject      m_obGetTreasureBox;
    private GameObject      m_obSkillStatus;
    private GameObject      m_obGameOver;
    private GameObject      m_obCraftingList;
    private GameObject      m_obCraftDelete;
    [SerializeField]
    private GameObject      m_obCraftDeleteOk;

    [SerializeField]
    private Image           m_ImageLevelGage;
    [SerializeField]
    private Text            m_TextLevelPercent;
    [SerializeField]
    private Text            m_TextLevelText;
    [SerializeField]
    private Text            m_TextTime;
    [SerializeField]
    private Text            m_TextMoney;
    [SerializeField]
    private GameObject       m_ObTextMoney;

    private GameObject  []  m_obSkillTouch;
    private GameObject  []  m_obSkillCoolTouch;
    private Text        []  m_obSkillCoolTimeText;
    private Image       []  m_obSkillCoolTimeImage;
    private float       []  m_nSkillCoolTime;
    private bool        []  m_nCoolTimeManage;

    private Transform       m_canvasTransform;
    private float           m_fCanvasHeight;
    private float           m_fCanvasWidth;
    private Vector3         m_vCalibValue;

    // 스킬
    [SerializeField]
    private Image       [] m_ImageSkill;
    [SerializeField]
    private Text        [] m_TextSkillName;
    [SerializeField]
    private Text        [] m_TextSkillComment;
    [SerializeField]
    private Text        [] m_TextSkillValue;
    [SerializeField]
    private GameObject  [] m_obNewText;

    private int         [] m_nSkill = new int[3];
    private int         [] m_nType  = new int[3];
    private float       [] m_fValue = new float[3];

    // 스킬 끝

    public Vector3 GetCanvasCalibValue() { return m_vCalibValue;}

    void Awake()
    {
        if (instance == null)   instance = this;  
        else                    Destroy(gameObject);

        initObject();
        Init();
    }

    void initObject()
    {
        m_canvasTransform           = GameObject.Find("Canvas").transform;
        m_obPauseBackScreen         = GameObject.Find("PauseBackScreen");
        m_obExitPlayGame            = GameObject.Find("ExitPlayGame");
        m_obGoHomeLoading           = GameObject.Find("GoHomeLoading");
        m_obDeadQuestion            = GameObject.Find("DeadQuestion");
        m_obRestartScreen           = GameObject.Find("RestartScreen");
        m_obTouchPad                = GameObject.Find("TouchPad");
        m_obLevelUp                 = GameObject.Find("SkillSelectBack");
        m_obGetTreasureBox          = GameObject.Find("TreasureBox");
        m_obSkillStatus             = GameObject.Find("SkillStatus");
        m_obGameOver                = GameObject.Find("GameOver");
        m_obEndGame                 = GameObject.Find("EndGame");
        m_obCraftingList            = GameObject.Find("CraftingList");
        m_obCraftDelete             = GameObject.Find("CraftDelete");
        

        m_TextLevelText.text        = "1 Lv";
        m_TextLevelPercent.text     = "0 %";
        m_ImageLevelGage.fillAmount = 0f;
        m_TextTime.text             = "00:00";
        SetMoneyText("0");
    }

  
    void Init()
    {
        if (m_obTouchPad != null) m_obTouchCircle = m_obTouchPad.transform.GetChild(0).gameObject;
  
        SetActive();

        m_fCanvasWidth          = m_canvasTransform.GetComponent<RectTransform>().rect.width;
        m_fCanvasHeight         = m_canvasTransform.GetComponent<RectTransform>().rect.height;
        m_vCalibValue           = new Vector3(m_fCanvasWidth / 2, m_fCanvasHeight / 2);     
    }

    private void Start()
    {
        DrawGrid.instance.SetLinePosition(100);
        DrawGrid.instance.RenderingGrid();
        DrawGrid.instance.SetActive(false);
    }

    private void Update()
    {
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_PAUSE) return;
        if (PlayingGameManager.GetGameState() == DefineManager.GameState.PLAYING_STATE_CRAFTING) return;

        SetTimeText();
        SetLevel();
        TimeEndGame();
    }

    void TimeEndGame()
    {
        if (PlayTimeManager.instance.IsTimeEnd() == true)
        {
            PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_PAUSE);
            SetActiveEndGame(true);
            PrintDeadQuestion(false);
        }
    }

    void SetTimeText()
    {
        m_TextTime.text = PlayTimeManager.instance.GetTimeText();
    }

    void SetLevel()
    {
        int level = DataManage.DataManager.instance.Level;
        m_TextLevelText.text = level + " Lv";
        int levelMaxEx = DataManage.ValueManager.instance.GetLevelEx(level);
        float fPercent = (float)DataManage.DataManager.instance.Exe / levelMaxEx;
        m_TextLevelPercent.text = (int)(fPercent * 100) + " %";
        m_ImageLevelGage.fillAmount = fPercent;
    }

    void PrintExMax()
    {
        m_TextLevelPercent.text = "100 %";
        m_ImageLevelGage.fillAmount = 1f;
    }

    void SetActive()
    {
        if(m_obPauseBackScreen      != null)    m_obPauseBackScreen.SetActive   (false);
        if(m_obExitPlayGame         != null)    m_obExitPlayGame.SetActive      (false);
        if(m_obGoHomeLoading        != null)    m_obGoHomeLoading.SetActive     (false);
        if(m_obTouchPad             != null)    m_obTouchPad.SetActive          (false);
        if(m_obLevelUp              != null)    m_obLevelUp.SetActive           (false);
        if(m_obGetTreasureBox       != null)    m_obGetTreasureBox.SetActive    (false);
        if(m_obSkillStatus          != null)    m_obSkillStatus.SetActive       (false);
        if(m_obGameOver             != null)    m_obGameOver.SetActive          (false);
        if(m_obEndGame              != null)    m_obEndGame.SetActive           (false);
        if (m_obCraftingList        != null)    m_obCraftingList.SetActive      (false);
        if (m_obCraftDelete         != null)    m_obCraftDelete.SetActive       (false);

        if (m_obRestartScreen != null)
        {
            m_obRestartScreen.SetActive(false);
            m_obRestartText = m_obRestartScreen.transform.GetChild(0).gameObject.GetComponent<Text>();
        }
        if (m_obDeadQuestion != null)   m_obDeadQuestion.SetActive          (false);
    }

    public void SetActiveSkillStatus(bool flag)
    {
        m_obSkillStatus.SetActive(flag);
    }

    public void PrintDeadQuestion(bool flag)
    {
        m_obGameOver.SetActive(flag);
        m_obDeadQuestion.SetActive(flag);
    }

    public void SetActiveGameOver(bool flag)
    {
        m_obGameOver.SetActive(flag);
    }

    public void SetActivePauseBackScreen(bool flag)
    {
        m_obPauseBackScreen.SetActive(flag);

        bool bEscape = !flag;

        if(bEscape)     PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_NOMAL);    
        else            PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_PAUSE);
     
        AndroidKeyUIManager.instance.SetEscape(flag);
    }

    public void SetActiveExitPlayGame(bool flag)
    {
        m_obExitPlayGame.SetActive(flag);
    }

    public void SetActiveGoHomeLoading(bool flag)
    {
        m_obGoHomeLoading.SetActive(flag);
    }

    public void SetActiveRestartScreen(bool flag)
    {
        m_obRestartScreen.SetActive(flag);
    }

    public void SetActiveLevelUp(bool flag)
    {
        m_obLevelUp.SetActive(flag);
    }

    public void SetActiveEndGame(bool flag)
    {
        m_obEndGame.SetActive(flag);
    }

    public void SetGameOverFunc(CallBackFunc func)
    {
        GameOverUIManager temp = m_obGameOver.GetComponent<GameOverUIManager>();
        if(temp != null) temp.SetFunc(func);      
    }

    public void SetActiveTouchPad(bool flag)
    {
        m_obTouchPad.SetActive(flag);
    }

    public void SetActiveCraftingList(bool flag)
    {
        m_obCraftingList.SetActive(flag);
    }

    public void SetActiveCraftDelete(bool flag)
    {
        m_obCraftDelete.SetActive(flag);
        m_obCraftDelete.transform.GetChild(0).GetChild(2);
    }

    public void SetCraftDeleteOkCallBack(EventCallBack callBack, CommunicationTypeDataClass value)
    {
        Clicked temp = m_obCraftDeleteOk.GetComponent<Clicked>();
        temp.EventCallBack = callBack;
        temp.InitCommunicateValue(value);
    }

    public void SetPositionTouchPad(Vector3 Position)
    {
        m_obTouchPad.transform.position = Position;
    }

    public void SetPositionTouchCircle(Vector3 Position)
    {
        m_obTouchCircle.transform.position = Position;
    }

    public void RestartGameTimer()
    {
        SetActiveRestartScreen(true);
        StartCoroutine(RestartGameTimerRoutine(3f));
    }



    IEnumerator RestartGameTimerRoutine(float timer)
    {
        m_obRestartText.text = ((int)timer).ToString();

        int tempTimer = (int)timer;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;

            if((int)timer + 1 != tempTimer)
            {
                tempTimer = (int)timer + 1;
                m_obRestartText.text = tempTimer.ToString();
            }

            yield return null;
        }

        SetActiveRestartScreen(false);
        PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_PAUSE);
        PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_NO_ENEMY);
    }

    public void PlayerLevelUp()
    {
        PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_PAUSE);
        PrintExMax();

        int nSkill = 0, nType = 0;
        float fValue = 0;

        for (int i = 0; i < 3; i++)
        {
            SkillManager.instance.GetRandSkillStatus(ref nSkill, ref nType, ref fValue);

            int nInSprite   = DataManage.InitData.instance.GetSpriteInImage(nSkill);
            string sPath    = DataManage.InitData.instance.GetSkillImagePath(nSkill);

            if (nInSprite != 0) Module.SetSpriteImageAll(m_ImageSkill[i], sPath, nInSprite);          // 스프라이트 이미지 내에 있는 이미지라면 
            else                Module.SetSpriteImage(m_ImageSkill[i], sPath);

            m_TextSkillName[i].text = DataManage.InitData.instance.GetSkillName(nSkill);

            List<string> templist   = Module.Split(DataManage.InitData.instance.GetSkillComment(nSkill), '.');
            string temp             = Module.MergeString(templist, '.', true);

            m_TextSkillComment[i].text = temp;

            bool bIsExist           = SkillManager.instance.IsExistSkill(nSkill);
            if (bIsExist == false) m_obNewText[i].SetActive(true);

            m_TextSkillValue[i].text = SetSkillValue(nType, fValue);

            m_nSkill[i]     = nSkill;
            m_nType[i]      = nType;
            m_fValue[i]     = fValue;
        }
        SetActiveLevelUp(true);
    }

    public void PlayerLevelUpEnd(int index) // 첫번째, 두번째 , 세번째 스킬 인덱스
    {
        PlayingGameManager.SetOutState(DefineManager.GameState.PLAYING_STATE_PAUSE);
        SetActiveLevelUp(false);
        SelectSkill(m_nSkill[index], m_nType[index], m_fValue[index]);   
    }

    void SelectSkill(int nSkill, int nType, float fValue)
    {
        SkillManager.instance.SkillJudge(nSkill, nType, fValue);
    }

    public string SetSkillValue(int nType, float fValue)
    {
        string sSkillValueText = "";
        switch ((DefineManager.SKILL)nType)
        {        
            case DefineManager.SKILL.SKILL_SIZE_PERCENT         : sSkillValueText = "범위 " + ((int)(fValue * 100)).ToString() + " % 증가";  break;
            case DefineManager.SKILL.SKILL_COOLTIME_PERCENT     : sSkillValueText = "쿨타임 " + ((int)(fValue * 100)).ToString() + " % 감소"; break; 
            //case DefineManager.SKILL.SKILL_ALL_COOLTIME         : sSkillValueText = "(ALL) 쿨타임 " + fValue.ToString("F3") + " 감소"; break; 
            case DefineManager.SKILL.SKILL_DAMAGE               : sSkillValueText = "데미지 " + ((int)fValue).ToString() + " 증가"; break; 
            case DefineManager.SKILL.SKILL_DAMAGE_PERCENT       : sSkillValueText = "데미지 " + ((int)(fValue * 100)).ToString() + " % 증가"; break; 
            case DefineManager.SKILL.SKILL_COUNT                : sSkillValueText = "발사체 증가"; break;
            case DefineManager.SKILL.SKILL_NUCKBACK_PERCENT     : sSkillValueText = "넉백 " + ((int)(fValue * 100)).ToString() + " % 증가"; break;
            case DefineManager.SKILL.SKILL_BLOOD_PERCENT        : sSkillValueText = "피흡률 " + (fValue * 100).ToString("F1") + " % 증가"; break;
        }
        return sSkillValueText;
    }

    public void GetTreasureBox(int nLevel, bool bAd = false)
    {
        string[] param = new string[1];
        param[0] = nLevel.ToString();

        if(bAd == true) GoogleAdsManager.instance.FrontShow(OpenBoxAD, param);  
        else            TreasureBoxManager.instance.OpenBoxAdd(nLevel);

        //SetActiveTreasureBox(true);
        //PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_PAUSE);
        //TreasureBoxManager.instance.PrintBox(nLevel);
    }

    void OpenBoxAD(bool bSuccess, string[] param)
    {
        if (bSuccess == false) return;
        if (param == null || param.Length != 1) return;
        int nLevel = int.Parse(param[0]);
        TreasureBoxManager.instance.OpenBoxAdd(nLevel);
    }

    public void SetActiveTreasureBox(bool flag)
    {
        m_obGetTreasureBox.SetActive(flag);
    }

    public void SetMoneyText(string sText)
    {
        Module.GetMoneyString(ref sText);
        m_TextMoney.text = sText;
    }

    public Vector3 GetMoneyTextPosition() {
        return m_ObTextMoney.transform.position;
    }
}
