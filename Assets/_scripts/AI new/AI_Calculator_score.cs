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
    private Comp_agent_float_move_child[] agents;
    private int maxFramesForRound;

    public int steps = 6;
    public float target = 0.95f;
    private int frames = 0;
    public bool movetotarget = true;
    public bool inferenceMode = false;
    public bool AIturn = false;

    public event Action<int> OnFramesCountChanged;

    public bool buttonRunRobot { get; set; }

    private void Awake()
    {
        openCVManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        barracudaCNNModel = FindObjectOfType<BarracudaCNNModel>();
        barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();
        barracudaOpenCvFeature = FindObjectOfType<BarracudaOpenCvFeature>();
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        agents = FindObjectsOfType<Comp_agent_float_move_child>();
        
        for (int i = 0; i < agents.Length; i++)
        {
            maxFramesForRound = agents[i].MaxStep; // they are all the same so taking last one
        }
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

        if (inferenceMode == true)
        {
            if (AIturn == true)
            {
                for (int i = 0; i < agents.Length; i++)
                {
                    agents[i].enabled = true;
                }

                frames++;

                if (frames % steps == 0)
                {
                    openCVManager.CallForOpenCVCalculationUpdates(); // 1
                    gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
                    barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
                    barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
                    barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
                    barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
                }
            }

            if(AIturn == false)
            {
                frames++;
                // scoring it is trigger by event on finger up
                
                for (int i = 0; i < agents.Length; i++)
                {
                    agents[i].enabled = false;
                }


            }
            


        }

        if (OnFramesCountChanged != null)
        {
            OnFramesCountChanged(frames);
        }

        if (frames > maxFramesForRound)
        {
            // start couroutine to display score and ask to continue if round AI completed
            AIturn = !AIturn;
            frames = 0;

        }

        

    }
    
    // the idea is clock from game manager establish the turns... the turns last by academy steps .. before running again
    // it shows the score of player ... then run AI ... when AI finished .. it shows screen with who winning ...
    // thank you for helping the project.. do you fancy a new round? or quit.. 
    // check data sent to database that are saved correctly with correct score ets..
    // not sure if it is better to stop move or deactivate agent script... I can use the frames count here for both clock 
    // and agent deativate.. by looking at agent max steps.. so it matches if I change it.. reference to max step..

}
