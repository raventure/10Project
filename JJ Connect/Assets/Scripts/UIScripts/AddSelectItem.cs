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
        newItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        listunit[count] = newItem.transform.GetChild(0).GetComponent<ItemUnit>();
        listunit[count].index = count + 1;
        count++;
    }
}
