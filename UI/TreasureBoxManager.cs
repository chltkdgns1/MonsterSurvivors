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


    //// Ʈ���� ���� �ܿ��� ����� ������Ʈ
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
        if(m_bStarted == false)    // ������ ���� ���� ������ ��쿡
        {
            if (m_TreasureBoxQueue.Count != 0) // ������� �ʴٸ�,
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
        // ������ �Ѱ��� ���.
        int nState = GetRandomBoxState();                               // ���� Ȯ������ üũ

        m_ListBox.Clear();
        m_ListSkillFormat.Clear();

        m_ListBox.Add(nBoxLevel);
        AddRandomBox(nState, nBoxLevel);                   // �ڽ� ���� ��� ����
        m_ListBox.Sort();                                             // �������� ����
        m_ListBox.Reverse();
        // ������ �̿��Ͽ� � �ڽ��� ����� ���������� ����.


        AddRandomSkill();                   // ������ �ڽ����� ���� ��ų, �ɷ�ġ
        SetImageBoxAndSkill();              // ��ų �̹��� ����
        SetSizeChange(false);
        PrintSequenceBox(3);

        //int nSize = m_ListBox.Count;
        //for(int i = 0; i < nSize; i++) PrintEachBox(i);
       
        // �ϴ� ��� �ڽ��� ���ؼ� ������ �س��´�. ���� ���, �ڽ��� �̹��������, �ڽ� ���� ��Ÿ�� ��ų�̶���� ���.


        // 1�� �Ǵ� 3����� �ڽ��� �¿�� ��鸮�鼭 �׳� �����.
        // �ڽ��� 5����� 3�� �ڽ��� �¿�� �����̴� ���߿� �߰� ����Ʈ �߻����� 5���� �߰� �����.
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

            if (nInSprite != 0) Module.SetSpriteImageAll(ImageSkill, sPath, nInSprite);          // ��������Ʈ �̹��� ���� �ִ� �̹������ 
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
            fValue *= (1f + 0.1f * m_ListBox[i]);                                             // �ڽ��� ������ŭ �� ����
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

        // ���� ȿ�� �����͵� �����غ��� �� ����.

        // ���� ������ �������� �� ������..
        // ����� ���� Ŀ���鼭 �Ѳ��� ���� ģ���� �ٲپ��ش�!
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

        SetImageBox(true);                  // �Ѳ��� ���� ģ���� ��ȯ�� ���ÿ�
        SetActiveSkill(nIndex, true);       // ��ų ���

        float time = 0f;

        while (time <= 3.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_nBoxOpenEnd--;

        if(m_nBoxOpenEnd == 0) SetActiveButton(false);
        
        // �ٸ������� ���󺹱� ���Ѽ� �����ʿ� ����.
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

        // �߰����� �̺�Ʈ ���.

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
