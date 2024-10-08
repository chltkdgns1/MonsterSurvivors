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
        m_TextList[0].text = Module.GetTimeText((int)PlayTimeManager.instance.GetTime());
        m_TextList[1].text = Module.GetLevelText(DataManage.DataManager.instance.Level);
        m_TextList[2].text = Module.GetKillMonsterText(DataManage.DataManager.instance.KillMonster);
        m_TextList[3].text = Module.GetDamageText(SkillManager.instance.GetAllSumDamage());
    }

    private void Start()
    {
        m_bInit = true;
        m_TextList[0].text = Module.GetTimeText((int)PlayTimeManager.instance.GetTime());
        m_TextList[1].text = Module.GetLevelText(DataManage.DataManager.instance.Level);
        m_TextList[2].text = Module.GetKillMonsterText(DataManage.DataManager.instance.KillMonster);
        m_TextList[3].text = Module.GetDamageText(SkillManager.instance.GetAllSumDamage());
    }
}
