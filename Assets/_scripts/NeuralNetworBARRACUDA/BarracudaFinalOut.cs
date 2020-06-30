using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using System;

public class BarracudaFinalOut : MonoBehaviour
{

    public bool USE_mean_not_NN = true;
    public NNModel ModelFinalOut;
    public event Action<float> OnScoreFinalOutChanged;
    public string modelName;
    private IWorker worker;
    private float scoreFinalOut;
    private ScoreCalculator scoreCalculator;
    private Model model;
    public bool useONX = false;

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<ScoreCalculator>();

        if (useONX)
        {
            model = ModelLoader.Load(ModelFinalOut);
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
        //worker = WorkerFactory.CreateWorker(model);
        if (model != null)
            worker = WorkerFactory.CreateWorker(model);
    }

    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void BarracudaCallTOCalculateFinalOutScore()
    {
        MakePredictionFinalOut();
    }

    private void MakePredictionFinalOut()
    {
        
        
        float i0 = scoreCalculator.scoreMobileNet;
        float i1 = scoreCalculator.scoreNNFrontTop;
        float i2 = scoreCalculator.scoreAllscorefeatures;

        if (USE_mean_not_NN)
        {
            scoreFinalOut = (i0 + i1 + i2) / 3;
        }

        else
        {
            float[] scores = new float[] { i0, i1, i2 };

            Tensor tensor = new Tensor(1, 3, scores);// batch of 1 dimensional data, scores initialized: batchCount x {elementCount}

            worker.Execute(tensor);
            //Tensor outputMobileNet = worker.Fetch();

            var O = worker.PeekOutput();

            //Debug.Log("this is the ouput of the OpenCV features Barrauda    " + O[0, 0, 0, 0]);
            //Debug.Log("this is the ouput of the Final Output Barrauda    " + O[0]);

            scoreFinalOut = O[0];

            tensor.Dispose();
        }
        

        if (OnScoreFinalOutChanged != null)
        {
            OnScoreFinalOutChanged(scoreFinalOut);
        }

        //O.Dispose();
        


    }
}
