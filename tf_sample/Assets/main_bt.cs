using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_bt : MonoBehaviour
{
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
