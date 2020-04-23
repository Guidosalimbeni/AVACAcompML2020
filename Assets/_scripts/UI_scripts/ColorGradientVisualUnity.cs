using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ColorGradientVisualUnity : MonoBehaviour
{
    public RawImage VisualUnityUI;
    public Text text;
    private Color lerpedColor = Color.white;
    private float VisualUnityScore;
    private GameVisualManager gamemanagerNotOpenCV;

    private void Awake()
    {
        //VisualUnityUI = VisualUnityUI.GetComponent<RawImage>();
        gamemanagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        gamemanagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
    }

    private void Handle_OnScoreUnityVisualChanged(float VisualUnityScore)
    {
        UpdateBalancePixelsUI(VisualUnityScore);
        text.text = VisualUnityScore.ToString();
    }

    public void UpdateBalancePixelsUI(float score)
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
        VisualUnityUI.color = lerpedColor;

    }
}
