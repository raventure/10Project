using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TableUnit : MonoBehaviour {

    public int index;
    public int row, col;
    public Color selectColor, normalColor;
    public Material faceMat;
    public TextMesh text;
    public int tableNumber;
    public GameObject blokObj;
    public GameObject faceObj;
    public MeshRenderer faceRender;
    public MeshRenderer numberRender;
    public Color[] listColor;

    public enum State
    {
        Free,
        Select,
        Block,
    }

    public State state = State.Free;

    private void Awake()
    {
        faceMat = faceRender.material;
        numberRender.sortingLayerName = "Number";
    }
    public void Reset()
    {
        ChangeToNormalColor();
    }
    public bool IsFree()
    {
        if(state == State.Free)
        {
            return true;
        }
        return false;
    }
    public bool IsSelect()
    {
        if(state == State.Select)
        {
            return true;
        }
        return false;
    }
    public void SetFree()
    {
        state = State.Free;
    }
    public void SetSelect()
    {
        state = State.Select;
    }

    public void SetValue(int r, int c)
    {
        row = r;
        col = c;
    }
    public Vec2 GetValue()
    {
        return new Vec2(row, col);
    }
    private void OnMouseOver()
    {
        //Debug.Log(transform.name + ") ORIGIN V1 (" + row + "," + col + ")");
        //Debug.Log(transform.name);
        //Debug.Log("ORIGIN V1 (" + row + "," + col + ")");
        if (MainState.state == MainState.State.Ingame)
        {
            MainObjControl.Instant.gamePlaying.TableUnitPress(new Vec2(row, col), IsFree(), tableNumber);
            //Debug.Log("ORIGIN V1 (" + row + ","+ col+")");
        }
    }
    public void ChangeToSelectColor()
    {
        state = State.Select;
        faceMat.SetColor("_Color", selectColor);
    }
    public void ChangeToNormalColor()
    {
        state = State.Free;
        faceMat.SetColor("_Color", normalColor);
    }
    public void ChangeBlockBox()
    {
        state = State.Block;
        faceObj.SetActive(false);
        blokObj.SetActive(true);
    }
    public void ChangeNormalBox()
    {
        faceObj.SetActive(true);
        blokObj.SetActive(false);
    }
    public void SetNumber(int number)
    {
        tableNumber = number;
        if(number == 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
            text.text = number.ToString();
            text.color = listColor[number - 1];

        }
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }

    bool isMoveUp;
    bool isMoveDown;

    IEnumerator coroutineMoveUp;
    IEnumerator coroutineMoveDown;

    public float duration;
    public float up, down;

    public void MoveUp()
    {
        if (isMoveDown)
        {
            isMoveDown = false;
            StopCoroutine(coroutineMoveDown);
        }

        if(!isMoveUp)
        {
            coroutineMoveUp = StartMoveUp();
            StartCoroutine(coroutineMoveUp);
        }
    }

    public void MoveDown()
    {
        if (isMoveUp)
        {
            isMoveUp = false;
            StopCoroutine(coroutineMoveUp);
        }

        if (!isMoveUp)
        {
            coroutineMoveDown = StartMoveDown();
            StartCoroutine(coroutineMoveDown);
        }
    }
    IEnumerator StartMoveUp()
    {
        isMoveUp = true;
        float elapsed = 0;
        Vector3 from = transform.position;
        while (elapsed <= duration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(from.y, up, elapsed / duration), transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, up, transform.position.z);
        isMoveUp = false;
    }
    IEnumerator StartMoveDown()
    {
        isMoveDown = true;
        float elapsed = 0;
        Vector3 from = transform.position;
        while(elapsed <= duration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(from.y, down, elapsed / duration), transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, down, transform.position.z);
        isMoveDown = false;
    }
    public void SetPosDown()
    {
        transform.position = new Vector3(transform.position.x, down, transform.position.z);
    }
    public void SetPosUp()
    {
        transform.position = new Vector3(transform.position.x, up, transform.position.z);
    }

}
