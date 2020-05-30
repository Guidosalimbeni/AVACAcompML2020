using UnityEngine;
using System.Collections.Generic;
using System;

public class AI_Calculator_score : MonoBehaviour
{
    private OpenCVManager openCVManager;
    private GameVisualManager gameManagerNotOpenCV;
    private BarracudaCNNModel barracudaCNNModel;
    private BarracudaNNfromDatabase barracudaNNfromDatabase;
    private BarracudaOpenCvFeature barracudaOpenCvFeature;
    private BarracudaFinalOut barracudaFinalOut;

    public int steps = 20;
    private int frames = 0;
    
    private void Awake()
    {
        openCVManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        barracudaCNNModel = FindObjectOfType<BarracudaCNNModel>();
        barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();
        barracudaOpenCvFeature = FindObjectOfType<BarracudaOpenCvFeature>();
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
    }
    private void FixedUpdate()
    {
        frames++;
        if (frames % steps == 0)
        {
            openCVManager.CallForOpenCVCalculationUpdates();
            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
            barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
            barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
            barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
            barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
        }
        
    }
    
}
