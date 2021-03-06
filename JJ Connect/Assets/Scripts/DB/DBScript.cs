﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class DBScript : MonoBehaviour
{

    //public static DBManager Instance;
    private string URL;
    public List<Dictionary<string, object>> data;
    public bool success = false;
    public bool mainRecordDataSuccess = false;
    public bool rankCountDataSuccess = false;
    public int rankCount;
    string UUID;
    int usernum;



    private void Awake()
    {
        //Instance = this;
        URL = "http://sambong0819.cafe24.com/CN";

        //URL = "http://sambong0819.cafe24.com/lotto";
        UUID = SystemInfo.deviceUniqueIdentifier;
    }

    private void Start()
    {
        GetUserAuth();
    }

    public void GetUserAuth()
    {
        WWWForm form = new WWWForm();
        form.AddField("UUID", UUID);
        resultFunction rf = new resultFunction(rf_GetUserAuth);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL +"/GetUserAuth.php", form, rf));

        //resultFunction rf = new resultFunction(LottoListResult);
        //StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/GetLottoList.php", form, rf));
    }

    /* 유저 회원가입 */
    public void SetUserAuth()
    {
        Debug.Log("아이디 삽입 :" + UUID);
        WWWForm form = new WWWForm();
        form.AddField("UUID", UUID);
        resultFunction rf = new resultFunction(SendResult);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/AddUserAuth.php", form, rf));
    }
    /* 유저 기록 정보 */
    public void SetAddRecord(int map, int level, string record, int state)
    {
        Debug.Log("기록 삽입");
        WWWForm form = new WWWForm();
        form.AddField("usernum", usernum);
        form.AddField("map", map);
        form.AddField("level", level);
        form.AddField("record", record);
        form.AddField("state", state);
        resultFunction rf = new resultFunction(SendResult);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/AddRecord.php", form, rf));
    }
    /* Main Record */
    public void SetAddMainRecord(int map, int level, string record)
    {
        Debug.Log("메인 기록 삽입 Map["+map+"]Level["+level+"]");
        WWWForm form = new WWWForm();
        form.AddField("map", map);
        form.AddField("level", level);
        form.AddField("record", record);
        resultFunction rf = new resultFunction(SendResult);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/AddMainRecord.php", form, rf));
    }
    /* 메인 기록 가져오기 */
    public void GetMainRecord(int map, int level)
    {
        //mainRecordDataSuccess = false;
        Debug.Log("메인 기록 가져오기 map =" + map + " / level = " + level);
        WWWForm form = new WWWForm();
        form.AddField("map", map);
        form.AddField("level", level);
        resultFunction rf = new resultFunction(rf_GetMainRecordData);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/GetMainRecordData.php", form, rf));
    }
    /* 내 순위보다 좋은 사람 가져오기 */
    public void GetRankCount(int map, int level, float record)
    {
        //rankCountDataSuccess = false;
        Debug.Log("순위 가져오기");
        WWWForm form = new WWWForm();
        form.AddField("map", map);
        form.AddField("level", level);
        form.AddField("record", record.ToString());
        resultFunction rf = new resultFunction(rf_GetRankCount);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/GetRankCount.php", form, rf));

    }

    public void GetRecordAvgList()
    {
        Debug.Log("기록 조회");
        WWWForm form = new WWWForm();
        form.AddField("Type", "avg");
        resultFunction rf = new resultFunction(rf_GetRecordAvfList);
        StartCoroutine(DataBaseControl.Instant.conn.SendData(URL + "/GetRecordAvgList.php", form, rf));
    }

    void rf_GetRecordAvfList()
    {
        ListResult();
        Settings.avgRecordList = data;
    }

    void rf_GetMainRecordData()
    {
        Debug.Log("[DB] 메인 레코드 데이터 가져오기");
        ListResult();
        Settings.mainRecordData = data;
        /*
        Debug.Log("겟 메인 레코드 데이터 개수 :" + data.Count);
        Debug.Log("DB 갯수 : " + Settings.mainRecordData[0]["fdCount"]);
        Debug.Log("평균기록 : " + Settings.mainRecordData[0]["fdRecAvg"]);
        */
        if(data.Count >= 1)
        {
            mainRecordDataSuccess = true;
        }
        
    }

    void rf_GetUserAuth()
    {
        //ListResult();
        if(DataBaseControl.Instant.conn._result == "")
        {
            Debug.Log("아이디 없음");
            SetUserAuth();
            GetUserAuth();


        }
        else
        {
            usernum = int.Parse(DataBaseControl.Instant.conn._result);
            Debug.Log("USER NUM : " + usernum);
        }
    }

    void rf_GetRankCount()
    {
        if(DataBaseControl.Instant.conn._result =="")
        {
            rankCount = 0;
            Debug.Log("[DB 값 획득 실패] rankCount = " + rankCount);
        }
        else
        {
            rankCount = int.Parse(DataBaseControl.Instant.conn._result) + 1;
            Debug.Log("[DB 값 획득] rankCount = " + rankCount);
            rankCountDataSuccess = true;
        }
        
        
    }

    void ListResult()
    {
        var list = new List<Dictionary<string, object>>();
        string[] lines = DataBaseControl.Instant.conn._result.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        var header = lines[0].Split(',');
        Debug.Log("기록 갯수 :" + lines.Length);
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;
            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }

        data = list;
        success = true;
        Debug.Log(data.Count);
    }
    void SendResult()
    {
        if(DataBaseControl.Instant.conn._result == "success")
        {
            Debug.Log("DB 작업 완료");
        }
        
    }


    /*
    public void GetLottoList()
    {
        Debug.Log("GetLottoList 실행 중");
        WWWForm form = new WWWForm();
        form.AddField("Get", "test");
        resultFunction rf = new resultFunction(LottoListResult);
        StartCoroutine(ConnectManager.getInst().SendData(URL + "/GetLottoList.php", form, rf));
    }
    */
    //로또 리스트 가져오기.
    void LottoListResult()
    {
        Debug.Log("LottoListResult 실행 중");
        var list = new List<Dictionary<string, object>>();
        //string[] lines = ConnectManager.getInst()._result.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] lines = DataBaseControl.Instant.conn._result.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        var header = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;
            var entry = new Dictionary<string, object>();
            for(var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        
        data = list;
        success = true;
        Debug.Log(data.Count);
    }

/*
    public void AddScore()
    {

        Debug.Log("Add Score");
        today = System.DateTime.Now.ToString("yyyy-MM-dd"); // 오늘 날짜
        nowScore = PlayerPrefs.GetFloat("nowscore"); // 현재 점수
        PlayerNickname = PlayerPrefs.GetString("nickname"); // 유저 닉네임

        WWWForm form = new WWWForm();

        form.AddField("nickname", PlayerNickname);
        form.AddField("nowscore", nowScore.ToString()); //문자열로 넘김
        form.AddField("today", today);
        //Debug.Log(PlayerNickname + "/" + nowScore.ToString() + "/" + today);

        resultFunction rf = new resultFunction(DebugLog);
        StartCoroutine(ConnectManager.getInst().SendData(URL + "/addScore.php", form, rf));
    }
    public void GetTodayRankList()
    {
        today = System.DateTime.Now.ToString("yyyy-MM-dd"); // 오늘 날짜

        WWWForm form = new WWWForm();
        form.AddField("today", today);
        //resultFunction rf = new resultFunction(ResultManager.Instance.ResultTodayRankMap);
       // StartCoroutine(ConnectManager.getInst().SendData(URL + "/getTodayScoreList.php", form, rf));
    }
    // Game Scene 에서만 출력 함. 위에껀 메인(인덱스 페이지)
    public void GetTodayRankList2()
    {
        today = System.DateTime.Now.ToString("yyyy-MM-dd"); // 오늘 날짜

        WWWForm form = new WWWForm();
        form.AddField("today", today);
        //resultFunction rf = new resultFunction(ResultManager.Instance.ResultTodayRankMapUINone);
        //StartCoroutine(ConnectManager.getInst().SendData(URL + "/getTodayScoreList.php", form, rf));
    }



    public void DebugLog()
    {
        Debug.Log(ConnectManager.getInst()._result);
    }
*/

}