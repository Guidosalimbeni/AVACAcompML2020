using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientMobileNet : MonoBehaviour
{
    public RawImage MobileNetScore_RawImage;
    public Text text;
    private Color lerpedColor = Color.white;

    //private InferenceCompositionML inferenceCompositionML;

    private BarracudaCNNModel barracudaCNNModel;
    

    private void Awake()
    {
        //inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        barracudaCNNModel = FindObjectOfType<BarracudaCNNModel>();

        //inferenceCompositionML.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
        barracudaCNNModel.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
    }

    private void Handle_OnScorescoreMobileNetChanged(float scoreMobileNetPassed)
    {
        UpdateMobileScoreUI(scoreMobileNetPassed);
        if (text != null)
            text.text = scoreMobileNetPassed.ToString();
    }

    public void UpdateMobileScoreUI(float score)
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
        MobileNetScore_RawImage.color = lerpedColor;
    }

    private void OnDisable()
    {
        //inferenceCompositionML.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
        barracudaCNNModel.OnScorescoreMobileNetChanged -= Handle_OnScorescoreMobileNetChanged;
    }
}
