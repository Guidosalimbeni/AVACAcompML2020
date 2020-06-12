using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemaining : MonoBehaviour
{
    public Image progressbar;
    
    AI_Calculator_score AI_Calculator_score;
    private int maxFrames;

    private void Start()
    {

        AI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
        AI_Calculator_score.OnFramesCountChanged += Handle_OnFramesCountChanged;
        maxFrames = AI_Calculator_score.maxFramesForRound;
    }

    private void Handle_OnFramesCountChanged(int frames)
    {
        
        
        if (maxFrames > 0)
        {
            
            float progress = (float)frames / (float)maxFrames;
            progressbar.fillAmount = 1 - progress;
            Debug.Log(progress);
        }
        
    }

    private void OnDisable()
    {
        AI_Calculator_score.OnFramesCountChanged -= Handle_OnFramesCountChanged;
    }
}
