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

    // Update is called once per frame
}
