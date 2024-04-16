using System;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            string path = "ScreenShot" + DateTime.Now.ToString("yyyy,mm,dddd,hh,mm.ss") + ".png";
            ScreenCapture.CaptureScreenshot(path, 2);
        }
    }
}
