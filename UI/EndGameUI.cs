using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField]
    private Text[] m_TextList;

    private bool m_bInit = false;
    private void OnEnable()
    {
        if (m_bInit == false) return;
        m_TextList[0].text = GetTimeText();
        m_TextList[1].text = GetLevelText();
        m_TextList[2].text = GetKillMonsterText();
        m_TextList[3].text = GetDamageText();
    }

    private void Start()
    {
        m_bInit = true;
        m_TextList[0].text = GetTimeText();
        m_TextList[1].text = GetLevelText();
        m_TextList[2].text = GetKillMonsterText();
        m_TextList[3].text = GetDamageText();
    }

    string GetTimeText()
    {
        int nTime = (int)GameUIManager.instance.GetTime();
        int nMinute = nTime / 60;
        int nSecond = nTime % 60;
        string sTime = nMinute + ":" + nSecond;

        if (nMinute >= 25) return "<color=#FF6464>" + sTime + "</color>";
        if (nMinute >= 15) return "<color=#64FF64>" + sTime + "</color>";
        if (nMinute >= 7)  return "<color=#6464FF>" + sTime + "</color>";
        return "<color=white>" + sTime + "</color>";
    }

    string GetLevelText()
    {
        int nLevel = PlayerOffline2D.instance.GetLevel();
        string sLevel = nLevel.ToString();

        if (nLevel >= 80) return "<color=#FF6464>" + sLevel + "</color>";
        if (nLevel >= 50) return "<color=#64FF64>" + sLevel + "</color>";
        if (nLevel >= 25) return "<color=#6464FF>" + sLevel + "</color>";
        return "<color=white>" + sLevel + "</color>";
    }


    string GetKillMonsterText()
    {
        int nKillMonster = PlayerOffline2D.instance.GetKillMonster();
        string sKillMonster = nKillMonster.ToString();

        if (nKillMonster >= 10000)  return "<color=#FF6464>" + sKillMonster + "</color>";
        if (nKillMonster >= 1000)   return "<color=#64FF64>" + sKillMonster + "</color>";
        if (nKillMonster >= 250)    return "<color=#6464FF>" + sKillMonster + "</color>";
        return "<color=white>" + sKillMonster + "</color>";
    }

    string GetDamageText()
    {
        float fDamage = SkillManager.instance.GetAllSumDamage();
        string sDamage = fDamage.ToString();

        if (fDamage >= 1e9) return "<color=#FF6464>" + (fDamage * 0.000000001f).ToString("F1") + "G" + "</color>";
        if (fDamage >= 1e6) return "<color=#64FF64>" + (fDamage * 0.000001f).ToString("F1") + "M" + "</color>";
        if (fDamage >= 1e3) return "<color=#6464FF>" + (fDamage * 0.001f).ToString("F1") + "K" + "</color>";
        return "<color=white>" + fDamage.ToString("F1") + "</color>";
    }
}
