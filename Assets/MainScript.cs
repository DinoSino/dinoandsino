using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public Text nickText;
    // Start is called before the first frame update
    void Start()
    {   
        nickText.text = PlayerPrefs.GetString("Name");
    }

    // Update is called once per frame
    public void BrushBtn()
    {
        SceneManager.LoadScene("BrushGuide");
    }
    public void EatBtn()
    {
        SceneManager.LoadScene("EatGuide");
    }
}
