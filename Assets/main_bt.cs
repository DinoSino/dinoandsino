using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main_bt : MonoBehaviour
{
    public Image image;
    public CanvasGroup[] CG_Views;

    
    public void guideClick()
    {
        SceneManager.LoadScene("guideline1");
    }
    public void characterClick()
    {
        SceneManager.LoadScene("MyCharacter");
    }
    public void settingClick()
    {
        SceneManager.LoadScene("SettingScene");
    }
    public void setting()
    {
        ViewCanvas(1);
    }

    public void mainClick()
    {
        ViewCanvas(0);
    }
    void ViewCanvas(int _index)
    {
        for (int i = 0; i < CG_Views.Length; i++)
        {
            if (_index == i)
            {
                CG_Views[i].alpha = 1;
                CG_Views[i].blocksRaycasts = true;
            }
            else
            {
                CG_Views[i].alpha = 0;
                CG_Views[i].blocksRaycasts = false;
            }
        }
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
