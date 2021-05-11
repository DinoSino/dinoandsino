using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using UnityEngine.UI;
using System;
using System.IO;
using System.Data;
using UnityEngine.Networking;

public class gotoMyCharacter : MonoBehaviour
{
    private Button button;
    public string email;
    public int eat_num;
    public int eat_numpp;

    // Button yourButton = GameObject.Find("yourButton Name").GetComponent<Button>();
    //public InputField sticker_sino;

    public string StickersinoUrl;
    void Start()
    {
        eat_num = PlayerPrefs.GetInt("stickerSino");//int로
        Debug.Log("eenum" + eat_num);

        email = PlayerPrefs.GetString("uemail");
        Debug.Log(email);

        

        Button button = GameObject.Find("ok_button").GetComponent<Button>();
        StickersinoUrl = "dustn1259.cafe24.com/Stickersino.php";//php 파일

    }

    // Start is called before the first frame update
    public void Click()
    {

        StartCoroutine(StickersinoCo());
        
       
    }


    public IEnumerator StickersinoCo() //버튼 누르면 +1
    {
        
        Debug.Log(email);
        eat_numpp = eat_num + 1;
        Debug.Log(eat_numpp);
        WWWForm form = new WWWForm();
        form.AddField("stickerSino", eat_numpp);
        form.AddField("userID", email);
        WWW webRequest = new WWW(StickersinoUrl, form);//php 주소 들어가야 함
        yield return webRequest;
        Debug.Log("test");
        Debug.Log(webRequest.text);
        JSONObject obj = new JSONObject(webRequest.text);
        
        
        var onClick = obj.GetField("success").ToString() == "false" ? false : true;

        if (onClick)//버튼 누를 시에 +1
        {
            PlayerPrefs.SetInt("tmpSino", eat_numpp);
            SceneManager.LoadScene("MyCharacter");
            Debug.Log(eat_numpp);
            
            PlayerPrefs.SetInt("stickerSino", eat_numpp);


        }
        else
        {
            Debug.Log("False");
        }



    }


}