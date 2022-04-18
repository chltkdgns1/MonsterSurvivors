using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationTypeDataClass
{
    private int m_Id;
    private GameObject m_Ob ;
    private string [] m_sParameter;

    //파라미터의 맨 처음 값은 구분자로 활용한다.

    public int GetId()
    {
        return m_Id;
    }

    public GameObject GetGameObject()
    {
        return m_Ob;
    }

    public void SetParameter(string[] str)
    {
        m_sParameter = str;
    }

    public string[] GetParameter()
    {
        return m_sParameter;
    }

    public string GetParamIndex(int index)
    {
        if (index < 0 || m_sParameter.Length >= index) return "";           
        return m_sParameter[index];
    }

    public int GetParamSize()
    {
        return m_sParameter.Length;
    }


    public CommunicationTypeDataClass() {
        m_Id = -1;
        m_Ob = null;
   
    }

    public CommunicationTypeDataClass(int id)
    {
        m_Id = id;
        m_Ob = null;
    }

    public CommunicationTypeDataClass(int id,GameObject ob)
    {
        m_Id = id;
        m_Ob = ob;
        //m_sParameter = parameter;
    }

    public CommunicationTypeDataClass(int id, GameObject ob,string []parameter)
    {
        m_Id = id;
        m_Ob = ob;
        m_sParameter = parameter;
    }
}
