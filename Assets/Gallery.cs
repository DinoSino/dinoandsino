using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;



public class Gallery : MonoBehaviour
{
    public Image image;

    void Start()
    {
#if UNITY_EDITOR
#else
        NativeCamera.TakePicture(callback);
#endif
    }

    // 카메라 촬영이후 호출되는 콜백함수
    private void callback(string path)
    {
        UpdateImage(path);
    }

    void UpdateImage(string path)
    {

        Texture2D tex = loadImage(image.rectTransform.sizeDelta, path);

        // texture2d로 sprite 생성
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    // 이미지 파일을 texture2d로 변환
    private static Texture2D loadImage(Vector2 size, string filePath)
    {

        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);

        return texture;
    }
    NativeCamera.ImageProperties prop = NativeCamera.GetImageProperties(Application.dataPath);
    

}
