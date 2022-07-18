using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameMananger : MonoBehaviour
{
    static public PlayGameMananger instance = null;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);
        init();
    }

    void init()
    {
        PlayingGameManager.ClearGAmeState();
        InitPlayGameData();
    }

    void InitPlayGameData()
    {
        DataManage.DataManager.instance.Level = 1;
        DataManage.DataManager.instance.Exe = 0;
        DataManage.DataManager.instance.InGameGold = 0;
    }

    public void AddEx(int nEx)
    {
        DataManage.DataManager.instance.Exe += nEx;

        int maxEx = DataManage.ValueManager.instance.GetLevelEx(DataManage.DataManager.instance.Level);
        if (DataManage.DataManager.instance.Exe >= maxEx)
        {
            DataManage.DataManager.instance.Exe %= maxEx;
            DataManage.DataManager.instance.Level++;           // ���� ����
            GameUIManager.instance.PlayerLevelUp(); // ������ ����
            // ������ UI ���
        }
    }

    public void ExitGame()
    {
        GameUIManager.instance.SetGameOverFunc(() =>
        {
            GameUIManager.instance.SetActiveEndGame(true);
            GameUIManager.instance.SetActiveGameOver(false);
        });
        GameUIManager.instance.SetActiveGameOver(true);
    }

    // Update is called once per frame
}
