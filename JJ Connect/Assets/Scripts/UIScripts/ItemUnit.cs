using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUnit : MonoBehaviour, IPointerClickHandler {
    public int index;
    public int map;

    public Text numberText;
    public Text clearText;
    public GameObject lockObj;
    public GameObject openObj;
    public GameObject clearObj;

    public Vector3 scaleZoom = new Vector3(1.5f, 1.5f, 1.5f);
    float timeZoomOut = 0.1f;
    float timeZoomIn = 0.2f;
    bool running = false;

    void OnEnable()
    {
        running = false;
        transform.localScale = Vector3.one;
    }
    public void SetLock()
    {
        lockObj.SetActive(true);
        openObj.SetActive(false);
    }
    public void SetOpen(int maxLv)
    {
        lockObj.SetActive(false);
        openObj.SetActive(true);
        numberText.text = index.ToString();
    }
    public void SetClear(int state)
    {
        lockObj.SetActive(false);
        openObj.SetActive(false);
        clearObj.SetActive(true);
        
        Image img = clearObj.GetComponent<Image>();
        switch (state)
        {
            case 2:
                clearText.text = index.ToString()+"\n"+"Bronze";
                img.color = new Color32(205, 127, 50, 255);
                break;
            case 3:
                clearText.text = index.ToString() + "\n" + "Silver";
                img.color = new Color32(211, 211, 211, 255);
                break;
            case 4:
                clearText.text = index.ToString() + "\n" + "Gold";
                img.color = new Color32(255, 215, 0, 255);
                break;
            default:
                clearText.text = index.ToString() + "\n" + "-";
                break;
        }
    }

    // 이벤트 먹이고 싶을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!running && !MainCanvas.Main.selectLevelScript.isSelected)
        {
            Debug.Log("버튼 클릭");
            StartCoroutine(ButtonClick()) ;
        }
    }
    public IEnumerator ButtonClick()
    {
        
        MainCanvas.Main.selectLevelScript.isSelected = true;
        running = true;
        float currentTime = 0;
        while(currentTime < timeZoomOut)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, scaleZoom, currentTime / timeZoomOut);
            currentTime += Time.unscaledDeltaTime; // 타임 스케일을 무시
            yield return null;
        }
        currentTime = 0;
        //처리
        /*
        switch(map)
        {
            case 4:
                Settings._saveInfo = Settings.saveInfo4;
                break;
            case 6:
                Settings._saveInfo = Settings.saveInfo6;
                break;
            default:
                Settings._saveInfo = Settings.saveInfo8;
                break;
        }

        */
        if (Settings._saveInfo0[index - 1].state == 0)
        {
            MainCanvas.Main.startGameScript.StartPlayerLevel(map, index);
        }
        else
        {
            Debug.Log("클리어 된 맵:"+ index);
            MainCanvas.Main.startGameScript.currentMap = map;
            MainCanvas.Main.startGameScript.currentLevel = index;
            MainCanvas.Main.clearScript.ClearView();
            //MainCanvas.Main.winScript.GameWin();
        }
        //MainCanvas.Main.startGameScript.StartPlayerLevel(map, index);

        while (currentTime < timeZoomIn)
        {
            transform.localScale = Vector3.Lerp(scaleZoom, Vector3.one, currentTime / timeZoomIn);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        running = false;
    }
}
