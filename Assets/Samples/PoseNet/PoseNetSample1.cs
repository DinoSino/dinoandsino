using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PoseNetSample1 : MonoBehaviour
{
    public static PoseNetSample1 Inst = null;

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
    public Sprite[] images;
    public Image random_text_images;
    public Sprite t0;
    public Sprite t1;
    public Sprite t2;
    public Sprite t3;
    public Sprite[] text_images;
    public int k = 0;

    public Slider timerSlider;
    public Text timerText;
    public float gameTime = 100.0f;
    float time;
    
    private bool stopTimer;

    [SerializeField] ParticleSystem ps;
    [SerializeField] AudioSource audioSource;
    [SerializeField, FilePopup("*.tflite")] string fileName = "posenet_mobilenet_v1_100_257x257_multi_kpt_stripped.tflite";
    [SerializeField] RawImage cameraView = null;
    [SerializeField, Range(0f, 1f)] float threshold = 0.3f;
    [SerializeField, Range(0f, 1f)] float lineThickness = 0.5f;
    [SerializeField] bool runBackground;

    WebCamTexture webcamTexture;
    PoseNet1 poseNet1;
    Vector3[] corners = new Vector3[4];
    PrimitiveDraw draw;
    UniTask<bool> task;
    PoseNet1.Result[] results;
    CancellationToken cancellationToken;
    // PoseNet. obj;
    public bool audioSW;
    public bool effectSW;
    public float ttk;
    //audio
    //public AudioClip[] Music = new AudioClip[4];
    
    public GameObject Panel_pause;
    

    void Start()
    {
        gameTime = 30.0f;//이거로 시간 조정!
        time = 0.0f;
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
        ttk = gameTime;

        images = new Sprite[4];
        images[0] = s0;
        images[1] = s1;
        images[2] = s2;
        images[3] = s3;

        text_images = new Sprite[4];
        text_images[0] = t0;
        text_images[1] = t1;
        text_images[2] = t2;
        text_images[3] = t3;
        StartCoroutine(changeImage());
        //audio 랜덤
        
        audioSource.Play();
        ps.Play();
        
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        poseNet1 = new PoseNet1(path);
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
        poseNet1?.Dispose();
        draw?.Dispose();
        Inst = null;
    }

    void Update()
    {
        time = gameTime -= Time.deltaTime;

        int seconds = Mathf.FloorToInt(time - 1f+1);
        int minutes = (seconds / 60) + 1;

        //Debug.Log($"time : {time} , gameTime : {gameTime}, seconds : {minutes}");
        string textTime = string.Format("{00}분",minutes);
        //Debug.Log(textTime);

        if (time <= 0)//

        {
            textTime = "0분";
            stopTimer = true;
            gameTime = ttk;
            SceneManager.LoadScene("FinishEating");
        }

        if (stopTimer == false)
        {
            timerText.text = textTime;
            timerSlider.value = time;
         
        }
        if(stopTimer == true)
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
            poseNet1.Invoke(webcamTexture);
            results = poseNet1.GetResults();
            Debug.Log(results);
            cameraView.material = poseNet1.transformMat;
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
            
            //audioSource.clip = Music[Random.Range(0, Music.Length)];
            audioSource.volume = 1.0f;
            //audioSource.Play();
        }
        else
        {
            
            //audioSource.clip = Music[Random.Range(0, Music.Length)];
            audioSource.volume = 0.0f;
            
        }
    }

    public void playeffect(bool ef)
    {
        if (ef)
        {
            ps.Emit(1);
        }
        else
        {
            ps.Emit(0);
        }
    }


    async UniTask<bool> InvokeAsync()
    {
        results = await poseNet1.InvokeAsync(webcamTexture, cancellationToken);
        cameraView.material = poseNet1.transformMat;
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