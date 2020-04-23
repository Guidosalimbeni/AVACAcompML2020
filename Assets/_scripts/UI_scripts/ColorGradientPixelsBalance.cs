using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientPixelsBalance : MonoBehaviour
{
    public RawImage balancePixelsUI;
    public Text text;
    Color lerpedColor = Color.white;
    private OpenCVManager openCvManager;

    private void Awake()
    {
        balancePixelsUI = balancePixelsUI.GetComponent<RawImage>();
        openCvManager = FindObjectOfType<OpenCVManager>();
    }

    private void Start()
    {
        openCvManager.OnPixelsCountBalanceChanged += HandlePixelsCountBalanceChanged;
    }

    private void HandlePixelsCountBalanceChanged(float score)
    {
        UpdateBalancePixelsUI(score);
        if (text != null)
            text.text = score.ToString();
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
        balancePixelsUI.color = lerpedColor;

    }

    private void OnDisable()
    {
        openCvManager.OnPixelsCountBalanceChanged -= HandlePixelsCountBalanceChanged;
    }

}
