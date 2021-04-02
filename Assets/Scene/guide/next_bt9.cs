using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class next_bt9 : MonoBehaviour
{
    public void exitClick()
    {
        SceneManager.LoadScene("Main");
    }
    public void priorClick()
    {
        SceneManager.LoadScene("guideline3");
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
