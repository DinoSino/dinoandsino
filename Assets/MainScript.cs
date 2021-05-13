using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public Text nickText;
    public int dino_level;
    public int sino_level;

    public GameObject dino1;
    public GameObject dino2;
    public GameObject dino3;
    public GameObject dino4;
    public GameObject dino5;

    public GameObject sino1;
    public GameObject sino2;
    public GameObject sino3;
    public GameObject sino4;
    public GameObject sino5;
    // Start is called before the first frame update
   
    void Start()
    {   
        nickText.text = PlayerPrefs.GetString("Name");
        dino_level = PlayerPrefs.GetInt("stickerDino");
        sino_level = PlayerPrefs.GetInt("stickerSino");
        DinoSino();
       
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
    public void DinoSino()
    {
        int level_text_dino = (dino_level - 100) / 10;
        int level_text_sino = sino_level / 10;
        Debug.Log(level_text_dino);
        Debug.Log(level_text_sino);

        if (level_text_dino == 0 )
        {
            dino1.SetActive(true);
        }
        if(level_text_dino == 1)
        {
            dino2.SetActive(true);
        }
        if(level_text_dino == 2)
        {
            dino3.SetActive(true);
        }
        if(level_text_dino == 3)
        {
            dino4.SetActive(true);
        }
        if(level_text_dino == 4)
        {
            dino5.SetActive(true);
        }
        //sino
        if (level_text_sino == 0)
        {
            sino1.SetActive(true);
        }
        if (level_text_sino == 1)
        {
            sino2.SetActive(true);
        }
        if (level_text_sino == 2)
        {
            sino3.SetActive(true);
        }
        if (level_text_sino == 3)
        {
            sino4.SetActive(true);
        }
        if (level_text_sino == 4)
        {
            sino5.SetActive(true);
        }
        
    }
}
