using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map : MonoBehaviour {
    public static String fileMapPath = "Data/Map";
    public static List<ObjectMap> listObjectMap4;
    public static List<ObjectMap> listObjectMap6;
    public static List<ObjectMap> listObjectMap8;

    public static void Loadmap4()
    {
        if (listObjectMap4 == null)
        {
            listObjectMap4 = new List<ObjectMap>();
        }
        else listObjectMap4.Clear();
        TextAsset mydata = Resources.Load(fileMapPath + 4) as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while((line = stringReader.ReadLine()) != null)
            {
                listObjectMap4.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }
    public static void Loadmap6()
    {
        if (listObjectMap6 == null)
        {
            listObjectMap6 = new List<ObjectMap>();
        }
        else listObjectMap6.Clear();
        TextAsset mydata = Resources.Load(fileMapPath + 6) as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                listObjectMap6.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }
    public static void Loadmap8()
    {
        if (listObjectMap8 == null)
        {
            listObjectMap8 = new List<ObjectMap>();
        }
        else listObjectMap8.Clear();
        TextAsset mydata = Resources.Load(fileMapPath + 8) as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                listObjectMap8.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }
    public static ObjectMap getObjectMap(int index, int map)
    {
        switch(index)
        {
            case 4:
                if(listObjectMap4 == null)
                {
                    listObjectMap4 = new List<ObjectMap>();
                }
                if(listObjectMap4.Count == 0)
                {
                    Debug.Log("Call : Loadmap4()");
                    Loadmap4();
                }
                if (map < listObjectMap4.Count)
                {
                    return listObjectMap4[map];
                }
                else
                {
                    Debug.Log(listObjectMap4[0]);
                    return listObjectMap4[0];
                }
                break;

            case 6:
                if (listObjectMap6 == null)
                {
                    listObjectMap6 = new List<ObjectMap>();
                }
                if (listObjectMap6.Count == 0)
                {
                    Debug.Log("Call : Loadmap6()");
                    Loadmap6();
                }
                if (map < listObjectMap6.Count)
                {
                    return listObjectMap6[map];
                }
                else
                {
                    Debug.Log(listObjectMap6[0]);
                    return listObjectMap6[0];
                }
                break;

            default:
                if (listObjectMap8 == null)
                {
                    listObjectMap8 = new List<ObjectMap>();
                }
                if (listObjectMap8.Count == 0)
                {
                    Debug.Log("Call : Loadmap8()");
                    Loadmap8();
                }
                if (map < listObjectMap8.Count)
                {
                    return listObjectMap8[map];
                }
                else
                {
                    Debug.Log(listObjectMap8[0]);
                    return listObjectMap8[0];
                }
                break;
        }
    }
}

public class ObjectMap
{
    public int[] listmap;
    public ObjectMap(string stringMap)
    {
        string[] strRowSplist = stringMap.Split(' ');
        listmap = new int[strRowSplist.Length];
        for(int i = 0; i < strRowSplist.Length -1; i++)
        {
            if("-1".Equals(strRowSplist[i]))
            {
                listmap[i] = -1;
            }
            else if("0".Equals(strRowSplist[i]))
            {
                listmap[i] = 0;
            }
            else
            {
                listmap[i] = int.Parse(strRowSplist[i]);
            }
        }
    }
}