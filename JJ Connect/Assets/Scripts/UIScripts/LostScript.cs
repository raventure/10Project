using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostScript : MonoBehaviour {
    public RectTransform rec;
    public void Reset()
    {
        rec.anchoredPosition = GameDefine.posRight;
        SetActive(false);
    }
    public void GameOver()
    {
        MainState.SetState(MainState.State.GameOver);
        SetActive(true);
        //오디오
        StartCoroutine(EffectControl.MoveAnchor(rec, GameDefine.posCenter, 0.5f, null));
        MainObjControl.Instant.gamePlaying.isGameOver = true;
    }
    public void TryAgainButton()
    {
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleTryAgain));
    }
    public void HomeButton()
    {
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MidleHome));
    }
    void MidleTryAgain()
    {
        MainCanvas.Main.Reset(MainState.StateBack.Continue);
    }
    void MidleHome()
    {
        MainCanvas.Main.Reset(MainState.StateBack.SelectLevel);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

}
