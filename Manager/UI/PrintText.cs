using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    Text text;
    Color alpha;
    private string sPrintText;
    private bool m_fInitEnd;

    private RectTransform m_Rect;
    private string m_sColor;

    [SerializeField]
    private float m_fFontSize = 40f;

    // Start is called before the first frame update

    private void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Restart();
    }

    public void Restart()
    {
        moveSpeed = 40.0f;
        alphaSpeed = 40.0f;
        destroyTime = 2.0f;

        text = GetComponent<Text>();
        m_fInitEnd = false;
        StartCoroutine(PrintTextRoutine());
    }

    public void SetText(string sPrint, string sColor)
    {
        sPrintText = sPrint;
        m_fInitEnd = true;
        m_sColor = sColor;
    }

    IEnumerator PrintTextRoutine()
    {
        while (m_fInitEnd == false) yield return null;

        float fFontSz = m_fFontSize;
        float fTime = 0f;
        while(fTime <= 0.8f)
        {
            fTime += Time.deltaTime;
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
            text.color = alpha;

            //text.text = "<color=white><size=30>99</size></color>";

            if (fTime <= 0.4f)
            {
                fFontSz += Time.deltaTime * m_fFontSize;
                //Debug.Log("fFontSz : " + fFontSz);
                text.text = "<color="+ m_sColor + "><size=" + fFontSz.ToString() + ">" + sPrintText + "</size></color>";
            }
            else
            {
                fFontSz -= Time.deltaTime * m_fFontSize;
                text.text = "<color=" + m_sColor + "><size=" + fFontSz.ToString() + " > " + sPrintText + "</size></color>";
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

