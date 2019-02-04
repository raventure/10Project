using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShowScript : MonoBehaviour {

    public int mapSelecting;
    public void Reset(bool isActive)
    {
        SetActive(isActive);
    }
    public void OpenMap4()
    {
        Debug.Log("버튼4");
        Open(4);
    }
    public void OpenMap6()
    {
        Open(6);
    }
    public void OpenMap8()
    {
        Open(8);
    }
    void Open(int map)
    {
        mapSelecting = map;
        //Settings.InstantLoad();
        MainCanvas.Main.fadeScript.Fade(new FaderControl.Callback0(MiddleSelect));
        
    }
    void MiddleSelect()
    {
        MainState.SetState(MainState.State.SelectLevel);
        MainCanvas.Main.selectLevelScript.SetActive(true);
        SetActive(false);
    }
    public void SetActive(bool isActive)
    {
        if(isActive)
        {
            Debug.Log("난이도 설정 화면");
            //MainCanvas.Main.selectLevelScript.Preload();
        }
        gameObject.SetActive(isActive);
    }

    private void Start()
    {
        //Open(4);
    }

}
