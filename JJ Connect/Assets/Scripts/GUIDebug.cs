using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDebug : MonoBehaviour {

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 100), "listArrayScriptCount :" + MainObjControl.Instant.gamePlaying.listArrayScript.Count);
        GUI.Label(new Rect(10, 30, 150, 100), "Mouse : : 작업해야함.");
    }
}
