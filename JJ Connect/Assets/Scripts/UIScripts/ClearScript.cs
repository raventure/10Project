using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearScript : MonoBehaviour {

    public RectTransform rec;
    public Text title;
    public Text timer;
    public Text feedback1;
    public Text feedback2;
    public Text feedback3;
    public Text GlobalAvg;
    public Image runBar;
    public Image clearLevel1;
    public Image clearLevel2;
    public Image clearLevel3;

    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
    }

    public void ClearView()
    {
        int map         = MainCanvas.Main.startGameScript.currentMap;
        int number      = MainCanvas.Main.startGameScript.currentLevel;
        int num         = Settings.clearInfo.FindIndex(e => e.map == map && e.level == number);
        int rankCount   = Settings.clearInfo[num].rank;
        int totalCount  = Settings.clearInfo[num].totalCount;
        float clearTimer = Settings.clearInfo[num].personReocrd;
        float globalRecAvg = Settings.clearInfo[num].globalAvg;
        int state       = Settings.clearInfo[num].state;



        float userPer = (float)rankCount / (float)totalCount;
        title.text = "Stage " + number;
        timer.text = "'" + clearTimer; // 타이머 기록 갱신
        GlobalAvg.text = "Global Average '" + globalRecAvg; //글로벌 기록
        feedback2.text = string.Format("{0:n0}", rankCount) + " / " + string.Format("{0:n0}", totalCount);

        runBar.fillAmount = 1 - userPer;
        feedback1.text = "belong to " + Mathf.Round((userPer * 100)).ToString("N2") + "% world record";

        clearLevel1.color = MainCanvas.Main.winScript.defaultColor;
        clearLevel2.color = MainCanvas.Main.winScript.defaultColor;
        clearLevel3.color = MainCanvas.Main.winScript.defaultColor;

        switch (state)
        {
            case 4:
                clearLevel1.color = MainCanvas.Main.winScript.Level1Color;
                break;
            case 3:
                clearLevel2.color = MainCanvas.Main.winScript.Level2Color;
                break;
            case 2:
                clearLevel3.color = MainCanvas.Main.winScript.Level3Color;
                break;
            default:
                break;
        }
        if (clearTimer <= globalRecAvg)
        {
            feedback3.text = "↑ '0." + Mathf.Round((globalRecAvg - clearTimer) * 1000.0f) + " Sec";
            feedback3.color = new Color32(241, 110, 125, 255);
        }
        else
        {
            feedback3.text = "↓ '0." + Mathf.Round((clearTimer - globalRecAvg) * 1000.0f) + " Sec";
            feedback3.color = new Color32(110, 184, 241, 255);
        }
        // UI 세팅 필요.
        if (num >= 0)
        {
            /*
            timer.text = Settings.clearInfo[num].personReocrd.ToString();
            goldRec.text = Settings.clearInfo[num].goldRecord.ToString();
            silverRec.text = Settings.clearInfo[num].silverRecord.ToString();
            bronzeRec.text = Settings.clearInfo[num].bronzeRecord.ToString();
            */
            /// 19.01.02 작업중
            /// 기록 보다 앞인 기록 개수 / 기록 개수 = 상위 % 표현.
            /// 코루틴으로 처리.
            /// Bar에 대한 콘트롤도 필요. 
            /// 등수도 보일 수 있도록 함.
        }
        SetActive(true);
        StartCoroutine(EffectControl.MoveAnchor(rec, GameDefine.posCenter, 0.1f, null));
    }

    public void BackButton()
    {
        Debug.Log("백 버튼 입력");
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleBack));
    }
    public void ReStart()
    {
        Debug.Log("재시작 버튼 입력");
        int level = MainCanvas.Main.startGameScript.currentLevel-1;
        switch ( MainCanvas.Main.startGameScript.currentMap)
        {
            case 4:
                Settings.saveInfo4[level].state = 0;
                break;
            case 6:
                Settings.saveInfo6[level].state = 0;
                break;
            default:
                Settings.saveInfo8[level].state = 0;
                break;
        }
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleReStart));
    }
    
    public void MidleBack()
    {
        MainCanvas.Main.Reset(MainState.StateBack.SelectLevel);
    }

    public void MidleReStart()
    {
        MainCanvas.Main.Reset(MainState.StateBack.Continue);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
