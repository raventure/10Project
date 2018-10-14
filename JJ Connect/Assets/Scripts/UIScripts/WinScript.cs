using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour {
    public RectTransform rec;
    public Text timer;
    int numberStar;
    int level;

    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
        
    }

    public void GameWin()
    {
        MainState.SetState(MainState.State.GameOver);
        timer.text = "'"+MainCanvas.Main.barScript.timer0;
        SetActive(true);
        //캔버스 스탑 타이머
        //사운드 출력
        StartCoroutine(EffectControl.MoveAnchor(rec, GameDefine.posCenter, 0.5f, null));

        int currentLevel = MainCanvas.Main.startGameScript.currentLevel;
        int currentMap = MainCanvas.Main.startGameScript.currentMap;
        Debug.Log(currentLevel + "/" + currentMap);
        if(currentLevel >= Settings.GetMaxLevel(currentMap) && Settings.GetMaxLevel(currentMap) < 200)
        {
            Settings.SetMaxLevel(currentMap, currentLevel + 1);
        }

        int state;
        float rankAvg = 4.0f;
        float avg = 5.0f;
        if(float.Parse(MainCanvas.Main.barScript.timer0) <= rankAvg)
        {
            state = 3;
        }else if(float.Parse(MainCanvas.Main.barScript.timer0) <= avg)
        {
            state = 2;
        }
        else
        {
            state = 1;
        }

        Save(currentMap, currentLevel, state, MainCanvas.Main.barScript.timer0);
    }


    void Save(int map, int level, int state, string record)
    {
        Debug.Log("SAVE");
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
