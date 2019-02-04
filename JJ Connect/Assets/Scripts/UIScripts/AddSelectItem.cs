using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AddSelectItem : MonoBehaviour {

    public GameObject item;
    public int row, col;
    public float height;
    public GridLayoutGroup grid;
    public RectTransform rec;
    public ItemUnit[] listunit;

    int count = 0;

    public void InitTable()
    {
        Debug.Log("테이블 초기화");
        Transform[] childList = GetComponentsInChildren<Transform>(true);
        if(childList != null)
        {
            for(int i = 0; i< childList.Length; i++)
            {
                if(childList[i] != transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }
    }

    public void CreateTable()
    {
        int numberItem = row * col;
        listunit = new ItemUnit[numberItem];
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                AddItem();
            }
        }
    }

    void AddItem()
    {
        GameObject newItem = Instantiate(item);
        newItem.transform.SetParent(transform);
        //newItem.transform.localScale = Vector3.one;
        Debug.Log("아이템 스케일 사이즈 조정");
        newItem.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        listunit[count] = newItem.transform.GetChild(0).GetComponent<ItemUnit>();
        listunit[count].index = count + 1;
        count++;
    }
}
