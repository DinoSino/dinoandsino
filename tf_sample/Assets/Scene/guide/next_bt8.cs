﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class next_bt8 : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene("guideline4");
    }
    public void priorClick()
    {
        SceneManager.LoadScene("guideline2");
    }
    public void exitClick()
    {
        SceneManager.LoadScene("Main");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
