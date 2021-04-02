using System.Collections;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;


public class EatGuide : MonoBehaviour
{
    public GameObject FPanelObj;
    public GameObject SPanelObj;
    public GameObject TPanelObj;
    // Start is called before the first frame update
    public void FirstClickBtn()
    {
        FPanelObj.SetActive(false);
        SPanelObj.SetActive(true);//패널이 뜨게 된다.
    }
    public void SecondClickBtn()
    {
        SPanelObj.SetActive(false);
        TPanelObj.SetActive(true);
        SceneManager.LoadScene("PoseNet1");//패널이 뜨게 된다.
    }
}
