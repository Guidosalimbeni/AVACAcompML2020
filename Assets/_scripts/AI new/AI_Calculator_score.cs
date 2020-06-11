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

    public int steps = 6;
    public float target = 0.95f;
    private int frames = 0;
    public bool movetotarget = true;
    public bool inferenceMode = false;
    
   
    public bool buttonRunRobot { get; set; }

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

        if (inferenceMode == false)
        {
            frames++;

            if (frames % steps == 0)
            {
                openCVManager.CallForOpenCVCalculationUpdates(); // 1 pixel count
                gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
                barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
                barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
                barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
                barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
            }
        }

        else
        {
            
            //frames++;

            //if (frames % steps == 0)
            //{
            //    openCVManager.CallForOpenCVCalculationUpdates(); // 1
            //    gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
            //    barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
            //    barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
            //    barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
            //    barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
            //}


        }

    }
    //private void FixedUpdate()
    //{

    //    if (inferenceMode == false)
    //    {
    //        frames++;

    //        if (frames % steps == 0)
    //        {
    //            openCVManager.CallForOpenCVCalculationUpdates(); // 1
    //            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
    //            barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
    //            barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
    //            barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
    //            barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
    //        }
    //    }

    //    else
    //    {
    //        if (buttonRunRobot)
    //        {
    //            frames++;

    //            if (frames % steps == 0)
    //            {
    //                openCVManager.CallForOpenCVCalculationUpdates(); // 1
    //                gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
    //                barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
    //                barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
    //                barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
    //                barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
    //            }

    //        }
    //    }

    //}

}
