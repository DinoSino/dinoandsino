using System.Collections;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;



    public class LoginInfo
    {
        public string response;


        public LoginInfo(string response)
        {
            response = response;
        }

    }

    public class LoginBtn : MonoBehaviour
    {
        [Header("LoginPanel")]
        public InputField emailField;
        public InputField passwordfield;

        [Header("RegisterPanel")]
        public InputField newemailfield;
        public InputField newpasswordfield;
        public InputField newagefield;
        public InputField newnicknamefield;
        public GameObject RegisterPanelObj;
        public GameObject LoginPanelObj;
        public string LoginUrl;
        public string CreateUrl;
        public string ValidateUrl;
        
        public int newStickersino = 0;
        public int Stickersino;
        public int newStickerdino = 100;
        public int Stickerdino;
        public int ans;
        public int brush;
        public int eat;

        public GameObject useIDPanel;
        public GameObject yesIDPanel;
        public GameObject notLoginPanel;


        public List<LoginInfo> LogList = new List<LoginInfo>();
        void Start()
        {

            LoginUrl = "dustn1259.cafe24.com/Login.php";
            CreateUrl = "dustn1259.cafe24.com/Register.php";
            ValidateUrl = "dustn1259.cafe24.com/UserValidate.php";

        }
        public void LoginButton()

        {
            StartCoroutine(LoginCo());

        }

        IEnumerator LoginCo()
        {
            Debug.Log(emailField.text);
            Debug.Log(passwordfield.text);

            WWWForm form = new WWWForm();
            form.AddField("userID", emailField.text);
            form.AddField("userPassword", passwordfield.text);
            WWW webRequest = new WWW(LoginUrl, form);

            yield return webRequest;
            JSONObject obj = new JSONObject(webRequest.text);
           

            var success = obj.GetField("success").ToString() == "false" ? false : true;
            if (success)
            {
                SceneManager.LoadScene("storytellingScene1");
               
                
            }
            else
            {
                Debug.Log("2");
                notLoginPanel.SetActive(true);
            }

            //var nick = obj.GetField(newnicknamefield.text).ToString();
            var nick = obj.GetField("usernickName").ToString();////제생각엔 이런식으로 값 저장했던것 같습니다.
            var uemail = emailField.text;
            PlayerPrefs.SetString("uemail", uemail);
            PlayerPrefs.SetString("Name", nick);
            Debug.Log("저장닉네임");
            Debug.Log(PlayerPrefs.GetString("Name"));
            Debug.Log(uemail);
            Debug.Log(success); //로그인 결과와 상관없이 success를 찍습니다. 
            Debug.Log(success.GetType()); //타입을 보니 JSONObject입니다.
            Debug.Log(obj);
            Debug.Log(nick);

            var brushing = obj.GetField("stickerDino").ToString();
            Int32.TryParse(brushing, out brush);
            PlayerPrefs.SetInt("stickerDino", brush);
            Debug.Log(brush);

            var eating = obj.GetField("stickerSino").ToString();
            Int32.TryParse(eating, out eat);
            PlayerPrefs.SetInt("stickerSino", eat);
            Debug.Log("eat: " + eat);

            LogList.Add(new LoginInfo(nick.ToString())); //아무값도 저장되지 않는 것 같습니다. null값이 저장되니 이렇게 받는게 아닌듯 합니다.

            Debug.Log(webRequest.text);
            JsonData InfoJson = JsonMapper.ToJson(LogList);  //json파일 만들기
            File.WriteAllText(Application.dataPath + "/Resource/LoginData.json", InfoJson.ToString());
            string Jsonstring = File.ReadAllText(Application.dataPath + "/Resource/LoginData.json");
            Debug.Log(Jsonstring);

            
        }
        
        static void LoadDino(JSONObject obj)
        {
            
        }
        public void notLogin()
        {
            notLoginPanel.SetActive(false);
        }

        public void OpenCreateAccountBtn()
        {
            RegisterPanelObj.SetActive(true);//패널이 뜨게 된다.
        }

        public void CreateAccountBtn()
        {

            if (newemailfield.text != "" && newpasswordfield.text != "" && newnicknamefield.text != "" && newagefield.text != "")
            {
                StartCoroutine(CreateCo());
            }
            else
            {
                Debug.Log("회원가입 실패");
            }

        }

        IEnumerator CreateCo()
        {
            Debug.Log(newemailfield.text);
            Debug.Log(newpasswordfield.text);
            Debug.Log(newagefield.text);
            Debug.Log(newnicknamefield.text);

            WWWForm form = new WWWForm();

            form.AddField("userID", newemailfield.text);
            form.AddField("userPassword", newpasswordfield.text);
            form.AddField("usernickName", newnicknamefield.text);
            form.AddField("userAge", newagefield.text);
            form.AddField("stickerSino", newStickersino);
            form.AddField("stickerDino", newStickerdino);
            WWW webRequest = new WWW(CreateUrl, form);
            yield return webRequest;
            Debug.Log(webRequest.text);
            JSONObject obj = new JSONObject(webRequest.text);
            var success = obj.GetField("success").ToString() == "false" ? false : true;
            if (success && ans == 1)
            {

                SceneManager.LoadScene("Login");
                Debug.Log("회원가입 성공");


            }
            else
            {

                Debug.Log("회원가입실패");
            }


        }
        public void ValidateBtn()
        {
            StartCoroutine(ValidateCo());
        }
        IEnumerator ValidateCo()
        {
            WWWForm form = new WWWForm();

            form.AddField("userID", newemailfield.text);
            WWW webRequest = new WWW(ValidateUrl, form);
            yield return webRequest;
            JSONObject obj = new JSONObject(webRequest.text);

            var IDSuccess = obj.GetField("success").ToString() == "false" ? false : true;
            if (IDSuccess)
            {
                ans = 1;
                yesIDPanel.SetActive(true);
                Debug.Log("중복아이디없음");
            }
            else
            {
                useIDPanel.SetActive(true);
                ans = 2;
                Debug.Log("회원가입진행불가");

                //알림창은 만들어야함

            }
        }
        public void Check()
        {
            useIDPanel.SetActive(false);
            yesIDPanel.SetActive(false);
        }

    }
