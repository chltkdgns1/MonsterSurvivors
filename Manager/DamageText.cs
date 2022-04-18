using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    Text text;
    Color alpha;
    private int damage;
    private bool m_fInitEnd;

    private RectTransform m_Rect;

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
        StartCoroutine(PrintText());
    }

    public void SetDamage(int nDamage)
    {
        damage = nDamage;
        m_fInitEnd = true;
    }

    IEnumerator PrintText()
    {
        while (m_fInitEnd == false) yield return null;

        float fFontSz = 40f;
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
                fFontSz += Time.deltaTime * 40;
                //Debug.Log("fFontSz : " + fFontSz);
                text.text = "<color=white><size=" + fFontSz.ToString() + ">" + damage.ToString() + "</size></color>";
            }
            else
            {
                fFontSz -= Time.deltaTime * 40;
                text.text = "<color=white><size=" + fFontSz.ToString() + ">" + damage.ToString() + "</size></color>";
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

