using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UI;
using Unity.MLAgents;
using System.Diagnostics.Eventing.Reader;
using Lean.Touch;

public class AI_Calculator_score : MonoBehaviour
{
    
    //public int maxFramesForRound { get; set; }

    public int maxFramesForRound = 600;

    
    public float target = 0.95f;
    public bool movetotarget = true;
    public bool inferenceMode = false;
    
    public GameObject you;
    public GameObject AI;
    public float PauseTimeForScore = 3.0f;
    

    public bool playerButtonOK { get; set; }
    private bool AIturn = false;

    private int steps = 6;
    private int frames = 0;
    private OpenCVManager openCVManager;
    private GameVisualManager gameManagerNotOpenCV;
    private BarracudaCNNModel barracudaCNNModel;
    private BarracudaNNfromDatabase barracudaNNfromDatabase;
    private BarracudaOpenCvFeature barracudaOpenCvFeature;
    private BarracudaFinalOut barracudaFinalOut;
    private Comp_agent_float_move_child[] agents;
    private float currentScore;
    private Image youColor;
    private Image AIColor;
    private float currentScoreAI;
    private float currentScorePLAYER;
    private ScoreGameAI ScoreGameAI;
    private LeanTouch leanTouch;
    private AudioSource robotSound;
    private SendToDatabase sendToDatabase;
    private bool WEBbuild;
    private ScoreCalculator scoreCalculator;


    public event Action<int> OnFramesCountChanged;

    public event Action<float> OnCurrentScoreChanged;

    public bool buttonRunRobot { get; set; }

    private void Awake()
    {
        ScoreGameAI = FindObjectOfType<ScoreGameAI>();
        openCVManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        barracudaCNNModel = FindObjectOfType<BarracudaCNNModel>();
        barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();
        barracudaOpenCvFeature = FindObjectOfType<BarracudaOpenCvFeature>();
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        agents = FindObjectsOfType<Comp_agent_float_move_child>();
        youColor = you.GetComponent<Image>();
        AIColor = AI.GetComponent<Image>();
        leanTouch = FindObjectOfType<LeanTouch>();
        robotSound = GetComponent<AudioSource>();
        sendToDatabase = FindObjectOfType<SendToDatabase>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();

        playerButtonOK = false;

        for (int i = 0; i < agents.Length; i++)
        {
            steps = agents[i].GetComponent<DecisionRequester>().DecisionPeriod; // they are all the same so taking last one
            WEBbuild = agents[i].WEBbuild;
        }

        if (WEBbuild == false)
        {
            barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
        }

        if (WEBbuild == true)
        {
            openCVManager.OnPixelsCountBalanceChanged += Handle_OnPixelsCountBalanceChanged;
        }
        

    }

    private void Handle_OnPixelsCountBalanceChanged(float score) // just to fire... this is a quick hack to to the web build
    {
        currentScore = (scoreCalculator.visualScoreBalancePixelsCount + scoreCalculator.scoreUnityVisual + scoreCalculator.scoreIsolationBalance + scoreCalculator.scoreLawOfLever) / 4;
        if (scoreCalculator.scoreUnityVisual == 0.0f)
        {
            currentScore = currentScore * 0.5f;
        }

        OnCurrentScoreChanged?.Invoke(currentScore);
    }

    private void Handle_OnScoreFinalOutChanged(float score)
    {
        currentScore = score;

        OnCurrentScoreChanged?.Invoke(currentScore);
    }

    private void FixedUpdate()
    {

        frames++;

        if (inferenceMode == false) // if training
        {
            if (frames % steps == 0)
            {
                CallToCalculateScores();
            }
        }

        if (inferenceMode == true)
        {
            PlayGameLogic();

        }

    }

    private void PlayGameLogic()
    {
        if (AIturn == false)
        {

            youColor.color = new Color(0, 200, 0);
            AIColor.color = new Color(256, 256, 256);

            // scoring it is triggered by event on finger up
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].enabled = false;
            }

            if (playerButtonOK == true) 
            {
                frames = maxFramesForRound;
                playerButtonOK = false;
            }

        }

        if (AIturn == true)
        {
            leanTouch.enabled = false;
            

            youColor.color = new Color(256, 256, 256);
            AIColor.color = new Color(0, 200, 0);

            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].enabled = true;
            }

            if (frames % steps == 0)
            {
                robotSound.Play();
                CallToCalculateScores();
            }

            if (currentScore >= target)
            {
                frames = maxFramesForRound;
            }

        }

        OnFramesCountChanged?.Invoke(frames);

        if (frames == maxFramesForRound)
        {

            robotSound.Stop();

            if (AIturn == true)
            {
                currentScoreAI = currentScore;

                sendToDatabase.PostDataForAI();

                StartCoroutine(PauseGame(PauseTimeForScore));
            }

            if (AIturn == false)
            {
                sendToDatabase.PostDataForPositiveJudge();
                currentScorePLAYER = currentScore;
                ShuffleItemPositionWithAgentEnable();

            }


            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].enabled = !agents[i].enabled;
            }

            
            AIturn = !AIturn;
            frames = 0;
            currentScore = 0.0f;
        }

    }

    private void ShuffleItemPositionWithAgentEnable()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            Vector3 spawnLocation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0f, UnityEngine.Random.Range(-1.0f, 1.0f));
            ChildTrigger item = agents[i].GetComponentInChildren<ChildTrigger>();
            item.transform.position = spawnLocation;
        }

        leanTouch.enabled = true;
    }


    private void CallToCalculateScores()
    {
        openCVManager.CallForOpenCVCalculationUpdates(); // 1 pixel count
        gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores(); // +3 = 4 of the barracyuda calculate opencv features score
        barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
        barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
        barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
        barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
    }

    private  IEnumerator PauseGame(float pauseTime) // need to disable lean touch as well--no se no come faccio a cliccare yes..?
    {
        ScoreGameAI.ShowResult(currentScorePLAYER, currentScoreAI);
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;

        ScoreGameAI.UpdatePointScores(currentScorePLAYER, currentScoreAI);
        ScoreGameAI.CloseGameResultPanel();

        ShuffleItemPositionWithAgentEnable();

    }

    private void OnDisable()
    {
        openCVManager.OnPixelsCountBalanceChanged -= Handle_OnPixelsCountBalanceChanged;
        barracudaFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;
    }




}
