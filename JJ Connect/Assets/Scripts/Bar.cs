using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    public Image image;
    public Text second;

    public string timer0;

    float duration;

    IEnumerator timeRun;
    bool runing;

    public void TimerGo(int map)
    {
        float totalDuration;
        switch (map)
        {
            case 4:
                duration = 20;
                break;
            case 6:
                duration = 40;
                break;
            default:
                duration = 60;
                break;
        }
        StopTimer();
        timeRun = StartTimer();
        StartCoroutine(timeRun);

    }

    public void StopTimer()
    {
        if(runing)
        {
            StopCoroutine(timeRun);
            runing = false;
        }
    }

    IEnumerator StartTimer()
    {
        runing = true;
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            image.fillAmount = 1 - timer / duration;
            timer0 = string.Format("{0:.###}", timer).ToString();
            second.text = timer0;
            //fraction.text = string.Format("{0:.###}", timer).ToString();
            yield return null;
        }
        MainCanvas.Main.Lost();
        runing = false;
    }

}
