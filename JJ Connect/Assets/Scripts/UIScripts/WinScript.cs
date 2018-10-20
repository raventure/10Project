using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour {
    public RectTransform rec;
    public Text timer;
    public Text goldTime;
    public Text silverTime;
    public Text bronzeTime;
    public Text feedback;
    public Image[] medalImg;

    int numberStar;
    int level;
    bool checkRecord;
    float avg;
    float std;

    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
        
    }

    public void GameWin()
    {
        MainState.SetState(MainState.State.GameOver);
        int currentLevel = MainCanvas.Main.startGameScript.currentLevel;
        int currentMap = MainCanvas.Main.startGameScript.currentMap;

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
            float goldRecord = Mathf.Round((avg - std) * 1000) / 1000;
            float silverRecord = Mathf.Round((avg) * 1000) / 1000;
            float bronzeRecord = Mathf.Round((avg + std) * 1000) / 1000;

            goldTime.text = "'" + goldRecord;
            silverTime.text = "'" + silverRecord;
            bronzeTime.text = "'" + bronzeRecord;

        }
        timer.text = "'"+MainCanvas.Main.barScript.timer0;
        SetActive(true);
        //캔버스 스탑 타이머
        //사운드 출력
        StartCoroutine(EffectControl.MoveAnchor(rec, GameDefine.posCenter, 0.5f, null));

        if(currentLevel >= Settings.GetMaxLevel(currentMap) && Settings.GetMaxLevel(currentMap) < 200)
        {
            Settings.SetMaxLevel(currentMap, currentLevel + 1);
        }

        int state;
        
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

        Save(currentMap, currentLevel, state, MainCanvas.Main.barScript.timer0);
    }


    void Save(int map, int level, int state, string record)
    {

        Settings.AddSaveData(map, level, state, record);

    }

    public void ContinueButton()
    {
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleContinue));
    }

    void MidleContinue()
    {
        level = 0;
        Settings.InstantLoad();
        if(Settings._saveInfo0.Exists(e => e.state == 0))
        {
            // 아직 클리어 못한게 있으면.
            for(int i = 1; i <= 100; i++)
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
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
