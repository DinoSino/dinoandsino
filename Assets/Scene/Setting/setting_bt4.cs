using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setting_bt4 : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Panel;

    public void logout()
    {
        Panel.SetActive(true);
    }
   public void Notice()
    {
        SceneManager.LoadScene("Notice");
    }
    // Update is called once per frame
  public void Question()
    {
        SceneManager.LoadScene("Question");
    }
    public void Cancel()
    {
        Panel.SetActive(false);
    }
}
