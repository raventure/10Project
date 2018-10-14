using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseControl : MonoBehaviour
{
    static DataBaseControl main;

    public ConnectScript conn;
    public DBScript db;

    private void Awake()
    {
        main = this;
    }

    public static DataBaseControl Instant
    {
        get { return main; }
    }


}
