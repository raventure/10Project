using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Camera cam;
    public Transform whiteBG;

    public Vector3 p4, p6, p8;
    public Vector3 s4, s6, s8;

    private void Awake()
    {
        float h = transform.position.y;
        float z = h * Mathf.Tan( (90 - transform.eulerAngles.x)  * Mathf.Deg2Rad);
        transform.position = new Vector3(transform.position.x, transform.position.y, -z);

    }
    public void ChangeView(int map)
    {
        switch(map)
        {
            case 4:
                SetCam(5.5f);
                whiteBG.position = p4;
                whiteBG.localScale = s4;
                break;
            case 6:
                SetCam(7f);
                whiteBG.position = p6;
                whiteBG.localScale = s6;
                break;
            case 8:
                SetCam(8.5f);
                whiteBG.position = p8;
                whiteBG.localScale = s8;
                break;
        }
    }

    void SetCam(float width)
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float cameraHeight = width / aspectRatio;
        cam.orthographicSize = cameraHeight / 2;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
