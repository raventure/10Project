using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelScript : MonoBehaviour {

    public AddSelectItem addScript;
    public ItemUnit[] listUnit;
    public bool isSelected;
    public RectTransform rec;
    
    public void Preload()
    {
        addScript.CreateTable();
        listUnit = addScript.listunit;
    }
    public void Reset(bool isActive)
    {
        isSelected = false;
        
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if(isActive)
        {
            int map = MainCanvas.Main.levelShowScript.mapSelecting;
            //int map = 4;
            SetContentTable(map);
            rec.anchoredPosition = new Vector2(0, -3640.105f);
        }
    }
    void SetContentTable(int map)
    {
        Settings.Load();
        int maxLv = Settings.GetMaxLevel(map);
        //Debug.Log("맥스 레벨 체크 :" + maxLv);
        maxLv = 200;
        for (int i = 0; i < listUnit.Length; i++)
        {
            if (i <= maxLv -1)
            {
                listUnit[i].SetOpen(maxLv);
                listUnit[i].map = map;
                listUnit[i].enabled = true;
            }
            else
            {
                listUnit[i].SetLock();
                listUnit[i].enabled = false;
            }
            if(Settings._saveInfo0[i].state >= 1)
            {
                listUnit[i].SetClear(Settings._saveInfo0[i].state);
                
            }
            
        }
    }
}
