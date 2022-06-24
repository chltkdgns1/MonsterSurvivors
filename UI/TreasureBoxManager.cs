using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBoxManager : MonoBehaviour
{
    // Start is called before the first frame update

    static public TreasureBoxManager instance = null;

    [SerializeField]
    private int m_nLeftRightNum;

    [SerializeField]
    private float m_fLeftRightSize;

    [SerializeField]
    private float m_fSpeed;

    [SerializeField]
    private int m_nUpcount;

    [SerializeField]
    private float m_fWidthChange;

    [SerializeField]
    private float m_fHeightChange;

    [SerializeField]
    private GameObject m_ObOpenFinish;
    [SerializeField]
    private GameObject m_ObOpen;

    [SerializeField]
    private string[] m_sBoxSpriteImage;
    [SerializeField]
    private string[] m_sOpenBoxSpriteImage;
    [SerializeField]
    private Text[]  m_TextSkill;

    [SerializeField]
    private GameObject[] m_ObBox;
    
    [SerializeField]
    private GameObject[] m_ObSkill;

    private List<int> m_ListBox                 = new List<int>();
    private List<SkillFormat> m_ListSkillFormat = new List<SkillFormat>();

    private bool m_bStarted = false;

    private Queue<int> m_TreasureBoxQueue = new Queue<int>();


    //// 트레저 상자 외에도 사용할 오브젝트
    //private GameObject[] m_obParam = null;
    //private string[] m_sImageParam = null;

    //private bool m_bEndRoutine;

    private int m_nBoxOpenEnd = 0;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
    }

    private void OnEnable()
    {
        InitSettings();
    }

    private void Update()
    {
        if(m_bStarted == false)    // 시작을 하지 않은 상태일 경우에
        {
            if (m_TreasureBoxQueue.Count != 0) // 비어있지 않다면,
            {
                int nSize = m_TreasureBoxQueue.Peek();
                m_TreasureBoxQueue.Dequeue();
                InitSettings();
                GameUIManager.instance.SetActiveTreasureBox(true);
                PlayingGameManager.SetGameState(DefineManager.GameState.PLAYING_STATE_PAUSE);
                PrintBox(nSize);
            }
        }
    }

    void InitSettings()
    {
        SetActiveButton(true);
        SetActiveBoxAll(false);
        SetActiveSkillAll(false);
    }

    void SetActiveButton(bool flag)
    {
        m_ObOpen.SetActive(flag);
        m_ObOpenFinish.SetActive(!flag);
    }

    void SetActiveButtonSide(bool flag)
    {
        m_ObOpen.SetActive(flag);
        m_ObOpenFinish.SetActive(flag);
    }

    void SetActiveBoxAll(bool flag)
    {
        int nBoxSize = m_ObBox.Length;
        for (int i = 0; i < nBoxSize; i++) m_ObBox[i].SetActive(flag);
    }

    void SetActiveSkillAll(bool flag)
    {
        int nSkillSize = m_ObSkill.Length;
        for (int i = 0; i < nSkillSize; i++) m_ObSkill[i].SetActive(false);       
    }

    void SetActiveBox(int nIndex, bool flag)
    {
        int nBoxSize = m_ObBox.Length;
        if (nBoxSize <= nIndex || nIndex < 0) return;
        m_ObBox[nIndex].SetActive(flag);
    }

    void SetActiveSkill(int nIndex,bool flag)
    {
        int nSkillSize = m_ObSkill.Length;
        if (nSkillSize <= nIndex || nIndex < 0) return;
        m_ObSkill[nIndex].SetActive(flag);
     }

    public void PrintBox(int nBoxLevel)
    {
        SetTreasureStartState(true);
        InitSettings();
        // 무조건 한개는 출력.
        int nState = GetRandomBoxState();                               // 랜덤 확률인지 체크

        m_ListBox.Clear();
        m_ListSkillFormat.Clear();

        m_ListBox.Add(nBoxLevel);
        AddRandomBox(nState, nBoxLevel);                   // 박스 구성 요소 생성
        m_ListBox.Sort();                                             // 내림차순 정렬
        m_ListBox.Reverse();
        // 랜덤을 이용하여 어떤 박스를 출력할 것인지까지 정함.


        AddRandomSkill();                   // 각각의 박스에서 나올 스킬, 능력치
        SetImageBoxAndSkill();              // 스킬 이미지 설정
        SetSizeChange(false);
        PrintSequenceBox(3);

        //int nSize = m_ListBox.Count;
        //for(int i = 0; i < nSize; i++) PrintEachBox(i);
       
        // 일단 모든 박스에 대해서 세팅을 해놓는다. 예를 들어, 박스의 이미지라든지, 박스 위의 나타날 스킬이라든지 등등.


        // 1개 또는 3개라면 박스가 좌우로 흔들리면서 그냥 출력함.
        // 박스가 5개라면 3개 박스가 좌우로 움직이는 도중에 추가 이펙트 발생으로 5개로 추가 출력함.
    }

    void SetSkillText(int nIndex)
    {
        if(nIndex < 0 || nIndex >= m_TextSkill.Length || nIndex >= m_ListSkillFormat.Count)
        {
            Debug.LogError(" void SetSkillText(int nIndex) : " + nIndex);
            return;
        }

        int nSkill = m_ListSkillFormat[nIndex].m_nSkill;
        int nType = m_ListSkillFormat[nIndex].m_nType;
        float fValue = m_ListSkillFormat[nIndex].m_fValue;
        m_TextSkill[nIndex].text = GameUIManager.instance.SetSkillValue(nType, fValue);
    }

    void PrintSequenceBox(int nSize)
    {
        int nSizeBox = m_ListBox.Count;
        int nPrintSize = Mathf.Min(nSize, nSizeBox);
        for(int i = 0; i < nPrintSize; i++) SetActiveBox(i, true);     
    }

    void PrintEachBox(int nIndex)
    {

        m_nBoxOpenEnd++;

        SetActiveBox(nIndex, true);
        SetSkillText(nIndex);
        StartCoroutine(MoveLeftRight(nIndex));
    }

    void SetImageBoxAndSkill()
    {
        int nBoxSize = m_ListBox.Count;

        SetImageBox(false);

        for (int i = 0; i < nBoxSize; i++)
        {
            Image ImageSkill    = m_ObSkill[i].GetComponent<Image>();
            int nSkill          = m_ListSkillFormat[i].m_nSkill;
            int nInSprite       = DataManage.InitData.instance.GetSpriteInImage(nSkill);
            string sPath        = DataManage.InitData.instance.GetSkillImagePath(nSkill);

            if (nInSprite != 0) Module.SetSpriteImageAll(ImageSkill, sPath, nInSprite);          // 스프라이트 이미지 내에 있는 이미지라면 
            else                Module.SetSpriteImage(ImageSkill, sPath);
        }
    }

    void SetImageBox(bool flag)
    {
        int nBoxSize = m_ListBox.Count;
        for (int i = 0; i < nBoxSize; i++)
        {
            Image ImageBox  = m_ObBox[i].GetComponent<Image>();
            int nBoxType    = m_ListBox[i];

            if(flag == false)   Module.SetSpriteImage(ImageBox, m_sBoxSpriteImage[nBoxType]);         
            else                Module.SetSpriteImage(ImageBox, m_sOpenBoxSpriteImage[nBoxType]);
        }       
    }

    int GetRandomBoxState()
    {
        int nRandNumber = Random.Range(0, 100);
        if (nRandNumber <= 20) return 4;
        if (nRandNumber <= 40) return 2;
        return 0;
    }

    void AddRandomBox(int nSize, int nMaxLevel)
    {
        for(int i = 0; i < nSize; i++) m_ListBox.Add(Random.Range(0, nMaxLevel + 1));
    }

    void AddRandomSkill()
    {
        int nSz = m_ListBox.Count;

        int nSkillSize = DataManage.InitData.instance.GetSkillSize();

        List<int> ArrayCreateSkillCnt = new List<int>();

        for (int i = 0; i < nSkillSize; i++) ArrayCreateSkillCnt.Add(0);

        for(int i = 0; i < nSz; i++)
        {
            int nSkill = 0, nType = 0;
            float fValue = 0f;
            SkillManager.instance.GetRandSkillStatus(ref nSkill, ref nType, ref fValue, ArrayCreateSkillCnt);
            fValue *= (1f + 0.1f * m_ListBox[i]);                                             // 박스의 레벨만큼 값 보정
            m_ListSkillFormat.Add(new SkillFormat(nSkill, nType, fValue));

            if (nType == (int)DefineManager.SKILL.SKILL_COUNT)
                ArrayCreateSkillCnt[nSkill]++;
        }
    }

    IEnumerator MoveLeftRight(int nIndex)
    {
        float gap = m_fLeftRightSize / m_nLeftRightNum;

        float tempLeftRightSize = m_fLeftRightSize;

        Vector3 vPosition = m_ObBox[nIndex].transform.position;

        for (int i = 0; i < m_nLeftRightNum; i++)
        {
            Vector3 vPositionLeft = vPosition + new Vector3(-tempLeftRightSize, 0, 0);
            Vector3 vPositionRight = vPosition + new Vector3(tempLeftRightSize, 0, 0);
            float fMax = 1e5f;
            while (Move(m_ObBox[nIndex], ref vPositionLeft, ref fMax)) yield return null;
            fMax = 1e5f;
            while (Move(m_ObBox[nIndex], ref vPositionRight, ref fMax)) yield return null;
            tempLeftRightSize -= gap;
        }

        m_ObBox[nIndex].transform.position = vPosition;
        StartCoroutine(UpSize(nIndex));

        // 광원 효과 같은것도 생각해보면 될 듯함.

        // 왼쪽 오른쪽 움직임이 다 끝나면..
        // 사이즈가 점점 커지면서 뚜껑이 열린 친구로 바꾸어준다!
    }

    IEnumerator UpSize(int nIndex)
    {
        Image tempImage = m_ObBox[nIndex].GetComponent<Image>();

        RectTransform memRect = tempImage.rectTransform;

        float memWidth = memRect.rect.width;
        float memHeight = memRect.rect.height;

        for (int i = 0; i < m_nUpcount; i++)
        {
            RectTransform rect = tempImage.rectTransform;
            float width = rect.rect.width;
            float height = rect.rect.height;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + m_fWidthChange);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + m_fHeightChange);
            yield return null;
        }

        SetImageBox(true);                  // 뚜껑이 열린 친구로 변환과 동시에
        SetActiveSkill(nIndex, true);       // 스킬 출력

        float time = 0f;

        while (time <= 3.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_nBoxOpenEnd--;

        if(m_nBoxOpenEnd == 0) SetActiveButton(false);
        
        // 다른곳에서 원상복구 시켜서 해줄필요 없음.
        //memRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, memWidth);
        //memRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, memHeight);
    }

    public void OpenBoxAdd(int nSize)
    {
        m_TreasureBoxQueue.Enqueue(nSize);
    }

    public void OpenBox(int nSize)
    {
        SetActiveButtonSide(false);
        SetSizeChange(true);
        int nBoxSize = m_ListBox.Count;
        //Debug.Log("nBoxSize : " + nBoxSize);
        int nSz = Mathf.Min(nSize, nBoxSize);
        for (int i = 0; i < nSz; i++) PrintEachBox(i);

        if(nBoxSize > 3)
            StartCoroutine(WaitOpenBox());
    }

    IEnumerator WaitOpenBox()
    {
        float fTime = 0;
        while(fTime <= 3f)
        {
            fTime += Time.deltaTime;
            yield return null;    
        }

        // 추가적인 이벤트 모션.

        PrintEachBox(3);
        PrintEachBox(4);
    }

    void SetSizeChange(bool bFlag)
    {
        int nSize = m_ObBox.Length;
        for(int i = 0; i < nSize; i++)        
            m_ObBox[i].GetComponent<AutoSizeChange>().SetStop(bFlag);        
    }

    public void SetApplySkill()
    {
        int nSz = m_ListSkillFormat.Count;
        for(int i = 0; i < nSz; i++)
        {
            int nSkill = m_ListSkillFormat[i].m_nSkill;
            int nType = m_ListSkillFormat[i].m_nType;
            float fValue = m_ListSkillFormat[i].m_fValue;
            SkillManager.instance.SkillJudge(nSkill, nType, fValue);
        }
        m_ListBox.Clear();
        m_ListSkillFormat.Clear();
    }

    bool Move(GameObject ob, ref Vector3 to , ref float fMax)
    {
        float dis = Vector3.Distance(to, ob.transform.position);
        if (fMax <= dis)
        {
            ob.transform.position = to;
            return false;
        }
        else fMax = dis;

        if (dis > 0.0001f)
        {

            ob.transform.position += (to - ob.transform.position).normalized  * m_fSpeed;

            //ob.transform.Translate((to - ob.transform.position).normalized * Time.deltaTime * m_fSpeed);
        }
        else return false;

        return true;
    }

    public void SetTreasureStartState(bool flag)
    {
        m_bStarted = flag;
    }
}
