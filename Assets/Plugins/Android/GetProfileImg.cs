using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GetProfileImg : MonoBehaviour
{
    public int _RESOLUTION = 256;
    public RawImage profileImg;
    public RawImage[] _rawImage;


    [SerializeField] private Image bakcgroundImg;

    public string LocalPath
    {
        get { return PlayerPrefs.GetString("profileImgPath" + "/"); }
        set { PlayerPrefs.SetString("profileImgPath" + "/", value); }

    }

    private void Awake()
    {
        SwitchImage(false);

        try
        {
            byte[] data = System.IO.File.ReadAllBytes(LocalPath);
            foreach (RawImage rawImg in _rawImage)
            {
                rawImg.material.mainTexture = Base64ToTexture2D(data);
                rawImg.texture = Base64ToTexture2D(data);
                rawImg.color = new Color(1, 1, 1, 1);
            }
        }
        catch
        {
            //Local Path가 없는데요?
        }
    }

    private Texture2D Base64ToTexture2D(byte[] imageData)
    {
        int width, height;
        GetImageSize(imageData, out width, out height);

        // 매프레임 new를 해줄경우 메모리 문제 발생 -> 멤버 변수로 변경
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);

        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(imageData);
        texture.Apply();

        return texture;
    }
    private void GetImageSize(byte[] imageData, out int width, out int height)
    {
        width = ReadInt(imageData, 3 + 15);
        height = ReadInt(imageData, 3 + 15 + 2 + 2);
    }

    private int ReadInt(byte[] imageData, int offset)
    {
        return (imageData[offset] << 8 | imageData[offset + 1]);
    }
    private float xValue, yValue;

    public float[] MoveImage(float x, float y)
    {
        float xMove = 0.0f, yMove = 0.0f;
        float[] result;
        xMove = -x / 30;
        yMove = -y / 30;
        if (xValue < yValue)
        {
            xMove = Mathf.Clamp(xMove, 0, 1 - xValue);
            profileImg.uvRect = new Rect(xMove, 0, xValue, yValue);
            yMove = 0;
            result = new float[] { xMove, 0 };
            return result;
        }
        else if (xValue > yValue)
        {
            yMove = Mathf.Clamp(yMove, 0, 1 - yValue);
            xMove = 0;
            profileImg.uvRect = new Rect(0, yMove, xValue, yValue);
            result = new float[] { 0, yMove };
            return result;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator CoSaveImg(Texture _texture, float x, float y)
    {
        yield return new WaitForEndOfFrame();
        var width = (int)(_RESOLUTION * (xValue >= 1 ? 1 : 1 / xValue));
        var height = (int)(_RESOLUTION * (yValue >= 1 ? 1 : 1 / yValue));
        Texture2D texture = ScaleTexture((Texture2D)_texture, width, height, 0, 0);

        Color[] colors = texture.GetPixels((int)(x * width), (int)(y * width), _RESOLUTION, _RESOLUTION);
        texture.Apply();

        Texture2D finTexture = new Texture2D(_RESOLUTION, _RESOLUTION, texture.format, false, false);
        finTexture.SetPixels(0, 0, _RESOLUTION, _RESOLUTION, colors);
        finTexture.Apply();
        profileImg.texture = finTexture;
        profileImg.uvRect = new Rect(0, 0, 1, 1);
        xValue = 1;
        yValue = 1;
        byte[] bytes = finTexture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + "test.png", bytes);
        LocalPath = Application.persistentDataPath + "/" + "test.png";
    }

    private void SwitchImage(bool sw)
    {
        int value = sw ? 1 : 0;
        bakcgroundImg.GetComponent<CanvasGroup>().alpha = 1 - value;
        bakcgroundImg.GetComponent<CanvasGroup>().interactable = !sw;
        profileImg.GetComponent<CanvasGroup>().alpha = value;
    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    return;
                }
                profileImg.texture = texture;
                xValue = texture.texelSize.x;
                yValue = texture.texelSize.y;
                if (xValue > yValue) { yValue = yValue / xValue; xValue = 1; }
                else if (xValue < yValue) { xValue = xValue / yValue; yValue = 1; }
                else { xValue = 1; yValue = 1; }
                profileImg.uvRect = new Rect(0, 0, xValue, yValue);
                SwitchImage(true);
            }
        });
    }

    public void OnClickProfileSwtich()
    {
        PickImage(_RESOLUTION);
    }


    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, int posX, int posY)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(posX, posY, targetWidth, targetHeight, rpixels, 0);
        result.Apply();
        return result;
    }


}