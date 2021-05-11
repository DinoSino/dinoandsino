using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class use : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Im;

    public AndroidPlugin AP;

    public void ButtonDown()
    {
        StartCoroutine(AP.ShowGallery(
            val:(_tex, _spr) => { Im.sprite = _spr; }
            
                ));
    }
}
