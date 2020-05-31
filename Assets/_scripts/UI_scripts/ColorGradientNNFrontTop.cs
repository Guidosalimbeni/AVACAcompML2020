using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientNNFrontTop : MonoBehaviour
{
    
    public RawImage NN_front_top_score_RawImage;
    public Text text;
    private Color lerpedColor = Color.white;

    //private InferenceNNfomDATABASE inferenceNNfomDATABASE;
    private BarracudaNNfromDatabase barracudaNNfromDatabase;

    private void Awake()
    {
        //inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
        barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();

        //inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
        barracudaNNfromDatabase.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
    }

    private void Handle_OnScorescoreNNFrontTopChanged(float scoreNNFrontTop)
    {
        UpdateLawOfLeverPixelsUI(scoreNNFrontTop);
        if (text != null)
            text.text = scoreNNFrontTop.ToString("F2");
    }

    public void UpdateLawOfLeverPixelsUI(float score)
    {
        float weight = 1.0f;

        if (score < 0.90f)
        {
            weight = 0.8f;
        }

        if (score < 0.85f)
        {
            weight = 0.5f;
        }
        if (score < 0.5f)
        {
            weight = 0.2f;
        }

        lerpedColor = Color.Lerp(Color.black, Color.white, score * weight);
        NN_front_top_score_RawImage.color = lerpedColor;
    }

    private void OnDisable()
    {
        //inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
        barracudaNNfromDatabase.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
    }

}
