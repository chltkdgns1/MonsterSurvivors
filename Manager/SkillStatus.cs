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

    public void SetData(DataManage.SkillStatusStruct value)
    {
        if (m_bFirst == false)
        {
            m_bFirst = true;
            InitObject();
        }

        int nInSprite = DataManage.InitData.instance.GetSpriteInImage(value.m_nSkillType);
        string sPath = DataManage.InitData.instance.GetSkillImagePath(value.m_nSkillType);

        if (nInSprite != 0) Module.SetSpriteImageAll(m_ImageSkillImage, sPath, nInSprite);          // 스프라이트 이미지 내에 있는 이미지라면 
        else Module.SetSpriteImage(m_ImageSkillImage, sPath);

        m_TextNameList[0].text = value.m_sSkillActive;
        m_TextNameList[1].text = value.m_sSkillName;

        if(m_bIsResult == true)
        {
            m_TextValueList[0].text = Module.GetDamageText((int)value.m_fDamageValue);
            m_TextValueList[1].text = Module.GetPercentText((int)(value.m_fCoolTimeValue * 100));
            m_TextValueList[2].text = Module.GetPercentText((int)(value.m_fSizeUpValue * 100));
            m_TextValueList[3].text = Module.GetDamageSumText(value.m_fSumDamage); 
            m_TextValueList[4].text = Module.GetPercentText((int)(value.m_fNuckbackValue * 100));
            m_TextValueList[5].text = Module.GetPercentText(value.m_fBloodValue * 100);
            m_TextValueList[6].text = Module.GetCountText(value.m_nSkillCnt);
            m_TextValueList[7].text = Module.GetDamageSumText(value.m_fDPS); 
            return;
        }

        m_TextValueList[0].text = Module.GetDamageText((int)value.m_fDamageValue);
        m_TextValueList[1].text = Module.GetPercentText((int)(value.m_fCoolTimeValue  * 100));
        m_TextValueList[2].text = Module.GetPercentText((int)(value.m_fSizeUpValue    * 100));
        m_TextValueList[3].text = Module.GetPercentText((int)(value.m_fNuckbackValue  * 100));
        m_TextValueList[4].text = Module.GetPercentText(value.m_fBloodValue * 100);
        m_TextValueList[5].text = Module.GetCountText(value.m_nSkillCnt);
    }
}
