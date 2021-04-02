using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class showSticker : MonoBehaviour
{
    public int brush_level;
    public int eat_level;

    public GameObject stamp1;
    public GameObject stamp2;
    public GameObject stamp3;
    public GameObject stamp4;
    public GameObject stamp5;
    public GameObject stamp6;
    public GameObject stamp7;
    public GameObject stamp8;
    public GameObject stamp9;
    public GameObject stamp10;

    public GameObject stamp101;
    public GameObject stamp102;
    public GameObject stamp103;
    public GameObject stamp104;
    public GameObject stamp105;
    public GameObject stamp106;
    public GameObject stamp107;
    public GameObject stamp108;
    public GameObject stamp109;
    public GameObject stamp110;
    //------------------------------//
    //숫자계산//
    
    //-----이미지-------//
    
    //------씬버튼------//
    public GameObject lookDino1;
    public GameObject lookDino2;
    public GameObject lookDino3;
    public GameObject lookDino4;
    public GameObject lookDino5;

    public GameObject lookSino1;
    public GameObject lookSino2;
    public GameObject lookSino3;
    public GameObject lookSino4;
    public GameObject lookSino5;

    public string Email;

    //sino 먹는거

    public int sticker_eat;
    public int sticker_eatpp;
    public int sticker_eataa;
    public int eat_lev;

    //dino 이닦는거

    public int sticker_brush;
    public int sticker_brushpp;
    public int sticker_brushaa;
    public int brush_lev;


    // Start is called before the first frame update
    void Start()
    {
        
        Email = PlayerPrefs.GetString("userID");
        Debug.Log("저장 아이디");
        Debug.Log(PlayerPrefs.GetString("userID"));
        showSino();
        showDino();
       
    }
    public void showSino()
    {
        
        sticker_eatpp = PlayerPrefs.GetInt("tmpSino");//gotomycharacter에서 버튼 눌러서 넘어온 애
        sticker_eat = PlayerPrefs.GetInt("stickerSino");//로그인에서 바로 온 애
        Debug.Log(sticker_eatpp);
        Debug.Log(sticker_eat);

      
        //로그인에서 온 애 int로 바꾼거
        
        if (sticker_eatpp > sticker_eat)
        {
        
            eat_lev = sticker_eatpp;
            Debug.Log("eat_lev: " + eat_lev);
            sinoLev();
            //sticker_eatpp = 0;
            
        }
        else
        {
            eat_lev = sticker_eat;
            Debug.Log("eat_lev: " + eat_lev);
            sinoLev();

        }
      
    }
    public void showDino()
    {   
        sticker_brushpp = PlayerPrefs.GetInt("tmpDino");//gotoMyCharacter_brush에서 온 애
        sticker_brush = PlayerPrefs.GetInt("stickerDino");//로그인 시에
        Debug.Log(sticker_brushpp);
        Debug.Log(sticker_brush);
        //ddkk = PlayerPrefs.GetInt("stickerDino1");

        //Debug.Log("sticker_brushaa: " + sticker_brushaa);

        if(sticker_brushpp > sticker_brush)
        {
            //sticker_brushpp = 0;
            brush_lev = sticker_brushpp;
            Debug.Log("sticker_brush_num: " + brush_level);
            dinoLev();
            
        }
        else
        {
            brush_lev = sticker_brush;
            Debug.Log("sticker_brush_num: "+ brush_level);
            dinoLev();
        }
        //sticker_brushpp = 0;
    }
  

    public void dinoLev()
    {
        int brush_level = (brush_lev-100) / 10;
        Debug.Log("brush_level: " + brush_level);
        int brush_stamp = (brush_lev-100) % 10;

        
        switch (brush_level)
        {
            case 0:
                lookDino1.SetActive(true);
                break;
            case 1:
                lookDino2.SetActive(true);
                
                break;
            case 2:
                lookDino3.SetActive(true);
                break;
            case 3:
                lookDino4.SetActive(true);
                break;
            case 4:
               lookDino5.SetActive(true);
                break;
        }
        
    


        switch (brush_stamp)
        {
            case 1:
                stamp101.SetActive(true);
                break;
            case 2:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                break;
            case 3:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                break;
            case 4:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                break;
            case 5:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                break;
            case 6:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                stamp106.SetActive(true);
                break;
            case 7:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                stamp106.SetActive(true);
                stamp107.SetActive(true); 
                break;
            case 8:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                stamp106.SetActive(true);
                stamp107.SetActive(true);
                stamp108.SetActive(true);
                break;
            case 9:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                stamp106.SetActive(true);
                stamp107.SetActive(true);
                stamp108.SetActive(true);
                stamp109.SetActive(true);
                break;
            case 10:
                stamp101.SetActive(true);
                stamp102.SetActive(true);
                stamp103.SetActive(true);
                stamp104.SetActive(true);
                stamp105.SetActive(true);
                stamp106.SetActive(true);
                stamp107.SetActive(true);
                stamp108.SetActive(true);
                stamp109.SetActive(true);
                stamp110.SetActive(true);
                break;

        }
    }
    public void isClickDino()
    {
        if (brush_level == 0)
        {    
            Debug.Log("Click");
            SceneManager.LoadScene("LookDino1");
           
        }
        if (brush_level == 1)
        {
            SceneManager.LoadScene("LookDino2");
        }
        if (brush_level == 2)
        {
            SceneManager.LoadScene("LookDino3");
        }
        if (brush_level == 3)
        {
            SceneManager.LoadScene("LookDino4");
        }
        if (brush_level == 4)
        {
            SceneManager.LoadScene("LookDino5");
        }
    }
    public void isClickSino()
    {
        if (eat_level == 0)
        {
            Debug.Log("Click");
            SceneManager.LoadScene("LookSino1");
            
        }
        if (eat_level == 1)
        {
            SceneManager.LoadScene("LookSino2");
        }
        if (eat_level == 2)
        {
            SceneManager.LoadScene("LookSino3");
        }
        if (eat_level == 3)
        {
            SceneManager.LoadScene("LookSino4");
        }
        if (eat_level == 4)
        {
            SceneManager.LoadScene("LookSino5");
        }
    }
    public void sinoLev()
    {

        int eat_level = eat_lev / 10;
        int eat_stamp = eat_lev % 10;

        switch (brush_level)
        {
            case 0:
                
                lookSino1.SetActive(true);
                break;
            case 1:
                lookSino2.SetActive(true);

                break;
            case 2:
                lookSino3.SetActive(true);
                break;
            case 3:
                lookSino4.SetActive(true);
                break;
            case 4:
                lookSino5.SetActive(true);
                break;
        }

        switch (eat_stamp)
        {
            case 1:
                stamp1.SetActive(true);
                break;
            case 2:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                break;
            case 3:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                break;
            case 4:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                break;
            case 5:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                break;
            case 6:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                stamp6.SetActive(true);
                break;
            case 7:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                stamp6.SetActive(true);
                stamp7.SetActive(true);
                break;
            case 8:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                stamp6.SetActive(true);
                stamp7.SetActive(true);
                stamp8.SetActive(true);
                break;
            case 9:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                stamp6.SetActive(true);
                stamp7.SetActive(true);
                stamp8.SetActive(true);
                stamp9.SetActive(true);
                break;
            case 10:
                stamp1.SetActive(true);
                stamp2.SetActive(true);
                stamp3.SetActive(true);
                stamp4.SetActive(true);
                stamp5.SetActive(true);
                stamp6.SetActive(true);
                stamp7.SetActive(true);
                stamp8.SetActive(true);
                stamp9.SetActive(true);
                stamp10.SetActive(true);
                break;

        }
    }


}