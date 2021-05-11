using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gobackMyCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public void Click()
    {
        SceneManager.LoadScene("MyCharacter");
     }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
