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
    public TextMeshProUGUI AItextPoint;
    public TextMeshProUGUI PlayertextPoint;
    public Animator playerWinAnim;
    public Animator AiWinAnim;

    private BarracudaFinalOut barracudaFinalOut;
    private int ScorePointAI = 0;
    private int ScorePointPlayer = 0;

    
    private AudioSource point;


    private void Awake()
    {
        AItextPoint.text = string.Format("{0}", 0);
        PlayertextPoint.text = string.Format("{0}", 0);
        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
        point = GetComponent<AudioSource>();
        
    }

    

    public void ShowResult(float playerScore, float AIScore)
    {
        ScorePanel.SetActive(true);
        YOUSCORED.text = string.Format("YOU {0} %", playerScore.ToString("F2"));
        AISCORED.text = string.Format("AI {0} %", AIScore.ToString("F2"));
    }

    public void UpdatePointScores(float playerScore, float AIScore)
    {
        if (playerScore > AIScore)
        {
            ScorePointPlayer++;
            playerWinAnim.SetTrigger("playerWin");
            point.Play();

        }

        else
        {
            ScorePointAI++;
            AiWinAnim.SetTrigger("AiWin");
            point.Play();
        }

        AItextPoint.text = string.Format("{0}", ScorePointAI);
        PlayertextPoint.text = string.Format("{0}", ScorePointPlayer);

    }


    public void CloseGameResultPanel()
    {
        ScorePanel.SetActive(false);
    }

    private void Handle_OnScoreFinalOutChanged(float scoreFinalOutPassed)
    {
        float  scoreFinalOut = scoreFinalOutPassed;
        //float score = 0.0f;
        //float thresh = 0.99f;
        //if (scoreFinalOut < thresh)
        //{
        //    score = map(scoreFinalOut, 0, thresh, 0, 0.85f);
        //}
        //else
        //{
        //    score = map(scoreFinalOut, thresh, 1, 0.85f, 1f);
        //}

        progressbar.fillAmount = scoreFinalOut;
        text.text = string.Format("{0}%", (scoreFinalOut * 100).ToString("F2"));
        
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
