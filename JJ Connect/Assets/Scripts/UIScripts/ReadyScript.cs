using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyScript : MonoBehaviour
{
    public Image ready;
    public Text readyTimer;
    public Image logo;
    public int currentMap;

    public void SetActive(bool isActive)
    {
        logo.gameObject.SetActive(false);
        gameObject.SetActive(isActive);
        if (isActive)
        {
            MainObjControl.Instant.gamePlaying.isGameStart = false;
            Debug.Log("스타트 코루틴 활성화");
            startReadyTimer();
        }
        else
        {
            
        }
        
        
    }

    public void startReadyTimer()
    {
        MainCanvas.Main.barScript.timer0 = "0.0";
        StartCoroutine("Ready");
    }

    public void textTimer(int value)
    {
        switch(value)
        {
            case 1:
                readyTimer.text = "";
                logo.gameObject.SetActive(true);
                break;
            case 0:
                StopCoroutine("Ready");
                MainCanvas.Main.barScript.TimerGo(currentMap);
                SetActive(false);
                MainObjControl.Instant.gamePlaying.isGameStart = true;
                break;
            default:
                readyTimer.text = "READY!";
                break;
        }   
    }

    IEnumerator Ready()
    {
        for (int i = 2; i >= 0; i--)
        {
            textTimer(i);
            yield return new WaitForSeconds(1.0f);

        }
    }
}
