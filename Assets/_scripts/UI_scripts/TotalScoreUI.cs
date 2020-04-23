using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;
    // private InferenceCompositionML inferenceCompositionML;
    // private InferenceNNfomDATABASE inferenceNNfomDATABASE;
    // private InferenceScoreFeatures inferenceScoreFeatures;
    // private InferenceFinalOut inferenceFinalOut;

    private BarracudaCNNModel barracudaCNNModel;
    private BarracudaNNfromDatabase barracudaNNfromDatabase;
    private BarracudaOpenCvFeature barracudaOpenCvFeature;
    private BarracudaFinalOut barracudaFinalOut;

    public bool SeparatedTotalSum = false;
    public bool TotalOfthe3Net = false;
    public bool tot = true;
    public float TOTALSCOREDebugging;

    float a;
    float b;
    float c;
    float d;
    float e;
    float f;
    float g;
    float h;
    float i;

    private void Awake()
    {

        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();

        // inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        // inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
        // inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();
        // inferenceFinalOut = FindObjectOfType<InferenceFinalOut>();

        barracudaCNNModel = FindObjectOfType<BarracudaCNNModel>();
        barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();
        barracudaOpenCvFeature = FindObjectOfType<BarracudaOpenCvFeature>();
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged += Handlex_OnScoreLawOfLeverChanged;
        gameManagerNotOpenCV.OnScoreIsolationBalanceChanged += Handle_OnScoreIsolationBalanceChanged;

        // inferenceCompositionML.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
        // inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
        // inferenceScoreFeatures.OnScoreAllscoresfeatures += Handle_OnScoreAllscoresfeatures;
        // inferenceFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;

        barracudaCNNModel.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
        barracudaNNfromDatabase.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
        barracudaOpenCvFeature.OnScoreAllscoresfeatures += Handle_OnScoreAllscoresfeatures;
        barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;


    }

    private void Handle_OnScoreFinalOutChanged(float obj)
    {
        i = obj;
    }

    private void Handle_OnScoreAllscoresfeatures(float obj)
    {
        h = obj;
    }

    private void Handle_OnScoreIsolationBalanceChanged(float obj)
    {
        g = obj;
    }

    private void Handle_OnScorescoreNNFrontTopChanged(float obj)
    {
        f = obj;
    }

    private void Handle_OnScorescoreMobileNetChanged(float obj)
    {
        e = obj;
    }

    private void Handlex_OnScoreLawOfLeverChanged(float obj)
    {
        d = obj;
    }

    private void Update()
    {
        if (SeparatedTotalSum)
        {
            TOTALSCOREDebugging = a + b + c + d + e + f + g;
        }

        if (TotalOfthe3Net)
        {
            TOTALSCOREDebugging = e + f + h;
        }

        if (tot)
        {
            TOTALSCOREDebugging = i;
        }
        
    }

    private void Handle_OnScoreUnityVisualChanged(float obj)
    {
        a = obj;
    }

    private void Handle_OnScoreBoundsBalanceChanged(float obj)
    {
        b = obj;
    }

    private void HandleOnPixelsCountBalanceChanged(float obj)
    {
        c = obj;
    }

    private void OnDisable()
    {
        openCvManager.OnPixelsCountBalanceChanged -= HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged -= Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged -= Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged -= Handlex_OnScoreLawOfLeverChanged;
        gameManagerNotOpenCV.OnScoreIsolationBalanceChanged -= Handle_OnScoreIsolationBalanceChanged;

        // inferenceCompositionML.OnScorescoreMobileNetChanged -= Handle_OnScorescoreMobileNetChanged;
        // inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
        // inferenceScoreFeatures.OnScoreAllscoresfeatures -= Handle_OnScoreAllscoresfeatures;
        // inferenceFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;

        barracudaCNNModel.OnScorescoreMobileNetChanged -= Handle_OnScorescoreMobileNetChanged;
        barracudaNNfromDatabase.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
        barracudaOpenCvFeature.OnScoreAllscoresfeatures -= Handle_OnScoreAllscoresfeatures;
        barracudaFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;
    }

}
