using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using System;

public class BarracudaOpenCvFeature : MonoBehaviour
{
    public NNModel ModelOpenCVFeatures;
    public event Action<float> OnScoreAllscoresfeatures;

    private IWorker worker;
    private ScoreCalculator scoreCalculator;
    private float ScoreAllscoreFeatures;

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }

    private void Start()
    {
        var model = ModelLoader.Load(ModelOpenCVFeatures);
        worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.ComputePrecompiled, model);
    }

    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void BarracudaCallTOCalculateOpencvFeaturesScore()
    {
        MakePredictionOpencvFeatures();
    }

    private void MakePredictionOpencvFeatures()
    {
        float i0 = scoreCalculator.visualScoreBalancePixelsCount;
        float i1 = scoreCalculator.scoreUnityVisual;
        float i2 = scoreCalculator.scoreLawOfLever;
        float i3 = scoreCalculator.scoreIsolationBalance;

        float[] scores = new float[] { i0, i1, i2, i3 };

        Tensor tensor = new Tensor(1, 4, scores);// batch of 1 dimensional data, scores initialized: batchCount x {elementCount}

        worker.Execute(tensor);
        //Tensor outputMobileNet = worker.Fetch();

        var O = worker.Peek();

        //Debug.Log("this is the ouput of the OpenCV features Barrauda    " + O[0, 0, 0, 0]);
        //Debug.Log("this is the ouput of the OpenCV features Barrauda    " + O[0]);

        ScoreAllscoreFeatures = O[0];

        if (OnScoreAllscoresfeatures != null)
        {
            OnScoreAllscoresfeatures(ScoreAllscoreFeatures);
        }

        O.Dispose();
    }
}
