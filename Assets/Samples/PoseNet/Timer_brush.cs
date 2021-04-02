using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer_brush : MonoBehaviour//stop 버튼 수정 해야함
{
    // Start is called before the first frame update
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;
    public float time;

    public GameObject Panel_pause;

    private bool stopTimer;


    void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime; 
    }

    // Update is called once per frame
    void Update()
    {
        time = gameTime - Time.time;

        int seconds = Mathf.FloorToInt(time - 1f +1);
        

        string textTime = string.Format("{000}초", seconds);

        if (time <= 0)//

        {
            textTime = "0초";
            stopTimer = true;
            Start();
            SceneManager.LoadScene("FinishBrush");
            //이렇게 돌려볼게요

        }

        if (stopTimer == false)
        {
             timerText.text = textTime;
            timerSlider.value = time;
            
        }


    }

    public void ClickBtn()
    {
        Panel_pause.SetActive(true);
        stopTimer = true;
        

        
    }
    public void ContinueBtn()
    {
        Panel_pause.SetActive(false);
        stopTimer = false;

    }
    public void BackBtn()//돌아가기 어디로?
    {
        SceneManager.LoadScene("Main");
    }
}
//일시정지, 팝업창 버튼 