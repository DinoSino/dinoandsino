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
        /*   Debug.Log("캡쳐");
           Panel.SetActive(false);
           string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
           string fileName = "Screenshot" + timeStamp + ".png";
           NativeCamera.

           string pathToSave = "/내장 메모리/DCIM/Camera";
           ScreenCapture.CaptureScreenshot(pathToSave + fileName);
           yield return new WaitForEndOfFrame();
           Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity);

            

           Debug.Log(pathToSave);
        */
        Panel.SetActive(false);
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
         
        // Save the screenshot to Gallery/Photos
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "GalleryTest", "My img {0}.png"));

        // To avoid memory leaks
        Destroy(ss);
        Panel.SetActive(true);
    }



}

