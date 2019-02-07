using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaying : MonoBehaviour {

    public TableUnit[,] listScript;
    public List<TableUnit> listArrayScript;
    public bool isGameOver;
    public bool isGameStart;
    public int totalBox;
    int max;
    public int currentNumber;

    public void SetValue(int newTotal, int maxNumber)
    {
        totalBox = newTotal;
        switch (maxNumber)
        {
            case 4:
                max = 4 + 1;
                break;
            case 6:
                max = 8 + 1;
                break;
            default:
                max = 10 + 1;
                break;
        }
    }

    public void Reset()
    {
        for(int i = 0; i <listArrayScript.Count; i++)
        {
            listArrayScript[i].Reset();
        }
        listArrayScript = new List<TableUnit>();
        isGameOver = false;
        currentNumber = 0;
        MainCanvas.Main.inGameScript.SetNumberNext(1, max, isGameOver);

        if (totalBox != 0)
        {
            MainCanvas.Main.inGameScript.SetLength(0, totalBox);
        }
    }



    public void TableUnitPress(Vec2 vec, bool isFree, int index)
    {
        if (isGameStart)
        {
            if (!isGameOver)
            {
                if (isFree)
                {
                    if (CheckNumberOnly(index))
                    {
                        //Debug.Log("Free");
                        FreePress(vec, index);
                    }
                }
                else
                {
                    //Debug.Log("Select");
                    SelectPress(vec, index);
                }
            }
        }

    }
    bool CheckNumberOnly(int index)
    {
        if(index == currentNumber + 1 || index == 0)
        {
            return true;
        }
        return false;
    }
    void Nextnumber(int index)
    {
        if (index == currentNumber + 1)
        {
            currentNumber++;
            MainCanvas.Main.inGameScript.SetNumberNext(currentNumber + 1, max, isGameOver);
        }
    }
    void FreePress(Vec2 vec, int index)
    {
        if(listArrayScript.Count == 0)
        {
            if (listScript[vec.R, vec.C].tableNumber == 1)
            {
                Vector3 pos = listScript[vec.R, vec.C].transform.position;
                MainObjControl.Instant.lineControl.AddPoint(new Vector3(pos.x, 0.8f, pos.z));
                listScript[vec.R, vec.C].ChangeToSelectColor();
                listArrayScript.Add(listScript[vec.R, vec.C]);
                Debug.Log("("+ listArrayScript.Count+")리스트 추가 : " + vec.R + "/" + vec.C);

                CheckGameover();
                Nextnumber(index);
                MainAudio.Main.PlaySound(TypeAudio.SoundRenew);
            }
        }
        else if(CheckRoad(vec, listArrayScript[listArrayScript.Count -1].GetValue()))
        {
            Vector3 pos = listScript[vec.R, vec.C].transform.position;
            MainObjControl.Instant.lineControl.AddPoint(new Vector3(pos.x, 0.8f, pos.z));
            listScript[vec.R, vec.C].ChangeToSelectColor();
            listArrayScript.Add(listScript[vec.R, vec.C]);
            //Debug.Log("(" + listArrayScript.Count + ")리스트 추가 : " + vec.R + "/" + vec.C);
            //Debug.Log(listArrayScript.Count + "/" + totalBox);
            CheckGameover();
            Nextnumber(index);
            MainAudio.Main.PlaySound(TypeAudio.SoundRenew);
        }
        
    }
    void SelectPress(Vec2 vec, int index)
    {
        if(listArrayScript.Count >= 2)
        {
            Vec2 oldVec = listArrayScript[listArrayScript.Count - 2].GetValue();
            if (vec.R == oldVec.R && vec.C == oldVec.C)
            {
                TurnBack(listArrayScript[listArrayScript.Count - 1].tableNumber);
                MainObjControl.Instant.lineControl.RemovePoint();
                listArrayScript[listArrayScript.Count - 1].ChangeToNormalColor();
                listArrayScript.RemoveAt(listArrayScript.Count - 1);
                MainCanvas.Main.inGameScript.SetLength(listArrayScript.Count, totalBox);

            }
        }
    }
    void TurnBack(int index)
    {
        if(currentNumber == index)
        {
            currentNumber = Mathf.Max(0, currentNumber - 1);
            MainCanvas.Main.inGameScript.SetNumberNext(currentNumber + 1, max, isGameOver);
        }
    }
    bool CheckRoad(Vec2 v1, Vec2 v2)
    {
        if ((v1.R == v2.R && Mathf.Abs(v1.C - v2.C) == 1) || (v1.C == v2.C && Mathf.Abs(v1.R - v2.R) == 1))
        {
            //Debug.Log("TRUE : v1(" + v1.R + "," + v1.C + ")");
            //Debug.Log("TRUE : v2(" + v2.R + "," + v2.C + ")");
            return true;
        }
        //Debug.Log("체크로드 결과 : false");
        //Debug.Log("FALSE : v1(" + v1.R + "," + v1.C + ")");
        //Debug.Log("FALSE : v2(" + v2.R + "," + v2.C + ")");
        return false;
    }

    void CheckGameover()
    {
        int current = listArrayScript.Count;
        MainCanvas.Main.inGameScript.SetLength(current, totalBox);
        if (!isGameOver && current == totalBox)
        {
            Debug.Log("게임오버");
            isGameOver = true;
            MainCanvas.Main.inGameScript.SetNumberNextDone();
            MainCanvas.Main.Win();
        }
        
        
    }
}
