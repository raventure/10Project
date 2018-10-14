using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjControl : MonoBehaviour {

    static MainObjControl main;
    public CubeCreater cubeCreater;
    public GamePlaying gamePlaying;
    public LineControl lineControl;
    public CameraControl cam;

    private void Awake()
    {
      
        Application.targetFrameRate = 60;   // 프레임 고정
        Time.timeScale = 1; //실시간
        main = this;
    }
    public static MainObjControl Instant
    {
        get { return main;  }
    }
}
