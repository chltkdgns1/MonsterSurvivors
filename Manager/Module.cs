using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        return new Vector3(0,0,1) * Random.Range(0, 360);
    }

    static public Vector3 GetZAxisRandomXYRotation()
    {
        return new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
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
}
