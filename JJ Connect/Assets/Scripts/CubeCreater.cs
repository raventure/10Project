using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreater : MonoBehaviour {
    public GameObject pkcPrefabs;
    public float screenDefault;
    public int numberCol, numberRow;
    public Shader pikachuShader;

    public TableUnit[,] listScript;
    public Vector3[,] listPost;
    public int[] pool;
    public float width, newBarScale;

    public GameObject lv, score;
    List<int> listInPkc;
    float startX, startZ;
    public Vector3[] savePos;
    int stt;
    int[] newMap;
    public List<TableUnit> listDeactive;


    public void CreateTableUnit(int newRow, int newCol, int indexData)
    {
        Debug.Log("[Lv."+indexData+"]"+"["+newRow+" x "+newCol+"]");
        
        if(listScript != null)
        {
            for(int i = 0; i < numberRow; i++)
            {
                for(int j = 0; j < numberCol; j++)
                {
                    listDeactive.Add(listScript[i, j]);
                    listScript[i, j].gameObject.SetActive(false);
                }
            }
        }
        numberRow = newRow;
        numberCol = newCol;

        listScript = new TableUnit[numberRow, numberCol];
        listPost = new Vector3[numberRow, numberCol];

        startX = -numberRow / 2.0f + 0.5f;
        startZ = -numberCol / 2.0f + 0.5f;

        for(int i = 0; i< numberRow; i++)
        {
            for(int j = 0; j < numberCol; j++)
            {
                Vector3 pos = new Vector3(startX, -0.1f, startZ);
                startZ += 1;
                InstantNewPKC(pos, i, j);
            }
            startZ = -numberCol / 2.0f + 0.5f;
            startX += 1;
        }
        MainObjControl.Instant.gamePlaying.listScript = listScript;
        ApplyData(indexData, newRow);
        MainObjControl.Instant.cam.ChangeView(newRow);
        MainCanvas.Main.inGameScript.SetLevel(indexData);


    }

    void ApplyData(int indexData, int map)
    {
        newMap = Map.getObjectMap(numberCol, indexData).listmap;
        Debug.Log(newMap[0]);
        int count = 0;
        int brick = 0;
        for(int i = 0; i < numberRow; i++)
        {
            for (int j = 0; j < numberCol; j++)
            {
                int index = newMap[count];
                //Debug.Log(index);
                switch(index)
                {
                    case 0:
                        listScript[i, j].SetNumber(0); // normal box
                        listScript[i, j].ChangeNormalBox();
                        listScript[i, j].ChangeToNormalColor();
                        break;
                    case -1:
                        listScript[i, j].ChangeBlockBox(); // Brick
                        listScript[i, j].SetNumber(0);
                        brick++;
                        break;
                    default:
                        listScript[i, j].SetNumber(newMap[count]);
                        listScript[i, j].ChangeNormalBox();
                        listScript[i, j].ChangeToNormalColor();
                        break;
                }
                count++;
            }
        }
        int totalBox = numberRow * numberCol - brick;
        MainObjControl.Instant.gamePlaying.SetValue(totalBox, map);
        MainCanvas.Main.inGameScript.SetLength(0, totalBox);
        //캔버스 처리
    }
    void InstantNewPKC(Vector3 pos, int row, int col)
    {
        GameObject pkc = GetBox(pos, row, col);
        listScript[row, col] = pkc.GetComponent<TableUnit>();
        listScript[row, col].index = stt;
        stt++;
        listScript[row, col].SetValue(row, col);
        listPost[row, col] = pkc.transform.localPosition;
    }
    GameObject GetBox(Vector3 pos, int row, int col)
    {
        GameObject newObj;
        if(listDeactive.Count == 0)
        {
            newObj = Instantiate(pkcPrefabs, pos, Quaternion.identity) as GameObject;
            newObj.transform.localScale = new Vector3(GameDefine.pikachuNormalSacle, GameDefine.pikachuNormalSacle, GameDefine.pikachuYScale);
            newObj.transform.eulerAngles = new Vector3(-90, 0, 0);
            newObj.transform.SetParent(transform);
            newObj.transform.name = "brick("+row+","+col+")";
        }
        else
        {
            newObj = listDeactive[listDeactive.Count - 1].gameObject;
            newObj.SetActive(true);
            listDeactive[listDeactive.Count - 1].Reset();
            listDeactive[listDeactive.Count - 1].ChangeNormalBox();
            listDeactive.RemoveAt(listDeactive.Count - 1);
            newObj.transform.position = pos;
            
        }
        return newObj;
    }
}
