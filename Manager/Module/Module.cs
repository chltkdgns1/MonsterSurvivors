using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public struct pair<T, A>
{
    public T first;
    public A second;

    public pair(T tFirst, A ASecond)
    {
        first = tFirst;
        second = ASecond;
    }
}

public class Module : MonoBehaviour
{
    static public void ChangeDirection(GameObject ob, bool flg)
    {
        Vector3 tmp = ob.transform.localScale;
        if (flg == false) tmp.x = Mathf.Abs(tmp.x);
        else tmp.x = -Mathf.Abs(tmp.x);
        ob.transform.localScale = tmp;
    }

    static public Vector3 GetZAxisRandomRatation()
    {
        return new Vector3(0,0,1) * UnityEngine.Random.Range(0, 360);
    }

    static public Vector3 GetZAxisRandomXYRotation()
    {
        return new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)).normalized;
    }

    static public float GetAngle(Vector3 vec1, Vector3 vec2)
    {
        float theta = Vector3.Dot(vec1, vec2) / (vec1.magnitude * vec2.magnitude);
        Vector3 dirAngle = Vector3.Cross(vec1, vec2);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        if (dirAngle.z < 0.0f) angle = 360 - angle;
        return angle;
    }

    static public void SetSpriteImage(Image ObImage, string imagePath)
    {
        if (ObImage == null) return;
        //Debug.Log("ImagePath : " + imagePath);
        ObImage.sprite = Resources.Load(imagePath, typeof(Sprite)) as Sprite;
    }

    static public void SetSpriteImageAll(Image ObImage, string imagePath, int index)
    {
        if (ObImage == null) return;
        Sprite[] temp = Resources.LoadAll<Sprite>(imagePath);
        if (temp.Length <= index || index < 0) return;
        ObImage.sprite = temp[index];
    }

    static public List<string> Split(string str, char ch)
    {
        int sz = str.Length;
        List<string> list = new List<string>();
        string temp = "";
        for(int i = 0; i< sz; i++)
        {
            if(str[i] == ch)
            {
                list.Add(temp);
                temp = "";
                continue;
            }
            temp += str[i];
        }
        if (temp.Length != 0) list.Add(temp);
        return list;
    }

    static public string MergeString(List<string> list, char ch , bool flag)
    {
        if (list.Count == 0) return "";
        string temp = "";
        for(int i = 0; i < list.Count; i++)
        {
            temp += list[i] + ch;
            if (i == list.Count - 1) break;
            if (flag) temp += "\n";
        }
        return temp;
    }


    static public bool GetSameSize(int[] size)
    {
        int sz = size.Length;

        if (sz == 0) return true;

        int value = size[0];
        for (int i = 1; i < sz; i++)
        {
            if (size[i] != value) return false;
        }

        return true;
    }

    static public int GetMinSize(int[] size)
    {
        int sz = size.Length;

        if (sz == 0) return DefineManager.INFINITE;

        int minValue = DefineManager.INFINITE;

        for (int i = 0; i < sz; i++)
        {
            minValue = Mathf.Min(minValue, size[i]);
        }

        return minValue;
    }

    static public void SetSize(RectTransform rect, float fWidth, float fHeight)
    {
        //Debug.LogError("fWidth : " + fWidth + " " + "fHeight : " + fHeight);
        float width = rect.rect.width;
        float height = rect.rect.height;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + fWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + fHeight);
    }

    static public string[] AddString(string[] from ,  string sAdd)
    {
        string [] to = new string[from.Length + 1];
        for(int i = 0; i < from.Length; i++)
        {
            to[i] = from[i];
        }
        to[from.Length] = sAdd;
        return to;
    }

    static public void CirclePosition(List<GameObject> obList, Vector3 vPosition, float fDistance)
    {
        int sz = obList.Count;
        float fAngle = 360f / sz;

        for(int i = 0; i < sz; i++)
        {
            float fCircleAng = fAngle * i;
            obList[i].transform.position = Quaternion.Euler(0, 0, fCircleAng) * Vector3.up * fDistance + vPosition;
        }
    }

    static public void GetRandPosition(Transform pTransform, float fWidth, float fHeight)
    {
        pTransform.transform.position = new Vector3(UnityEngine.Random.Range(-fWidth, fWidth), 
            UnityEngine.Random.Range(-fHeight, fHeight), pTransform.transform.position.z);
    }

    static public void GetRandPosition(Transform pTransform, Vector3 vPosition, float fDistance)
    {
        Vector3 vDir        = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        pTransform.position = vPosition + vDir.normalized * UnityEngine.Random.Range(1, Mathf.Max(fDistance, 1));
    }

    static public void GetMoneyString(ref string str)
    {
        int sz = str.Length;
        string sMoneyStr = "";
        for(int i = sz - 1, cnt = 0; i >= 0; i--,cnt++)
        {
            sMoneyStr += str[i];
            if (cnt == 3) sMoneyStr += ',';
            cnt %= 3;
        }
        GetReverseString(ref sMoneyStr);
        str = sMoneyStr;
    }

    static public string GetMoneyString(int nMoney)
    {
        string str = nMoney.ToString();
        int sz = str.Length;
        string sMoneyStr = "";
        for (int i = sz - 1, cnt = 0; i >= 0; i--, cnt++)
        {
            sMoneyStr += str[i];
            if (cnt == 3) sMoneyStr += ',';
            cnt %= 3;
        }
        GetReverseString(ref sMoneyStr);
        return sMoneyStr;
    }

    static public void GetReverseString(ref string str)
    {
        string sReverseStr = "";
        int sz = str.Length;
        for (int i = sz - 1; i >= 0; i--) sReverseStr += str[i];
        str = sReverseStr;
    }

    static public string GetLengthNumber(string str, int size)
    {
        if (str.Length >= size) return str;
        int diff = size - str.Length;
        string sAns = "";
        for (int i = 0; i < diff; i++) sAns += '0';
        return sAns + str;   
    }

    static public string GetJsonString(object ob)
    {
        return JsonUtility.ToJson(ob);
    }
    
    static public string GetHaxString()
    {
        string id = "";
        int randDomChar = UnityEngine.Random.Range(0, 24);
        long tempValue = (DateTime.Now.Ticks / 1000);
        // 알바벳 32 진수로 진행

        while (tempValue > 0)
        {
            long binary_32 = tempValue % 32;
            tempValue /= 32;

            if (binary_32 < 10) id += binary_32.ToString();
            else id += ((char)('A' + (binary_32 - 10))).ToString();

        }
        return '#' + id;
    }

    static public string GetPercentText(int value)
    {
        if (value >= 80) return "<color=#FF6464>" + value + "%</color>";
        if (value >= 60) return "<color=#64FF64>" + value + "%</color>";
        if (value >= 30) return "<color=#6464FF>" + value + "%</color>";
        return "<color=white>" + value + "%</color>";
    }

    static public string GetPercentText(float value)
    {
        if (value >= 80f) return "<color=#FF6464>" + value.ToString("F1") + "%</color>";
        if (value >= 60f) return "<color=#64FF64>" + value.ToString("F1") + "%</color>";
        if (value >= 30f) return "<color=#6464FF>" + value.ToString("F1") + "%</color>";
        return "<color=white>" + value.ToString("F1") + "%</color>";
    }

    static public string GetCountText(int value)
    {
        if (value >= 10) return "<color=#FF6464>" + value + "</color>";
        if (value >= 7) return "<color=#64FF64>" + value + "</color>";
        if (value >= 3) return "<color=#6464FF>" + value + "</color>";
        return "<color=white>" + value + "</color>";
    }

    static public string GetDamageText(int value)
    {
        if (value >= 50) return "<color=#FF6464>" + value + "</color>";
        if (value >= 30) return "<color=#64FF64>" + value + "</color>";
        if (value >= 15) return "<color=#6464FF>" + value + "</color>";
        return "<color=white>" + value + "</color>";
    }

    static public string GetDamageSumText(float value)
    {
        if (value >= 1e9) return "<color=#FF6464>" + (value * 0.000000001f).ToString("F1") + "G" + "</color>";
        if (value >= 1e6) return "<color=#64FF64>" + (value * 0.000001f).ToString("F1") + "M" + "</color>";
        if (value >= 1e3) return "<color=#6464FF>" + (value * 0.001f).ToString("F1") + "K" + "</color>";
        return "<color=white>" + value.ToString("F1") + "</color>";
    }

    static public string GetTimeText(int nTime)
    {

        int nMinute = nTime / 60;
        int nSecond = nTime % 60;
        string sTime = Module.GetLengthNumber(nMinute.ToString(), 2) + ":" + Module.GetLengthNumber(nSecond.ToString(), 2);

        if (nMinute >= 25) return "<color=#FF6464>" + sTime + "</color>";
        if (nMinute >= 15) return "<color=#64FF64>" + sTime + "</color>";
        if (nMinute >= 7) return "<color=#6464FF>" + sTime + "</color>";
        return "<color=white>" + sTime + "</color>";
    }

    static public string GetLevelText(int nLevel)
    {
        string sLevel = nLevel.ToString();

        if (nLevel >= 80) return "<color=#FF6464>" + sLevel + "</color>";
        if (nLevel >= 50) return "<color=#64FF64>" + sLevel + "</color>";
        if (nLevel >= 25) return "<color=#6464FF>" + sLevel + "</color>";
        return "<color=white>" + sLevel + "</color>";
    }


    static public string GetKillMonsterText(int nKillMonster)
    {
        string sKillMonster = nKillMonster.ToString();

        if (nKillMonster >= 10000) return "<color=#FF6464>" + sKillMonster + "</color>";
        if (nKillMonster >= 1000) return "<color=#64FF64>" + sKillMonster + "</color>";
        if (nKillMonster >= 250) return "<color=#6464FF>" + sKillMonster + "</color>";
        return "<color=white>" + sKillMonster + "</color>";
    }

    static public string GetDamageText(float fDamage)
    {
        string sDamage = fDamage.ToString();

        if (fDamage >= 1e9) return "<color=#FF6464>" + (fDamage * 0.000000001f).ToString("F1") + "G" + "</color>";
        if (fDamage >= 1e6) return "<color=#64FF64>" + (fDamage * 0.000001f).ToString("F1") + "M" + "</color>";
        if (fDamage >= 1e3) return "<color=#6464FF>" + (fDamage * 0.001f).ToString("F1") + "K" + "</color>";
        return "<color=white>" + fDamage.ToString("F1") + "</color>";
    }

    static public string GetHpText(float fHp)
    {
        if (fHp >= 50) return "<color=#FF6464>" + fHp + "</color>";
        if (fHp >= 300) return "<color=#64FF64>" + fHp + "</color>";
        if (fHp >= 800) return "<color=#6464FF>" + fHp + "</color>";
        return "<color=white>" + fHp + "</color>";
    }
}
