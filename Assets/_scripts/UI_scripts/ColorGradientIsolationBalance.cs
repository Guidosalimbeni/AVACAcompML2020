using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientIsolationBalance : MonoBehaviour
{
    public RawImage IsolationBalanceImg;
    public Text text;
    private Color lerpedColor = Color.white;
    private GameVisualManager gameVisualManager;

    private void Awake()
    {
        gameVisualManager = FindObjectOfType<GameVisualManager>();
        gameVisualManager.OnScoreIsolationBalanceChanged += Handle_OnScoreIsolationBalanceChanged;
    }

    private void Handle_OnScoreIsolationBalanceChanged(float scoreIsolationbalancePassed)
    {
        UpdateLawOfLeverPixelsUI(scoreIsolationbalancePassed);
        if (text != null)
        {
            text.text = scoreIsolationbalancePassed.ToString();
        }
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
        IsolationBalanceImg.color = lerpedColor;
    }

    private void OnDisable()
    {
        gameVisualManager.OnScoreIsolationBalanceChanged -= Handle_OnScoreIsolationBalanceChanged;
    }
}
