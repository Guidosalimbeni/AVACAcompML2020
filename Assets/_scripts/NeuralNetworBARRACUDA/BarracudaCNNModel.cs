using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MLAgents;
using Unity.Barracuda;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using System;


public class BarracudaCNNModel : MonoBehaviour
{
    // https://github.com/Unity-Technologies/ml-agents/blob/master/UnitySDK/Assets/ML-Agents/Plugins/Barracuda.Core/Barracuda.md
    // https://enoxsoftware.com/opencvforunity/mat-basic-processing2/

    // public event Action<float> OnScorescoreMobileNetChanged;
    public event Action<float> OnScorescoreMobileNetChanged;
    public NNModel modelSource;
    public RenderTexture camRenderTexture; // this is 64 x 64 changed using custom... but need this to come as 240 x 180 and then convert to 64 64
    public int W = 64;
    public int H = 64;

    private IWorker worker;
    private Texture2D normTxt;
    private float ScoreFromBarracudaCNN;

    private void Start()
    {
        normTxt = new Texture2D(W, H, TextureFormat.RGB24, false);
        var model = ModelLoader.Load(modelSource);
        //worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        worker = WorkerFactory.CreateWorker(model);
    }

    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateBarracudaCNNScore()
    {
        // previously CallTOCalculateMobileNetScore
        Texture2D m_Texture = ToTexture2DAndResize(camRenderTexture, W, H);
        UseTexture(m_Texture);
    }

    
    private void UseTexture(Texture2D input)
    {
        Tensor tensor = new Tensor(input);
        worker.Execute(tensor);
        //Tensor outputMobileNet = worker.Fetch();

            // for (int h = 0; h < tensor.height; h++)
            // {
            //    for (int w = 0; w < tensor.width; w++)
            //    {
            //        for (int c = 0; c < 3; c++)
            //        {
            //            Debug.Log(tensor[0, h, w, c]);
            //        }
                        
            //    }
                    
            // }

        var O = worker.PeekOutput();
        //Debug.Log("this is the ouput of the CNN Barrauda    " + O[0,0,0,1]);
        //Debug.Log("this SHAPE    " + O[0]);
        
        ScoreFromBarracudaCNN = O[0,0,0,1];
 
        O.Dispose();
        

        if (OnScorescoreMobileNetChanged != null)
        {
            OnScorescoreMobileNetChanged(ScoreFromBarracudaCNN);
        }
    }

    private Texture2D ToTexture2DAndResize(RenderTexture rTex, int W, int H)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false); //RGBA32
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        Mat imgMat = new Mat(tex.height, tex.width, CvType.CV_8UC3); 
        Utils.texture2DToMat(tex, imgMat);

        Size scaleSize = new Size(W, H);
        Imgproc.resize(imgMat, imgMat, scaleSize, 0, 0, interpolation: Imgproc.INTER_AREA);
        
        Texture2D resizedImg = new Texture2D(W, H, TextureFormat.RGB24, false); 
        Utils.matToTexture2D(imgMat, resizedImg);

        return resizedImg;

    }


    // NOT USED since the values are already from 0 to 1
    private Texture2D NormalisedInputTexture(Texture2D texture)
    {
        
        //plane.GetComponent<Renderer>().material.mainTexture = normTxt;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color newColor;
                float r = texture.GetPixel(x,y).r * 255;
                float g = texture.GetPixel(x, y).g * 255;
                float b = texture.GetPixel(x, y).b * 255;
                
                newColor = new Color((r - 127.5f) / 127.5f, (g - 127.5f) / 127.5f, (b - 127.5f) / 127.5f);
                
                normTxt.SetPixel(x, y, newColor);
            }
        }
        normTxt.Apply();

        return normTxt;
    }

}
