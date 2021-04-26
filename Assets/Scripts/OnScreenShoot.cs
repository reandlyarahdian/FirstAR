using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OnScreenShoot : MonoBehaviour
{
    public OnScreenShoot instance;

    private bool afterScreenShot;

    private Camera cam;

    private string path;

    void Awake()
    {
        instance = this;

        cam = Camera.main;
    }

    private void Screenshot()
    {
        StartCoroutine(enumerator());
    }

    public void TakeScreenshot()
    {
        instance.Screenshot();
    }

    private IEnumerator enumerator()
    {
        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(System.DateTime.Now.ToString("yyyy - MM - dd - HHmm") + ".png");

        path = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy - MM - dd - HHmm") + ".png");

        Debug.Log(path);

        new NativeShare().AddFile(path)
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();

        afterScreenShot = true;
        

    }
}
