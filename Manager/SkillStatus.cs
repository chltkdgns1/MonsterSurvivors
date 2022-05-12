using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SkillStatus : MonoBehaviour
{
    private bool m_bFirst = false;
    private List<Text> m_TextValueList = new List<Text>();
    private List<Text> m_TextNameList = new List<Text>();
    private Image m_ImageSkillImage;
    [SerializeField]
    private bool m_bIsResult = false;
    private void Awake()
    {

    }

    void InitObject()
    {
        GameObject SkillData = transform.GetChild(0).GetChild(0).gameObject;    // 스킬 데이터
        GameObject SkillName = transform.GetChild(0).GetChild(1).gameObject;

        m_ImageSkillImage = SkillName.GetComponent<Image>();

        int nChildCount = SkillData.transform.childCount;
        int nChildNameCount = SkillName.transform.childCount;

        for (int i = 0; i < nChildCount; i++)
            m_TextValueList.Add(SkillData.transform.GetChild(i).GetChild(0).GetComponent<Text>());

        for (int i = 0; i < nChildNameCount; i++)
            m_TextNameList.Add(SkillName.transform.GetChild(i).GetComponent<Text>());


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(SkillStatusStruct value)
    {
        if (m_bFirst == false)
        {
            m_bFirst = true;
            InitObject();
        }

        int nInSprite = ValueManager.instance.GetSpriteInImage(value.m_nSkillType);
        string sPath = ValueManager.instance.GetSkillImagePath(value.m_nSkillType);

        if (nInSprite != 0) Module.SetSpriteImageAll(m_ImageSkillImage, sPath, nInSprite);          // 스프라이트 이미지 내에 있는 이미지라면 
        else Module.SetSpriteImage(m_ImageSkillImage, sPath);

        m_TextNameList[0].text = value.m_sSkillActive;
        m_TextNameList[1].text = value.m_sSkillName;

        if(m_bIsResult == true)
        {
            m_TextValueList[0].text = GetDamageText((int)value.m_fDamageValue);
            m_TextValueList[1].text = GetPercentText((int)(value.m_fCoolTimeValue * 100));
            m_TextValueList[2].text = GetPercentText((int)(value.m_fSizeUpValue * 100));
            m_TextValueList[3].text = GetDamageSumText(value.m_fSumDamage); 
            m_TextValueList[4].text = GetPercentText((int)(value.m_fNuckbackValue * 100));
            m_TextValueList[5].text = GetPercentText(value.m_fBloodValue * 100);
            m_TextValueList[6].text = GetCountText(value.m_nSkillCnt);
            m_TextValueList[7].text = GetDamageSumText(value.m_fDPS); 
            return;
        }

        m_TextValueList[0].text = GetDamageText((int)value.m_fDamageValue);
        m_TextValueList[1].text = GetPercentText((int)(value.m_fCoolTimeValue  * 100));
        m_TextValueList[2].text = GetPercentText((int)(value.m_fSizeUpValue    * 100));
        m_TextValueList[3].text = GetPercentText((int)(value.m_fNuckbackValue  * 100));
        m_TextValueList[4].text = GetPercentText(value.m_fBloodValue * 100);
        m_TextValueList[5].text = GetCountText(value.m_nSkillCnt);
    }

    string GetPercentText(int value)
    {
        if (value >= 80)    return "<color=#FF6464>" + value + "%</color>";
        if (value >= 60)    return "<color=#64FF64>" + value + "%</color>";
        if (value >= 30)    return "<color=#6464FF>" + value + "%</color>";
                            return "<color=white>" + value + "%</color>";
    }

    string GetPercentText(float value)
    {
        if (value >= 80f) return "<color=#FF6464>" + value.ToString("F1") + "%</color>";
        if (value >= 60f) return "<color=#64FF64>" + value.ToString("F1") + "%</color>";
        if (value >= 30f) return "<color=#6464FF>" + value.ToString("F1") + "%</color>";
                          return "<color=white>"   + value.ToString("F1") + "%</color>";
    }

    string GetCountText(int value)
    {
        if (value >= 10)    return "<color=#FF6464>"    + value + "</color>";
        if (value >= 7)     return "<color=#64FF64>"    + value + "</color>";
        if (value >= 3)     return "<color=#6464FF>"    + value + "</color>";
                            return "<color=white>"      + value + "</color>";
    }

    string GetDamageText(int value)
    {
        if (value >= 50)    return "<color=#FF6464>"    + value + "</color>";
        if (value >= 30)    return "<color=#64FF64>"    + value + "</color>";
        if (value >= 15)    return "<color=#6464FF>"    + value + "</color>";
                            return "<color=white>"      + value + "</color>";
    }

    string GetDamageSumText(float value)
    {
        if(value >= 1e9)    return "<color=#FF6464>"    + (value * 0.000000001f).ToString("F1") + "G" + "</color>";
        if(value >= 1e6)    return "<color=#64FF64>"    + (value * 0.000001f).ToString("F1") + "M" + "</color>";
        if(value >= 1e3)    return "<color=#6464FF>"    + (value * 0.001f).ToString("F1") + "K" + "</color>";
                            return "<color=white>"      + value.ToString("F1") + "</color>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
