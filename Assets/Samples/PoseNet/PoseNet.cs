using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using TensorFlowLite;
using System.Timers;


namespace TensorFlowLite
{

    /// <summary>
    /// Pose Estimation Example
    /// https://www.tensorflow.org/lite/models/pose_estimation/overview
    /// </summary>
    public class PoseNet : BaseImagePredictor<float>
    {
        float x1;
        float y1;

        public enum Part
        {
            NOSE,
            LEFT_EYE,
            RIGHT_EYE,
            LEFT_EAR,
            RIGHT_EAR,
            LEFT_SHOULDER,
            RIGHT_SHOULDER,
            LEFT_ELBOW,
            RIGHT_ELBOW,
            LEFT_WRIST,
            RIGHT_WRIST,
            LEFT_HIP,
            RIGHT_HIP,
            LEFT_KNEE,
            RIGHT_KNEE,
            LEFT_ANKLE,
            RIGHT_ANKLE
        }

        public static readonly Part[,] Connections = new Part[,]
        {
            // HEAD
            { Part.LEFT_EAR, Part.LEFT_EYE },
            { Part.LEFT_EYE, Part.NOSE },
            { Part.NOSE, Part.RIGHT_EYE },
            { Part.RIGHT_EYE, Part.RIGHT_EAR },
            // BODY
            { Part.LEFT_HIP, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_WRIST },
            { Part.LEFT_HIP, Part.LEFT_KNEE },
            { Part.LEFT_KNEE, Part.LEFT_ANKLE },
            { Part.RIGHT_HIP, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_WRIST },
            { Part.RIGHT_HIP, Part.RIGHT_KNEE },
            { Part.RIGHT_KNEE, Part.RIGHT_ANKLE },
            { Part.LEFT_SHOULDER, Part.RIGHT_SHOULDER },
            { Part.LEFT_HIP, Part.RIGHT_HIP }
        };

        [System.Serializable]


        public struct Result
        {
            public Part part;
            public float confidence;
            public float x;
            public float y;
            

        }


        Result[] results = new Result[17];

        float[,,] outputs0; // heatmap
        float[,,] outputs1; // offset

        // float[] outputs2 = new float[9 * 9 * 32]; // displacement fwd
        // float[] outputs3 = new float[9 * 9 * 32]; // displacement bwd


        public PoseNet(string modelPath) : base(modelPath)
        {
            var odim0 = interpreter.GetOutputTensorInfo(0).shape;
            var odim1 = interpreter.GetOutputTensorInfo(1).shape;
            outputs0 = new float[odim0[1], odim0[2], odim0[3]];
            outputs1 = new float[odim1[1], odim1[2], odim1[3]];

        }

        public override void Invoke(Texture inputTex)
        {
            ToTensor(inputTex, input0);

            interpreter.SetInputTensorData(0, input0);
            interpreter.Invoke();
            interpreter.GetOutputTensorData(0, outputs0);
            interpreter.GetOutputTensorData(1, outputs1);
        }

        public async UniTask<Result[]> InvokeAsync(Texture inputTex, CancellationToken cancellationToken)
        {
            await ToTensorAsync(inputTex, input0, cancellationToken);
            await UniTask.SwitchToThreadPool();

            interpreter.SetInputTensorData(0, input0);
            interpreter.Invoke();
            interpreter.GetOutputTensorData(0, outputs0);
            interpreter.GetOutputTensorData(1, outputs1);

            var results = GetResults();

            await UniTask.SwitchToMainThread(cancellationToken);
            return results;
        }

        public Result[] GetResults()
        {
            // Name alias
            float[,,] scores = outputs0;
            float[,,] offsets = outputs1;
            float stride = scores.GetLength(0) - 1;

            float differentx;
            float differenty;
            Queue q = new Queue();


            ApplySigmoid(scores);
            var argmax = ArgMax2D(scores);

            // Add offsets
            for (int part = 0; part < results.Length; part++)
            {
                ArgMaxResult arg = argmax[part];
                Result res = results[part];
                GameObject obj;

                float offsetX = offsets[arg.y, arg.x, part + results.Length];
                float offsetY = offsets[arg.y, arg.x, part];
                res.x = ((float)arg.x / stride * width + offsetX) / width;
                res.y = ((float)arg.y / stride * height + offsetY) / height;
                res.confidence = arg.score;
                res.part = (Part)part;

                results[part] = res;

                //양치
                if (res.confidence > 0.01 && res.part.ToString() == "RIGHT_WRIST")//confidence 값에 따라 보이는 값이 달라짐 0~1사이(화면에 나타나면 1에 가깝게 나옴)
                {
                    // Debug.Log(res.part.ToString() + res.x.ToString() + res.y.ToString());
                      
                     differentx = x1 - res.x;
                     differenty = y1 - res.y;
                     x1 = res.x;
                     y1 = res.y;
                        
                    if ((0.2< res.x && res.x < 0.8) && (0.6 < res.y && res.y < 0.9))//입 주위에서 소리
                    {
                       if ((-0.03 < differentx && differentx < 0.03) && (-0.03< differenty && differenty < 0.03))//손 안움직일 때
                       {
                            PoseNetSample.Inst.effectSW = false;
                            PoseNetSample.Inst.audioSW = true;
                            Debug.Log("양치 안할 때  " + "differentx: " + differentx + "differenty: " + differenty);
                       }
                            // 손 움직일 때
                    else
                    {
                      // Debug.Log("x: " + res.x + "y: " + res.y + "differentx: " + differentx + "differenty: " + differenty);
                      PoseNetSample.Inst.audioSW = false;
                        PoseNetSample.Inst.effectSW = true;
                      Debug.Log("양치 할 때 " + "x1: " + x1 + "differentx:" + differentx + "y1: " + y1 + "differenty:" + differenty);
                    }
                    }
       
                }
                else//손이 안 보일 때
                {
                    if (res.part.ToString() == "RIGHT_WRIST")
                    {
                        //Debug.Log("res: " + res.confidence);
                        PoseNetSample.Inst.audioSW = true;
                        PoseNetSample.Inst.effectSW = false;
                        Debug.Log("인식 안됨");
         
                    }
                }

                //식사
                //Debug.Log(res.part.ToString()+res.confidence);
                /*if (res.confidence > 0.01 && res.part.ToString() == "RIGHT_WRIST")//confidence 값에 따라 보이는 값이 달라짐 0~1사이(화면에 나타나면 1에 가깝게 나옴)
                {
                    // Debug.Log(res.part.ToString() + res.x.ToString() + res.y.ToString());

                        differentx = x1 - res.x;
                        differenty = y1 - res.y;
                        x1 = res.x;
                        y1 = res.y;

                        //Debug.Log("x1:" + x1 + "y1:" + y1);
                        if ((0.2< res.x && res.x < 0.75) && (0.6 < res.y && res.y < 1.1))//입 주위에서 소리
                        {
                            
                           Debug.Log("differentx: " + differentx + "differenty:" + differenty);
                            if ((-0.05<differentx && differentx < 0.05) && (-0.05< differenty && differenty < 0.05))//손 안움직일 때
                            {
                                PoseNetSample.Inst.audioSW = true;
                                PoseNetSample.Inst.effectSW = false;
                                Debug.Log("식사 안 할 때  " + "differentx: " + differentx + "differenty: " + differenty);
                            }
                            // 손 움직일 때
                            else
                            {
                                // Debug.Log("x: " + res.x + "y: " + res.y + "differentx: " + differentx + "differenty: " + differenty);
                                PoseNetSample.Inst.audioSW = false;
                                 PoseNetSample.Inst.effectSW = true;
                                Debug.Log("식사 할 때 " + "x1: " + x1 + "differentx:" + differentx + "y1: " + y1 + "differenty:" + differenty);

                            }
                        }
                        
                }
                else//손이 안 보일 때
                {
                    if (res.part.ToString() == "RIGHT_WRIST")
                    {
                        //Debug.Log("res: " + res.confidence);
                        PoseNetSample.Inst.audioSW = true;
                        PoseNetSample.Inst.effectSW = false;
                        Debug.Log("인식 안됨");
         
                    }
                }*/
                
            }

            return results;
            
        }


        static void ApplySigmoid(float[,,] arr)
        {
            int rows = arr.GetLength(0); // y
            int cols = arr.GetLength(1); // x
            int parts = arr.GetLength(2);
            // simgoid to get score
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    for (int part = 0; part < parts; part++)
                    {
                        arr[y, x, part] = MathTF.Sigmoid(arr[y, x, part]);

                    }
                }
            }


        }

        struct ArgMaxResult
        {
            public int x;
            public int y;
            public float score;
        }

        static ArgMaxResult[] argMaxResults;
        static ArgMaxResult[] ArgMax2D(float[,,] scores)
        {
            int rows = scores.GetLength(0); //y
            int cols = scores.GetLength(1); //x
            int parts = scores.GetLength(2);

            // Init with minimum float
            if (argMaxResults == null)
            {
                argMaxResults = new ArgMaxResult[parts];
            }
            for (int i = 0; i < parts; i++)
            {
                argMaxResults[i].score = float.MinValue;
            }

            // ArgMax
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    for (int part = 0; part < parts; part++)
                    {
                        float current = scores[y, x, part];
                        if (current > argMaxResults[part].score)
                        {
                            argMaxResults[part] = new ArgMaxResult()
                            {
                                x = x,
                                y = y,
                                score = current,

                            };
                        }
                    }
                }
            }

            return argMaxResults;
        }


    }
}