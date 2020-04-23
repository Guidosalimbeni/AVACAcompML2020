using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barracuda;
using System;

public class BarracudaFinalOut : MonoBehaviour
{

    public NNModel ModelFinalOut;
    public event Action<float> OnScoreFinalOutChanged;

    private IWorker worker;
    private float scoreFinalOut;
    private ScoreCalculator scoreCalculator;

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }

    private void Start()
    {
        var model = ModelLoader.Load(ModelFinalOut);
        worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.ComputePrecompiled, model);
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

        float[] scores = new float[] { i0, i1, i2 };

        Tensor tensor = new Tensor(1, 3, scores);// batch of 1 dimensional data, scores initialized: batchCount x {elementCount}

        worker.Execute(tensor);
        //Tensor outputMobileNet = worker.Fetch();

        var O = worker.Peek();

        //Debug.Log("this is the ouput of the OpenCV features Barrauda    " + O[0, 0, 0, 0]);
        //Debug.Log("this is the ouput of the Final Output Barrauda    " + O[0]);

        scoreFinalOut = O[0];

        if (OnScoreFinalOutChanged != null)
        {
            OnScoreFinalOutChanged(scoreFinalOut);
        }

        O.Dispose();

    }
}
