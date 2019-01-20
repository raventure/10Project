using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour {
    public RectTransform rec;
    public Text timer;
    public Text feedback1;
    public Text feedback2;
    public Text feedback3;
    public Text GlobalAvg;
    public Image runBar;
    public Image clearLevel1;
    public Image clearLevel2;
    public Image clearLevel3;

    int clearLevel;
    //int numberStar;
    int level;
    bool checkRecord;
    float avg;
    float std;
    float goldRecord;
    float silverRecord;
    float bronzeRecord;


    float globalRecAvg;
    float clearTimer;
    int totalCount;
    int rankCount;

    bool UIUpdate;

    Color32 defaultColor = new Color32(200, 200, 200, 255);
    Color32 Level1Color = new Color32(245, 177, 255, 255);
    Color32 Level2Color = new Color32(210, 177, 255, 255);
    Color32 Level3Color = new Color32(187, 177, 255, 255);

    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
        
    }
    public void GameWin()
    {


        // 1. 세팅 
        UIUpdate = true;
        clearLevel = 0;
        MainState.SetState(MainState.State.GameOver);
        int currentLevel = MainCanvas.Main.startGameScript.currentLevel;
        int currentMap = MainCanvas.Main.startGameScript.currentMap;
        int state = 1;
        clearTimer = float.Parse(MainCanvas.Main.barScript.timer0);

        // 2. 메인 기록 세이브 
        DataBaseControl.Instant.db.SetAddMainRecord(currentMap, currentLevel, MainCanvas.Main.barScript.timer0);

        // 3. 데이터 삽입 및 가져오기
        DataBaseControl.Instant.db.GetRankCount(currentMap, currentLevel, clearTimer); // 내 기록 보다 좋은 사람 Get
        DataBaseControl.Instant.db.GetMainRecord(currentMap, currentLevel); // 메인 데이터 가져오기
        
        // U.I 갱신
        {
            timer.text = "'" + MainCanvas.Main.barScript.timer0; // 타이머 기록 갱신
            clearLevel1.color = defaultColor;
            clearLevel2.color = defaultColor;
            clearLevel3.color = defaultColor;
        }


        if (Settings.avgRecordList.Count >= 2)
        {
            checkRecord = true;
        }
        if(checkRecord)
        {
            int pos;
            pos = Settings.GetAvgRecord(currentMap, currentLevel);
            //Debug.Log(currentMap +"/"+ currentLevel +"/ pos :" + pos);
            if(pos >= 0)
            {
                avg = float.Parse(Settings.avgRecordList[pos]["fdAvg"].ToString());
                std = float.Parse(Settings.avgRecordList[pos]["fdStd"].ToString());

                if (std == 0)
                {
                    std = avg * 0.1f;
                }

            }
            else
            {
                
                avg = float.Parse(MainCanvas.Main.barScript.timer0) - Random.Range(0.005f, 0.014f);
                std = avg * 0.05f;
            }
            goldRecord = Mathf.Round((avg - std) * 1000) / 1000;
            silverRecord = Mathf.Round((avg) * 1000) / 1000;
            bronzeRecord = Mathf.Round((avg + std) * 1000) / 1000;

        }
        
        SetActive(true);
        //캔버스 스탑 타이머
        MainAudio.Main.PlaySound(TypeAudio.SoundWin);
        // 3번째 인자값은 뜨는 연출 시간.
        StartCoroutine(EffectControl.MoveAnchor(rec, GameDefine.posCenter, 0.0f, null));

        if(currentLevel >= Settings.GetMaxLevel(currentMap) && Settings.GetMaxLevel(currentMap) < 200)
        {
            Settings.SetMaxLevel(currentMap, currentLevel + 1);
        }


        /*
        if(float.Parse(MainCanvas.Main.barScript.timer0) <= (avg - std))
        {
            state = 4;
            medalImg[0].color = new Color32(255, 215, 0, 255);
            medalImg[1].color = new Color32(255, 255, 255, 255);
            medalImg[2].color = new Color32(255, 255, 255, 255);
            feedback.text = "PERFECT!";
        }
        else if(float.Parse(MainCanvas.Main.barScript.timer0) <= avg)
        {
            state = 3;
            medalImg[0].color = new Color32(255, 255, 255, 255);
            medalImg[1].color = new Color32(211, 211, 211, 255);
            medalImg[2].color = new Color32(255, 255, 255, 255);
            feedback.text = "GREAT!";
        }
        else if (float.Parse(MainCanvas.Main.barScript.timer0) <= (avg+std))
        {
            state = 2;
            medalImg[0].color = new Color32(255, 255, 255, 255);
            medalImg[1].color = new Color32(255, 255, 255, 255);
            medalImg[2].color = new Color32(205, 127, 50, 255);
            feedback.text = "GOOD!";
        }
        else
        {
            state = 1;
            medalImg[0].color = new Color32(255, 255, 255, 255);
            medalImg[1].color = new Color32(255, 255, 255, 255);
            medalImg[2].color = new Color32(255, 255, 255, 255);
            feedback.text = "NOT BAD.";
        }
        */
        Save(currentMap, currentLevel, state, MainCanvas.Main.barScript.timer0);
        Settings.WorldRecordSave(currentMap, currentLevel, state, MainCanvas.Main.barScript.timer0, goldRecord, silverRecord, bronzeRecord);
        
    }

    void Update()
    {
        // 동적 U.I처리를 위한 것들 (DB에서 받아오는 시간이 걸려 업데이트로 지속 처리
        if (UIUpdate)
        {

            GlobalAvg.text = "Wait...";
            feedback1.text = "Wait...........";
            feedback2.text = "Wait...";
            feedback3.text = "Wait...";
            runBar.fillAmount = 0.0f;
            rankCount = 0;
            totalCount = 0;

            // 전체 랭킹을 가져온지 체크.
            if (DataBaseControl.Instant.db.rankCountDataSuccess)
            {
                rankCount = DataBaseControl.Instant.db.rankCount;
            }

            // 메인 기록을 가져왔는지 체크.
            if (DataBaseControl.Instant.db.mainRecordDataSuccess)
            {
                globalRecAvg = float.Parse(Settings.mainRecordData[0]["fdRecAvg"].ToString());
                totalCount = int.Parse(Settings.mainRecordData[0]["fdCount"].ToString()) + 1;
            }
            // 정상 값이 들어올 경우 U.I 업데이트 중지
            if(rankCount >= 1 && totalCount >= 1)
            {
                UIUpdate = false;
            }
        }
        else
        {
            float userPer = (float)rankCount / (float)totalCount; 
            // U.I 기록
            GlobalAvg.text = "Global Average '" + globalRecAvg; //글로벌 기록
            feedback2.text = rankCount + " / " + totalCount;
            runBar.fillAmount = 1 - userPer;
            feedback1.text = "belong to " + Mathf.Round((userPer*100)).ToString("N2") + "% world record";
            if (userPer <= 0.3)
            {
                clearLevel = 3;
                clearLevel1.color = Level1Color;
            }
            else if( userPer <= 0.5)
            {
                clearLevel = 2;
                clearLevel2.color = Level2Color;
            }
            else if( userPer <= 0.7)
            {
                clearLevel = 1;
                clearLevel3.color = Level3Color;
            }
            if(clearTimer <= globalRecAvg)
            {
                feedback3.text = "↑ '0." + Mathf.Round((globalRecAvg - clearTimer)* 1000.0f) +" Sec";
                feedback3.color = new Color32(241, 110, 125, 255);
            }
            else
            {
                feedback3.text = "↓ '0." + Mathf.Round((clearTimer - globalRecAvg) * 1000.0f) + " Sec";
                feedback3.color = new Color32(110, 184, 241, 255);
            }
        }
        

    }


    void Save(int map, int level, int state, string record)
    {

        Settings.AddSaveData(map, level, state, record);
    }

    public void ContinueButton()
    {
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleContinue));
    }

    public void BackButton()
    {
        Debug.Log("백 버튼 입력");
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleBack));
    }

    void MidleContinue()
    {
        level = 0;
        Settings.InstantLoad();
        if(Settings._saveInfo0.Exists(e => e.state == 0))
        {
            // 아직 클리어 못한게 있으면.
            for(int i = 1; i <= 1000; i++)
            {
                int _rnd = Random.Range(0, 200);
                if(Settings._saveInfo0[_rnd].state == 0)
                {
                    level = _rnd;
                    i = 10000;
                }
                //100번을 돌려도 안나오면, 강제로 지정
                if (i == 100)
                {
                    level=Settings._saveInfo0.Find(e => e.state == 0).number;
                }
            }
        }
        else
        {

            // 전부 클리어 했으면 다음 레벨 진행
        }

        //MainCanvas.Main.startGameScript.currentLevel = MainCanvas.Main.startGameScript.currentLevel + 1;
        MainCanvas.Main.startGameScript.currentLevel = level;
        MainCanvas.Main.Reset(MainState.StateBack.Continue);
    }

    public void MidleBack()
    {
        MainCanvas.Main.Reset(MainState.StateBack.SelectLevel);
    }

    public void SetActive(bool isActive)
    {
        DataBaseControl.Instant.db.rankCountDataSuccess = false;
        DataBaseControl.Instant.db.mainRecordDataSuccess = false;
        UIUpdate = true;
        Debug.Log("UI 업데이트 상태:" + UIUpdate);
        gameObject.SetActive(isActive);
    }
}
