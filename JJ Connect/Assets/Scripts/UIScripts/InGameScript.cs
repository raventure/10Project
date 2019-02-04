using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameScript : MonoBehaviour {

    public RectTransform rec;
    public Text levelText;
    public Text LengthText;
    public Text numberNextText;

    public OnChangeScale lengthTextScript;
    public OnChangeScale numberNextScript;
    

    public void Reset(bool isActive)
    {
        SetActive(isActive);
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetLevel(int level)
    {
        levelText.text = "STAGE  " + level;
    }

    public void SetLength( int t1, int t2)
    {
        LengthText.text = t1 + " / " + t2;
    }

    public void SetNumberNext( int number, int max, bool isWin)
    {
        if(number == 1)
        {
            numberNextText.text = "Start at number " + number;
        }
        else if(number == max)
        {
            if(isWin)
            {
                //처리
                SetNumberNextDone();
            }
            else
            {
                numberNextText.text = "Must go through all the boxes";
            }
        }
        else
        {
            numberNextText.text = "Go to number " + number;
        }
    }

    public void SetNumberNextDone()
    {
        numberNextText.text = "Done !";
    }

    public void RewNew()
    {
        Debug.Log("초기화 버튼 클릭");
        if (MainState.allowButton)
        {
            MainObjControl.Instant.lineControl.Reset();
            MainObjControl.Instant.gamePlaying.Reset();
        }
    }
    public void Back()
    {
        Debug.Log("패배 버튼 클릭");
        if(MainState.allowButton)
        {
            MainCanvas.Main.Lost();
            //MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleBack));
        }
    }
    void MidleBack()
    {
        MainState.SetState(MainState.State.SelectLevel);
        MainCanvas.Main.Reset(MainState.StateBack.SelectLevel);
    }
    public void ShowTuto()
    {
        MainCanvas.Main.tutorialScript.ShowTutorialNow();
    }
}
