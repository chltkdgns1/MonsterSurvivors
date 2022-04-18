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
        PlayingGameManager.InitGameState();
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        //PlayingGameManager.SetGameState(DefineManager.PLAYING_STATE_NOMAL);
        //Debug.Log("¼³Á¤ Àû¿ëµÊ?");
    }

    void Start()
    {
 
    }

    // Update is called once per frame
}
