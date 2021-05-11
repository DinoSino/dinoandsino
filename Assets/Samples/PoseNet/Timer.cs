using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;
    
    public GameObject Panel_pause;
    private bool stopTimer;


    void Start()
    {   //여기에 찍죠 
        
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        float time = gameTime - Time.time;//이게 time 남은건데 

        float minutes = Mathf.FloorToInt(( time / 60) +1);

        string textTime = string.Format("{00}분", minutes);

        if (time <=0)//이미지 다름-> background에 맞춰지고 있어서 다른거로 수정 필요?

        {
            textTime = "0분";
            stopTimer = true;
            time = gameTime;
            Debug.Log(time);
            
            SceneManager.LoadScene("FinishEating");
            
        }

        if(stopTimer == false)
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
