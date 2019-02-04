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
    public ClearScript          clearScript;
    public TutorialScript       tutorialScript;


    public static MainCanvas Main
    {
        get { return mainCanvas; }
    }
    private void Awake()
    {
        //Settings.Clear();
        mainCanvas = this;
    }
    private void Start()
    {
        Preload();
    }
    void Preload()
    {
        MainState.allowButton = true;
        selectLevelScript.Preload();
    }
    public void Reset(MainState.StateBack stateBack)
    {
        MainObjControl.Instant.gamePlaying.Reset();
        MainObjControl.Instant.lineControl.Reset();

        //
        barScript.StopTimer();
        tutorialScript.Reset();
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
                Debug.Log("[SelectLevel]상태");
                selectLevelScript.Reset(true);
                selectLevelScript.SetActive(true);
                startGameScript.Reset(false);
                //inGameScript.Reset(false);
                levelShowScript.Reset(false);
                break;
            case MainState.StateBack.SelectMap:
                break;
        }

        
        winScript.Reset();
        lostScript.Reset();
        clearScript.Reset();
        //fadeScript.Reset();
    }
    public void Win()
    {
        Debug.Log("게임 완료");
        barScript.StopTimer();
        winScript.GameWin();
    }
    public void Lost()
    {
        Debug.Log("게임 실패");
        barScript.StopTimer();
        lostScript.GameOver();
    }
    public void Clear()
    {
        Debug.Log("게임 클리어");
    }

}
