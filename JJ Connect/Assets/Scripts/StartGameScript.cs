using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour {

    public int currentLevel;
    public int currentMap;
	// Use this for initialization
	void Start () {
        //Settings.Clear();
        //유저 정보 체크
        Settings.CheckNRU();
        //Settings.Load();
        DataBaseControl.Instant.db.GetRecordAvgList();
    }
    public void Reset(bool isActive)
    {
        //사운드
        SetActive(isActive);
        
    }
    public void  SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if(isActive)
        {
            Settings.LoadSaveInfo(4);
            Settings.LoadSaveInfo(6);
            Settings.LoadSaveInfo(8);
        }
    }
    public void StartPlayerLevel(int map, int level)
    {
        currentLevel = level;
        currentMap = map;
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(Midle));
    }
    public void Midle()
    {
        Debug.Log("Midle Call ["+currentMap+"]");
        //currentLevel = 1;
        //currentMap = 4;
        MainState.SetState(MainState.State.Ingame);
        MainObjControl.Instant.cubeCreater.CreateTableUnit(currentMap, currentMap, currentLevel);
        
        MainCanvas.Main.selectLevelScript.SetActive(false);
        MainCanvas.Main.barScript.TimerGo(currentMap);
    }

}
