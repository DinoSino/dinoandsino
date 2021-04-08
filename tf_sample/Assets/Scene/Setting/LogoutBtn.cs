using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutBtn : MonoBehaviour
{
    public string LogoutUrl;
    // Start is called before the first frame update
    void Start()
    {
        LogoutUrl = "dustn1259.cafe24.com/Logout.php";
    }
    public void Logout()
    {
        Debug.Log("Logout");
        SceneManager.LoadScene("Login");
        
    }
  
}
