using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveInfo
{
    public int number;
    public int state;
    public string record;
}
public class ClearInfo
{
    public int map;
    public int level;
    public int state;
    public float psersonReocrd;
    public float goldRecord;
    public float silverRecord;
    public float bronzeRecord;
}

public class Settings{
    static public List<SaveInfo> saveInfo4 = new List<SaveInfo>();
    static public List<SaveInfo> saveInfo6 = new List<SaveInfo>();
    static public List<SaveInfo> saveInfo8 = new List<SaveInfo>();
    static public List<SaveInfo> _saveInfo0 = new List<SaveInfo>();
    static public List<ClearInfo> clearInfo = new List<ClearInfo>();

    static public List<Dictionary<string, object>> avgRecordList; //기록 리스트


    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    // 배열 초기화
    public static void InitData(int map)
    {
        Debug.Log("배열 최초 초기화");
        var binaryFormatter = new BinaryFormatter();
        var memoryStream = new MemoryStream();
        for (int i = 1; i <= 200; i++)
        {
            switch(map)
            {
                case 4:
                    saveInfo4.Add(new SaveInfo
                    {
                        number = i,
                        state = 0,
                        record = "0"
                    });
                    if(i == 200)
                    {
                        binaryFormatter.Serialize(memoryStream, saveInfo4);
                        PlayerPrefs.SetString("SaveInfo4", Convert.ToBase64String(memoryStream.GetBuffer()));
                    }

                    break;
                case 6:
                    saveInfo6.Add(new SaveInfo
                    {
                        number = i,
                        state = 0,
                        record = "0"
                    });
                    if (i == 200)
                    {
                        binaryFormatter.Serialize(memoryStream, saveInfo6);
                        PlayerPrefs.SetString("SaveInfo6", Convert.ToBase64String(memoryStream.GetBuffer()));
                    }

                    break;
                default:
                    saveInfo8.Add(new SaveInfo
                    {
                        number = i,
                        state = 0,
                        record = "0"
                    });
                    if (i == 200)
                    {
                        binaryFormatter.Serialize(memoryStream, saveInfo8);
                        PlayerPrefs.SetString("SaveInfo8", Convert.ToBase64String(memoryStream.GetBuffer()));
                    }


                    break;
            }
        }
    }
    // 데이터 저장
    public static void AddSaveData(int map, int number, int state, string record)
    {
        Debug.Log("저장");
        //bool saving = false;
        bool saving = true; // 중복 저장 가능하도록
        var binaryFormatter = new BinaryFormatter();
        var memoryStream = new MemoryStream();
        switch (map)
        {
            case 4:
                if(saveInfo4[number - 1].state == 0)
                {
                    saving = true;
                    saveInfo4[number - 1].state = state;
                    saveInfo4[number - 1].record = record;
                    binaryFormatter.Serialize(memoryStream, saveInfo4);
                    PlayerPrefs.SetString("SaveInfo4", Convert.ToBase64String(memoryStream.GetBuffer()));
                }

                break;
            case 6:
                if (saveInfo6[number - 1].state == 0)
                {
                    saving = true;
                    saveInfo6[number - 1].state = state;
                    saveInfo6[number - 1].record = record;
                    binaryFormatter.Serialize(memoryStream, saveInfo6);
                    PlayerPrefs.SetString("SaveInfo6", Convert.ToBase64String(memoryStream.GetBuffer()));
                }

                break;
            default:
                if(saveInfo8[number - 1].state == 0)
                {
                    saving = true;
                    saveInfo8[number - 1].state = state;
                    saveInfo8[number - 1].record = record;
                    binaryFormatter.Serialize(memoryStream, saveInfo8);
                    PlayerPrefs.SetString("SaveInfo8", Convert.ToBase64String(memoryStream.GetBuffer()));
                }

                break;
        }
        if(saving == true)
        {
            saving = false;
            DataBaseControl.Instant.db.SetAddRecord(map, number, record, state);
        }

    }

    // 해당 유저가 신규 유저일 경우 SAVE 정보 초기화
    public static void CheckNRU()
    {
        
        if (PlayerPrefs.GetInt("nru") == 0)
        {
            Debug.Log("신규 유저");
            InitData(4);
            InitData(6);
            InitData(8);
            PlayerPrefs.SetInt("nru", 1);
        }
        else
        {
            Debug.Log("기존 유저");
        }
    }
    // 로드
    public static void Load()
    {
        saveInfo4 = LoadSaveInfo(4);
        saveInfo6 = LoadSaveInfo(6);
        saveInfo8 = LoadSaveInfo(8);
        _saveInfo0 = LoadSaveInfo(MainCanvas.Main.levelShowScript.mapSelecting);
    }
    public static void InstantLoad()
    {
        _saveInfo0 = LoadSaveInfo(MainCanvas.Main.levelShowScript.mapSelecting);
    }
    public static List<SaveInfo> LoadSaveInfo(int map)
    {
        var             _saveInfo = "";
        List<SaveInfo>  saveInfo = new List<SaveInfo>();

        switch (map)
        {
            case 4:
                _saveInfo = PlayerPrefs.GetString("SaveInfo4");
                break;
            case 6:
                _saveInfo = PlayerPrefs.GetString("SaveInfo6");
                break;
            default:
                _saveInfo = PlayerPrefs.GetString("SaveInfo8");
                break;
        }
        if (!string.IsNullOrEmpty(_saveInfo))
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream(Convert.FromBase64String(_saveInfo));
            saveInfo = (List<SaveInfo>)binaryFormatter.Deserialize(memoryStream);
        }
        else
        {
            saveInfo = new List<SaveInfo>();
        }
        
        return saveInfo;
    }

    // 저장 정보 클리어
    public static void Clear()
    {
        PlayerPrefs.SetInt("nru", 0);
        saveInfo4.Clear();
        saveInfo6.Clear();
        saveInfo8.Clear();
        var binaryFormmater = new BinaryFormatter();
        var memoryStream = new MemoryStream();
        binaryFormmater.Serialize(memoryStream, saveInfo4);
        PlayerPrefs.SetString("SaveInfo4", Convert.ToBase64String(memoryStream.GetBuffer()));

        binaryFormmater.Serialize(memoryStream, saveInfo6);
        PlayerPrefs.SetString("SaveInfo6", Convert.ToBase64String(memoryStream.GetBuffer()));

        binaryFormmater.Serialize(memoryStream, saveInfo8);
        PlayerPrefs.SetString("SaveInfo8", Convert.ToBase64String(memoryStream.GetBuffer()));
    }


    // 기록 조회
    public static int GetAvgRecord(int map, int level)
    {
        int i = 0;
        foreach(var lt in avgRecordList)
        {
            if(lt["fdMap"].ToString() == map.ToString() && lt["fdLevel"].ToString() == level.ToString())
            {
                i++;
                return i -1 ;
                //return float.Parse(lt["fdAvg"].ToString());
            }
        }
        return -1;
        //return 0.0f;
    }












    public static void SetMaxLevel(int index, int value)
    {
        PlayerPrefs.SetInt("maxlevel" + index.ToString(), value);
    }
    public static int GetMaxLevel(int index)
    {
        if(!Settings.HasKey("maxlevel" + index.ToString()))
        {
            Settings.SetMaxLevel(index, 1);
        }
        return PlayerPrefs.GetInt("maxlevel" + index.ToString());
    }

}
