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

public class gotoMyCharacter_brush : MonoBehaviour
{
    private Button button;
    public string email;
    public int brush_num;
    public int brush_numpp;


    public string StickerdinoUrl;

    public List<LoginInfo> LogList = new List<LoginInfo>();//??���� �տ��κ��̶� �Ȱ��� �غ����� �����Ծ�� ������ �ſ�
    void Start()
    {
        brush_num = PlayerPrefs.GetInt("stickerDino");//int��
        Debug.Log("bbnum"+brush_num); 

        email = PlayerPrefs.GetString("uemail");
        Debug.Log(email);

        Button button = GameObject.Find("ok_button").GetComponent<Button>();
        StickerdinoUrl = "dustn1259.cafe24.com/Stickerdino.php";//php ����

    }

    // Start is called before the first frame update
    public void Click()
    {

        StartCoroutine(StickerdinoCo());


    }


    public IEnumerator StickerdinoCo()
    {
        Debug.Log(email);
        brush_numpp = brush_num + 1;
        Debug.Log(brush_numpp);
        WWWForm form = new WWWForm();
        form.AddField("stickerDino", brush_numpp);
        form.AddField("userID", email);
        WWW webRequest = new WWW(StickerdinoUrl, form);//php �ּ� ���� ��
        yield return webRequest;
        Debug.Log(webRequest.text);
        JSONObject obj = new JSONObject(webRequest.text);

        var onClick = obj.GetField("success").ToString() == "false" ? false : true;

        if (onClick)//��ư ���� �ÿ� +1
        {
            PlayerPrefs.SetInt("tmpDino", brush_numpp);
            SceneManager.LoadScene("MyCharacter");
            Debug.Log(brush_numpp);
            PlayerPrefs.SetInt("stickerDino", brush_numpp);

        }
        else
        {
            Debug.Log("False");
        }



    }


    // Update is called once per frame

}