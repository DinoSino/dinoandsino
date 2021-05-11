using System.Collections;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;


public class BrushGuide : MonoBehaviour
{
    public GameObject FirstPanelObj;
    public GameObject SecondPanelObj;
    public GameObject ThirdPanelObj;
    // Start is called before the first frame update
    public void FirstClickBtn()
    {
        FirstPanelObj.SetActive(false);
        SecondPanelObj.SetActive(true);//패널이 뜨게 된다.

    }
    public void SecondClickBtn()
    {
        SecondPanelObj.SetActive(false);
        ThirdPanelObj.SetActive(true);
        SceneManager.LoadScene("PoseNet");//패널이 뜨게 된다.
    }
}
