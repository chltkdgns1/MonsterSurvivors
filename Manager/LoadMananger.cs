using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMananger
{
    public static GameObject LoadPrefabs(string prefab)
    {
        GameObject obj = Resources.Load(prefab) as GameObject;
        if (obj == null)
        {
            //Debug.LogError("ResourceLoader " + prefab + " Load Failed!");
        }
        return obj;
    }

}
