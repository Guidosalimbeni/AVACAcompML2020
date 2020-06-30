using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private int ScorePointAI = 0;
    private int ScorePointPlayer = 0;

    private AudioSource point;

    private AI_Calculator_score aI_Calculator_Score;

    private void Awake()
    {
        AItextPoint.text = string.Format("{0}", 0);
        PlayertextPoint.text = string.Format("{0}", 0);
        aI_Calculator_Score = FindObjectOfType<AI_Calculator_score>();

        point = GetComponent<AudioSource>();


        aI_Calculator_Score.OnCurrentScoreChanged += Handle_OnCurrentScoreChanged;
    }

    private void Handle_OnCurrentScoreChanged(float currentScore)
    {
        float scoreFinalOut = currentScore;

        if (aI_Calculator_Score.inferenceMode == true)
        {
            progressbar.fillAmount = scoreFinalOut;
            text.text = string.Format("{0}%", (scoreFinalOut * 100).ToString("F2"));
        }
        
    }



    public void ShowResult(float playerScore, float AIScore)
    {
        Debug.Log(" I am here");
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


    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void OnDisable()
    {
        aI_Calculator_Score.OnCurrentScoreChanged -= Handle_OnCurrentScoreChanged;
    }
}
