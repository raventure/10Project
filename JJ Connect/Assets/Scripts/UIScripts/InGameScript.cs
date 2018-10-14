using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScript : MonoBehaviour {

    public void Reset(bool isActive)
    {
        SetActive(isActive);
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
