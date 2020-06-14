using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;
using System.Security.Permissions;

public class ScoreGameAI : MonoBehaviour
{
    public Image progressbar;
    public Text text;
    public TextMeshProUGUI YOUSCORED;
    public TextMeshProUGUI AISCORED;
    public GameObject ScorePanel;

    private BarracudaFinalOut barracudaFinalOut;
    private AI_Calculator_score AI_Calculator_score;
    

    private void Awake()
    {
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        AI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
        barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
    }

    

    public void ShowResult(float playerScore, float AIScore)
    {
        ScorePanel.SetActive(true);
        YOUSCORED.text = string.Format("YOU SCORED {0}", playerScore);
        AISCORED.text = string.Format("AI SCORED {0}", AIScore);


    }

    public void CloseGameResultPanel()
    {
        ScorePanel.SetActive(false);
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
