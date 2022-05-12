using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileIOManager
{
    private static string m_FilePath = @"E:\LogFile";

    public static void WriteLog(string msg)
    {
#if UNITY_EDITOR
        string year         = DateTime.Now.ToString("yyyy");
        string month        = DateTime.Now.ToString("MM");
        string day          = DateTime.Now.ToString("dd");

        string time         = DateTime.Now.ToString("HH:mm:ss");

        string yearDir      = m_FilePath + "\\" + year;
        string monthDir     = yearDir + "\\" + month;
        string dayDir       = monthDir + "\\" + day;

        if(Directory.Exists(yearDir) == false)
            Directory.CreateDirectory(yearDir);

        if (Directory.Exists(monthDir) == false)
            Directory.CreateDirectory(monthDir);

        if (Directory.Exists(dayDir) == false)
            Directory.CreateDirectory(dayDir);

        string txtPath = dayDir + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "_Log.txt";

        StreamWriter writer;
        writer = File.AppendText(txtPath);
        writer.WriteLine(time + " : " + msg);
        writer.Close();
#endif
    }


    //void Start()
    //{
    //    if (!File.Exists(m_FilePath))
    //    {
    //        using (StreamWriter sw = File.CreateText(m_FilePath))
    //        {
    //            sw.WriteLine("Hello");
    //            sw.WriteLine("And");
    //            sw.WriteLine("Welcome");
    //        }
    //    }

    //    using (StreamReader sr = File.OpenText(m_FilePath))
    //    {
    //        string s;
    //        while ((s = sr.ReadLine()) != null)
    //        {
    //            Debug.Log(s);
    //        }
    //    }
    //}
}
