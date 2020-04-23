using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientLawOfLever : MonoBehaviour
{
    public RawImage LawOfLeverRawImage;
    public Text text;
    private Color lerpedColor = Color.white;
    private GameVisualManager gameVisualManager;

    private void Awake()
    {
        gameVisualManager = FindObjectOfType<GameVisualManager>();
        gameVisualManager.OnScoreLawOfLeverChanged += Handle_OnScoreLawOfLeverChanged;
    }

    private void Handle_OnScoreLawOfLeverChanged(float scorelawOfLever)
    {
        UpdateLawOfLeverPixelsUI(scorelawOfLever);
        if (text != null)
        {
            text.text = scorelawOfLever.ToString();
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
        LawOfLeverRawImage.color = lerpedColor;
    }

    private void OnDisable()
    {
        gameVisualManager.OnScoreLawOfLeverChanged -= Handle_OnScoreLawOfLeverChanged;
    }
}
