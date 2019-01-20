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
[Serializable]
public class ClearInfo
{
    public int map;
    public int level;
    public int state;
    public string date;
    public string personReocrd;
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
    static public List<Dictionary<string, object>> mainRecordData; //메인기록 리스트

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
                case 100:
                    clearInfo.Add(new ClearInfo
                    {
                        map = 0,
                        level = 0,
                        state = 0,
                        goldRecord = 0.0f,
                        silverRecord = 0.0f,
                        bronzeRecord = 0.0f,
                        personReocrd = "0.0",
                        date = DateTime.Now.ToString("yyyy-MM-dd")
                    });
                    if(i == 1)
                    {
                        binaryFormatter.Serialize(memoryStream, clearInfo);
                        PlayerPrefs.SetString("ClearInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
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
            //DataBaseControl.Instant.db.SetAddMainRecord(map, number, record);
        }

    }
    // 골드, 실버, 브론즈에 대한 기록 저장
    public static void WorldRecordSave(int map, int number, int state, string rec, float goldRec, float silverRec, float bronzeRec)
    {
        Debug.Log("월드 레코드 저장 [" + map + "][" + number + "]");

        if (clearInfo.Exists(e => e.map == map && e.level == number))
        {

            //있으면,
            int num = clearInfo.FindIndex(e => e.map == map && e.level == number);
            Debug.Log("월드 레코드 존재 : num : ["+num+"]");
            //갱신
            clearInfo[num].personReocrd = rec;
            clearInfo[num].goldRecord = goldRec;
            clearInfo[num].silverRecord = silverRec;
            clearInfo[num].bronzeRecord = bronzeRec;
            clearInfo[num].date = DateTime.Now.ToString("yyyy-MM-dd");
        }
        else
        {
            Debug.Log("월드 레코드 없음 : "+ clearInfo.Count);
            //없으면,
            
            clearInfo.Add(new ClearInfo
            {
                map = map,
                level = number,
                state = state,
                goldRecord = goldRec,
                silverRecord = silverRec,
                bronzeRecord = bronzeRec,
                personReocrd = rec,
                date = DateTime.Now.ToString("yyyy-MM-dd")
            });
           
        }
        var binaryFormatter = new BinaryFormatter();
        var memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, clearInfo);
        PlayerPrefs.SetString("ClearInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        Debug.Log("클리어 정보 배열 저장 완료 : " + clearInfo.Count);
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
            InitData(100);                  // ClearInfo Init
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
        clearInfo = LoadClaerInfo();
        saveInfo4 = LoadSaveInfo(4);
        saveInfo6 = LoadSaveInfo(6);
        saveInfo8 = LoadSaveInfo(8);
        _saveInfo0 = LoadSaveInfo(MainCanvas.Main.levelShowScript.mapSelecting);
        Debug.Log("클리어 인포 배열 갯수 : " + clearInfo.Count);
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
    // 클리어 정보 호출
    public static List<ClearInfo> LoadClaerInfo()
    {
        var _clearInfo = "";
        List<ClearInfo> tmpClearInfo = new List<ClearInfo>();
        _clearInfo = PlayerPrefs.GetString("ClearInfo");
        if (!string.IsNullOrEmpty(_clearInfo))
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream(Convert.FromBase64String(_clearInfo));
            tmpClearInfo = (List<ClearInfo>)binaryFormatter.Deserialize(memoryStream);
        }
        else
        {
            tmpClearInfo = new List<ClearInfo>();
        }
        return tmpClearInfo;
    }

    // 저장 정보 클리어
    public static void Clear()
    {
        Debug.Log("전체 저장정보 클리어");
        PlayerPrefs.SetInt("nru", 0);
        saveInfo4.Clear();
        saveInfo6.Clear();
        saveInfo8.Clear();
        clearInfo.Clear();

        var binaryFormmater = new BinaryFormatter();
        var memoryStream = new MemoryStream();
        binaryFormmater.Serialize(memoryStream, saveInfo4);
        PlayerPrefs.SetString("SaveInfo4", Convert.ToBase64String(memoryStream.GetBuffer()));

        binaryFormmater.Serialize(memoryStream, saveInfo6);
        PlayerPrefs.SetString("SaveInfo6", Convert.ToBase64String(memoryStream.GetBuffer()));

        binaryFormmater.Serialize(memoryStream, saveInfo8);
        PlayerPrefs.SetString("SaveInfo8", Convert.ToBase64String(memoryStream.GetBuffer()));

        binaryFormmater.Serialize(memoryStream, clearInfo);
        PlayerPrefs.SetString("ClearInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
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
