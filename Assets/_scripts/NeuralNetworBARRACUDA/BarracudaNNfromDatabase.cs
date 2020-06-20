using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
//using OpenCVForUnity.CoreModule;
//using OpenCVForUnity.UnityUtils;
using System;

public class BarracudaNNfromDatabase : MonoBehaviour
{

    /*
    THIS IS THE BARRACUDA VERSION of the previous TF version:

    This is the neural network that looks at the fron and top view . the training is using the data collected as blob in the mysql.salimbeni
    the data has labels good or bad using the button on the UI 
    traning happens offline so that new model needs to be updated offline by downloading the data from sql and run training
    
    */

    public event Action<float> OnScorescoreNNFrontTopChanged;
    public NNModel modelSourceNNFrontTOP;
    public RenderTexture camRenderFront; 
    public RenderTexture camRenderTop;
    public GameObject plane;
    public string modelName;

    private IWorker worker;
    private float scoreNNFrontTop;
    Model model;
    public bool useONX = false;

    private void Awake()
    {
        if (useONX)
        {
            model = ModelLoader.Load(modelSourceNNFrontTOP);
        }
        //
        else
        {
            model = ModelLoader.LoadFromStreamingAssets(modelName + ".onnx");
        }

    }
    private void Start()
    {

        //worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        if (model != null)
            worker = WorkerFactory.CreateWorker(model);
    }

    //call from leantouch and population manager one during breeding and one for last move
    //also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateBarracudaNNFrontTopcore()
    {
        MakePrecitionFrontTopConcatanedCamera();
    }

    public void MakePrecitionFrontTopConcatanedCamera()
    {
        //Texture2D m_TextureFront = ToTexture2D(camRenderFront);
        //Texture2D m_TextureTop = ToTexture2D(camRenderTop);
        //Texture2D TextureConcatanate = Concat2Texture2D(m_TextureFront, m_TextureTop, 40,20);

        Texture2D TextureConcatanate = Concat2Texture2D(camRenderFront, camRenderTop, 40, 20);
        Material surface = plane.GetComponent<MeshRenderer>().material;
        surface.SetTexture("_MainTex", TextureConcatanate);
        UseTexture(TextureConcatanate);

    }

    private void UseTexture(Texture2D input)
    {
        Tensor tensor = new Tensor(input);
        worker.Execute(tensor);
        //Tensor outputMobileNet = worker.Fetch();

        var O = worker.PeekOutput();
        //Debug.Log("this is the ouput of the TOP FRONT Barrauda    " + O[0, 0, 0, 1]);

        scoreNNFrontTop = O[0, 0, 0, 1];

        if (OnScorescoreNNFrontTopChanged != null)
        {
            OnScorescoreNNFrontTopChanged(scoreNNFrontTop);
        }

        //O.Dispose();
        tensor.Dispose();
    }

  

    private Texture2D Concat2Texture2D(RenderTexture front_Tex, RenderTexture top_Tex, int W, int H)
    {

        Texture2D finaltex = new Texture2D(W, H, TextureFormat.RGB24, false);

        Texture2D tex_front = new Texture2D(W/2, H, TextureFormat.RGB24, false);
        RenderTexture.active = front_Tex;
        tex_front.ReadPixels(new UnityEngine.Rect(0, 0, W/2 , H), 0, 0);
        tex_front.Apply();

        for (int x = 0; x < W/2; x++)
        {

            for (int y = 0; y < H; y++)
            {
                Color newColor = tex_front.GetPixel(x, y);

                
                finaltex.SetPixel(x, y, newColor);
            }
        }

        Texture2D tex_top = new Texture2D(W / 2, H, TextureFormat.RGB24, false);
        RenderTexture.active = top_Tex;
        tex_top.ReadPixels(new UnityEngine.Rect(0, 0, W / 2, H), 0, 0);
        tex_top.Apply();

        for (int x = W / 2; x < W; x++)
        {

            for (int y = 0; y < H; y++)
            {
                Color newColor = tex_top.GetPixel(x, y);


                finaltex.SetPixel(x, y, newColor);
            }
        }

        finaltex.Apply();

        return finaltex;
    }


    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    //private Texture2D Concat2Texture2D(Texture2D front, Texture2D top)
    //{
    //    // https://stackoverflow.com/questions/42539257/unity2d-combine-sprites
    //    // https://stackoverflow.com/questions/20078875/merging-cvmat-horizontally

    //    Texture2D TextureConcatanate = new Texture2D(front.width * 2, front.height, TextureFormat.RGB24, false);
    //    Mat imgMatTextureConcatanate = new Mat(front.height, front.width * 2,  CvType.CV_8UC3);
    //    Utils.texture2DToMat(TextureConcatanate, imgMatTextureConcatanate);
    //    Mat imgMatFront = new Mat(front.height, front.width, CvType.CV_8UC3);
    //    Utils.texture2DToMat(front, imgMatFront);
    //    Mat imgMatTop = new Mat(top.height, top.width, CvType.CV_8UC3);
    //    Utils.texture2DToMat(top, imgMatTop);

    //    List<Mat> ListToConcat = new List<Mat>();
    //    ListToConcat.Add(imgMatFront);
    //    ListToConcat.Add(imgMatTop);

    //    Core.hconcat(ListToConcat, imgMatTextureConcatanate);

    //    Utils.matToTexture2D(imgMatTextureConcatanate, TextureConcatanate);
    //    ListToConcat.Clear();

    //    return TextureConcatanate;

    //}
}
