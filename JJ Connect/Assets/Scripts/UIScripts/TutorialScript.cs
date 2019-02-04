using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public void Reset()
    {
        
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void ShowToturial()
    {
        if(Settings.ShowTutorial())
        {
            Time.timeScale = 0;
            SetActive(true);
        }
    }

    public void ShowTutorialNow()
    {
        Time.timeScale = 0;
        SetActive(true);
    }

    public void GotItClick()
    {
        Time.timeScale = 1;
        SetActive(false);
    }

}
