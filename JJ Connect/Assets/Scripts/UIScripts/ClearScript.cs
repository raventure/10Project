using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearScript : MonoBehaviour {

    public RectTransform rec;

    public Text timer;
    public Text goldRec;
    public Text silverRec;
    public Text bronzeRec;
    public Text title;

    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
    }

    public void ClearView()
    {
        int map = MainCanvas.Main.startGameScript.currentMap;
        int number = MainCanvas.Main.startGameScript.currentLevel;
        int num = Settings.clearInfo.FindIndex(e => e.map == map && e.level == number);
        // UI 세팅 필요.
        if(num >= 0)
        {
            timer.text = Settings.clearInfo[num].personReocrd.ToString();
            goldRec.text = Settings.clearInfo[num].goldRecord.ToString();
            silverRec.text = Settings.clearInfo[num].silverRecord.ToString();
            bronzeRec.text = Settings.clearInfo[num].bronzeRecord.ToString();
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
