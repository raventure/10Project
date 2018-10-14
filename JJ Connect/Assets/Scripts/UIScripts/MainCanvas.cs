using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour {
    static MainCanvas           mainCanvas;
    public FaderControl         fadeScript;
    public StartGameScript      startGameScript;
    public LevelShowScript      levelShowScript;
    public SelectLevelScript    selectLevelScript;
    public InGameScript         inGameScript;
    public WinScript            winScript;
    public LostScript           lostScript;
    public Bar                  barScript;

    public static MainCanvas Main
    {
        get { return mainCanvas; }
    }
    private void Awake()
    {
        mainCanvas = this;
    }
    private void Start()
    {
        Preload();
    }
    void Preload()
    {
        selectLevelScript.Preload();
    }
    public void Reset(MainState.StateBack stateBack)
    {
        MainObjControl.Instant.gamePlaying.Reset();
        MainObjControl.Instant.lineControl.Reset();

        //캔버스 처리
        switch(stateBack)
        {
            case MainState.StateBack.Home:
                startGameScript.Reset(true);
                inGameScript.Reset(false);
                //백그라운드 스크립트
                selectLevelScript.Reset(false);
                levelShowScript.Reset(false);
                
                break;
            case MainState.StateBack.Continue:
                startGameScript.Reset(false);
                startGameScript.Midle();
                inGameScript.Reset(true);
                //백그라운드 스크립트?
                selectLevelScript.Reset(false);
                levelShowScript.Reset(false);
                MainState.SetState(MainState.State.Ingame);
                
                break;
            case MainState.StateBack.SelectLevel:
                inGameScript.Reset(true);
                break;
            case MainState.StateBack.SelectMap:
                break;
        }

        winScript.Reset();
        lostScript.Reset();
        fadeScript.Reset();
    }
    public void Win()
    {
        barScript.StopTimer();
        winScript.GameWin();
    }
    public void Lost()
    {
        barScript.StopTimer();
        lostScript.GameOver();
    }

}
