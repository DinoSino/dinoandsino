using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PoseNetSample : MonoBehaviour
{
    public static PoseNetSample Inst = null;
    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            if (Inst != this)
            {
                Destroy(gameObject);
            }
        }

    }
    public Image randomImage;
    public Sprite s0;
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;
    public Sprite s6;
    public Sprite s7;
    public Sprite s8;
    public Sprite s9;

    public Sprite[] images;
    public Image random_text_images;
    public Sprite t0;
    public Sprite t1;
    public Sprite t2;
    public Sprite t3;
    public Sprite t4;
    public Sprite t5;
    public Sprite t6;
    public Sprite t7;
    public Sprite t8;
    public Sprite t9;

    public Sprite[] text_images;
    public int k = 0;

    public Slider timerSlider;
    public Text timerText;
    public float gameTime = 180.0f;
    float time;

    public GameObject Panel_pause;//일시정지 버튼

    private bool stopTimer;

    [SerializeField] ParticleSystem ps;
    [SerializeField] AudioSource audioSource;
    [SerializeField, FilePopup("*.tflite")] string fileName = "posenet_mobilenet_v1_100_257x257_multi_kpt_stripped.tflite";
    [SerializeField] RawImage cameraView = null;
    [SerializeField, Range(0f, 1f)] float threshold = 0.3f;
    [SerializeField, Range(0f, 1f)] float lineThickness = 0.5f;
    [SerializeField] bool runBackground;

    WebCamTexture webcamTexture;
    PoseNet poseNet;
    Vector3[] corners = new Vector3[4];
    PrimitiveDraw draw;
    UniTask<bool> task;
    PoseNet.Result[] results;
    CancellationToken cancellationToken;

    public bool audioSW;
    public bool effectSW;
    public float ttk;

    void Start()
    {
        gameTime = 10.0f;//이거로 시간 조정!
        time = 0.0f;
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
        ttk = gameTime;

        images = new Sprite[10];
        images[0] = s0;
        images[1] = s1;
        images[2] = s2;
        images[3] = s3;
        images[4] = s4;
        images[5] = s5;
        images[6] = s6;
        images[7] = s7;
        images[8] = s8;
        images[9] = s9;


        text_images = new Sprite[10];
        text_images[0] = t0;
        text_images[1] = t1;
        text_images[2] = t2;
        text_images[3] = t3;
        text_images[4] = t4;
        text_images[5] = t5;
        text_images[6] = t6;
        text_images[7] = t7;
        text_images[8] = t8;
        text_images[9] = t9;
        StartCoroutine(changeImage());

        audioSource.Play();
        ps.Play();

        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        poseNet = new PoseNet(path);
        // Init camera
        WebCamDevice[] devices = WebCamTexture.devices;

        int selectedCameraIndex = -1;
        for (int i = 0; i < devices.Length; i++)
        {
            // 사용 가능한 카메라 로그
            Debug.Log("Available Webcam: " + devices[i].name + ((devices[i].isFrontFacing) ? "(Front)" : "(Back)"));

            // 후면 카메라인지 체크
            if (devices[i].isFrontFacing == true)
            {
                // 해당 카메라 선택
                selectedCameraIndex = i;
                break;
            }
        }

        string cameraName = devices[selectedCameraIndex].name;
        // string cameraName = WebCamUtil.FindName();
        webcamTexture = new WebCamTexture(cameraName, 360, 740, 30);
        webcamTexture.Play();
        cameraView.texture = webcamTexture;


        draw = new PrimitiveDraw()
        {
            color = Color.clear,
        };

        cancellationToken = this.GetCancellationTokenOnDestroy();

    }
    IEnumerator changeImage()//image
    {

        float timer = 0.0f;
        while (true)
        {
            if (timer > 20.0f)
            { //아직 에러존재
                if (k < images.Length && k < text_images.Length)
                {
                    randomImage.sprite = images[k];
                    random_text_images.sprite = text_images[k];
                    k = k + 1;
                    StartCoroutine(changeImage());
                    yield break;

                }


            }
            timer += Time.deltaTime;

            yield return null;
        }
    }

    void OnDestroy()
    {
        webcamTexture?.Stop();
        poseNet?.Dispose();
        draw?.Dispose();
        Inst = null;
    }

    void Update()
    {
        time = gameTime -= Time.deltaTime;


        int seconds = Mathf.FloorToInt(time - 1f + 1);

        //Debug.Log($"time : {time} , gameTime : {gameTime}, seconds : {seconds}");
        string textTime = string.Format("{000}초", seconds);

        if (time <= 0)//

        {
            textTime = "0초";
            stopTimer = true;
            gameTime = ttk;
            SceneManager.LoadScene("FinishBrush");
            //이렇게 돌려볼게요

        }

        if (stopTimer == false)
        {
            timerText.text = textTime;
            timerSlider.value = time;


        }
        if (stopTimer == true)
        {
            time = gameTime += Time.deltaTime;
        }


        playeffect(effectSW);
        playon(audioSW);
        if (runBackground)
        {
            if (task.Status.IsCompleted())
            {
                task = InvokeAsync();
            }
        }
        else
        {
            poseNet.Invoke(webcamTexture);
            results = poseNet.GetResults();
            Debug.Log(results);
            cameraView.material = poseNet.transformMat;
        }

        if (results != null)
        {
            DrawResult();
        }
    }

    void DrawResult()
    {
        var rect = cameraView.GetComponent<RectTransform>();
        rect.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];

        // var connections = PoseNet.Connections;
        // int len = connections.GetLength(0);
        // for (int i = 0; i < len; i++)
        // {
        //     var a = results[(int)connections[i, 0]];
        //     var b = results[(int)connections[i, 1]];
        //     if (a.confidence >= threshold && b.confidence >= threshold)
        //     {
        //         draw.Line3D(
        //             MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
        //             MathTF.Lerp(min, max, new Vector3(b.x, 1f - b.y, 0)),
        //             lineThickness
        //         );
        //     }
        // }


        /* for (int i = 0; i < 5; i++)
         {
             var a = results[i];
             draw.Point(
                 MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
                 0.5f
             );
         }
        */

        for (int i = 10; i < 11; i++)
        {
            var a = results[i];
            draw.Point(
                MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
                0.5f
            );


        }

        draw.Apply();
    }


    public void playon(bool sw)
    {
        if (sw)
        {
            audioSource.volume = 1.0f;
            stopTimer = true;
        }
        else
        {
            audioSource.volume = 0.0f;
            stopTimer = false;

        }
    }

    public void playeffect(bool ef)
    {
        if (ef)
        {
            ps.Emit(1);
            stopTimer = true;
        }
        else
        {
            ps.Emit(0);
            stopTimer = false;
        }
    }


    async UniTask<bool> InvokeAsync()
    {
        results = await poseNet.InvokeAsync(webcamTexture, cancellationToken);
        cameraView.material = poseNet.transformMat;
        return true;
    }
    public void ClickPuase()
    {
        Panel_pause.SetActive(true);
        stopTimer = true;
        webcamTexture.Stop();
        audioSource.Stop();
        ps.Emit(0);


    }

    public void ContinueBtn()
    {
        Panel_pause.SetActive(false);
        stopTimer = false;
        webcamTexture.Play();
        audioSource.Play();
        ps.Emit(1);


    }
    public void BackBtn()//돌아가기 어디로?
    {
        SceneManager.LoadScene("Main");
    }
}