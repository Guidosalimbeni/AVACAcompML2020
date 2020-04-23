using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barracuda;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
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

    private IWorker worker;
    private float scoreNNFrontTop;

    private void Start()
    {
        var model = ModelLoader.Load(modelSourceNNFrontTOP);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    //call from leantouch and population manager one during breeding and one for last move
    //also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateBarracudaNNFrontTopcore()
    {
        MakePrecitionNNDatabase();
    }

    public void MakePrecitionNNDatabase()
    {
        Texture2D m_TextureFront = ToTexture2D(camRenderFront);
        Texture2D m_TextureTop = ToTexture2D(camRenderTop);
        Texture2D TextureConcatanate = Concat2Texture2D(m_TextureFront, m_TextureTop);

        UseTexture(TextureConcatanate);

    }

    private void UseTexture(Texture2D input)
    {
        Tensor tensor = new Tensor(input);
        worker.Execute(tensor);
        //Tensor outputMobileNet = worker.Fetch();

        var O = worker.Peek();
        //Debug.Log("this is the ouput of the TOP FRONT Barrauda    " + O[0, 0, 0, 1]);

        scoreNNFrontTop = O[0, 0, 0, 1];

        if (OnScorescoreNNFrontTopChanged != null)
        {
            OnScorescoreNNFrontTopChanged(scoreNNFrontTop);
        }

        O.Dispose();
        //worker.Dispose();
    }


    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private Texture2D Concat2Texture2D(Texture2D front, Texture2D top)
    {
        // https://stackoverflow.com/questions/42539257/unity2d-combine-sprites
        // https://stackoverflow.com/questions/20078875/merging-cvmat-horizontally

        Texture2D TextureConcatanate = new Texture2D(front.width * 2, front.height, TextureFormat.RGB24, false);
        Mat imgMatTextureConcatanate = new Mat(front.height, front.width * 2,  CvType.CV_8UC3);
        Utils.texture2DToMat(TextureConcatanate, imgMatTextureConcatanate);
        Mat imgMatFront = new Mat(front.height, front.width, CvType.CV_8UC3);
        Utils.texture2DToMat(front, imgMatFront);
        Mat imgMatTop = new Mat(top.height, top.width, CvType.CV_8UC3);
        Utils.texture2DToMat(top, imgMatTop);

        List<Mat> ListToConcat = new List<Mat>();
        ListToConcat.Add(imgMatFront);
        ListToConcat.Add(imgMatTop);

        Core.hconcat(ListToConcat, imgMatTextureConcatanate);

        Utils.matToTexture2D(imgMatTextureConcatanate, TextureConcatanate);
        ListToConcat.Clear();

        return TextureConcatanate;

    }
}
