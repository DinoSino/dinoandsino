using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class question_bt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject questionPanel1;
    public GameObject questionPanel2;
    public GameObject questionPanel3;
  
    public void qPanel1()
    {
        questionPanel1.SetActive(true);
    }
    public void qPanel2()
    {
        questionPanel2.SetActive(true);
    }
    public void qPanel3()
    {
        questionPanel3.SetActive(true);
    }
    public void backPanel()
    {
        questionPanel1.SetActive(false);
        questionPanel2.SetActive(false);
        questionPanel3.SetActive(false);
    }
}
