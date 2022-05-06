using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValueManager : MonoBehaviour
{
    public static GameValueManager instance = null;

    private long m_nGold;
    private int m_nRestart;

    private void Awake()
    {
        if (instance == null)   instance = this;
        else                    Destroy(gameObject);

        init();
    }

    void init()
    {
        m_nGold = 0;
        m_nRestart = DefineManager.PLAYING_MAX_CONTINUE_COUNT;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRestart(int value) { m_nRestart = value; }
    public int GetRestart() { return m_nRestart; }

    public void AddGold(int nGold) {
        m_nGold += nGold;
        GameUIManager.instance.SetMoneyText(m_nGold.ToString());
        Vector3 position = GameUIManager.instance.GetMoneyTextPosition();
        PrintTextManager.instance.SetText(GameUIManager.instance.GetMoneyTextPosition() - new Vector3(0,-3,0), "+" + nGold, true);
    }
}
