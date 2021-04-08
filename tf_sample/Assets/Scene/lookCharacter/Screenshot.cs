using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{

    [SerializeField]
    GameObject blink;

    public GameObject Panel;

    public void TakeShot()
    {
       
        StartCoroutine(Capture());
        
    }

    IEnumerator Capture()
    {
        Debug.Log("캡쳐");
        Panel.SetActive(false);
        string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity);
        
         Panel.SetActive(true);
       
        

    }



}

