using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWeb : MonoBehaviour
{
    public Image progressbar;
    public Text text;

    private BarracudaFinalOut barracudaFinalOut;


    private void Awake()
    {
        
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
    }

    private void Handle_OnScoreFinalOutChanged(float scoreFinalOutPassed)
    {
        float  scoreFinalOut = scoreFinalOutPassed;
        float score = 0.0f;
        float thresh = 0.99f;
        if (scoreFinalOut < thresh)
        {
            score = map(scoreFinalOut, 0, thresh, 0, 0.85f);
        }
        else
        {
            score = map(scoreFinalOut, thresh, 1, 0.85f, 1f);
        }

        progressbar.fillAmount = score;
        text.text = string.Format("{0}%", (score * 100).ToString("F2"));
        
    }

    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void OnDisable()
    {
        barracudaFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;
    }
}
