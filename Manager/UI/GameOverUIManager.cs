using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBackFunc();

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_ObGameOver;
    private List<Vector2> m_vNormalPosition = new List<Vector2>();
    private List<Move2D> m_Move2DList       = new List<Move2D>();
    private int m_nHeight;
    private int m_nWidth;
    private int m_nMoveIndex;
    private bool m_bEnd;
    float m_fheightRatio;
    float m_fwidthRatio;

    private CallBackFunc m_Func = null;
 
    private void Awake()
    {
        m_nHeight = Screen.height + 100;
        m_nWidth = Screen.width / 2;
        m_nMoveIndex = 0;
        m_bEnd = false;

        m_fheightRatio = Screen.height / 1080.0f;
        m_fwidthRatio = Screen.width / 1920.0f;
        for (int i = 0; i < m_ObGameOver.Length; i++)
        {

            float xpos = (m_ObGameOver[i].transform.position.x - Screen.width / 2) * m_fheightRatio + Screen.width / 2;
            float ypos = (m_ObGameOver[i].transform.position.y - Screen.height / 2) * m_fheightRatio + Screen.height / 2;


            m_vNormalPosition.Add(new Vector2(xpos,ypos));
            m_Move2DList.Add(m_ObGameOver[i].GetComponent<Move2D>());
            m_ObGameOver[i].SetActive(false);
        }
    }

    public void SetFunc(CallBackFunc func)
    {
        m_Func = func;
    }

    private void OnEnable()
    {
        m_nMoveIndex = 0;
        m_bEnd = false;

        for (int i = 0; i < m_ObGameOver.Length; i++)
        {
            m_ObGameOver[i].transform.position = new Vector2(UnityEngine.Random.Range(m_nWidth - 100f, m_nWidth + 100f), (m_nHeight - Screen.height / 2) / m_fheightRatio + Screen.height / 2);
            m_ObGameOver[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bEnd) return;

        Move2D tempMove = m_Move2DList[m_nMoveIndex];
        if (tempMove == null) return;

        if (tempMove.Run(m_vNormalPosition[m_nMoveIndex]) == false)
        {
            m_ObGameOver[m_nMoveIndex].transform.position = m_vNormalPosition[m_nMoveIndex];
            m_nMoveIndex++;
        }
        if (m_nMoveIndex == m_ObGameOver.Length)
        {
            m_bEnd = true;
            if (m_Func != null)
            {
                m_Func();
                m_Func = null;
            }
        }
    }
}
