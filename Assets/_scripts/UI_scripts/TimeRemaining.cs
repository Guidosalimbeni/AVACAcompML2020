using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemaining : MonoBehaviour
{
    public Image progressbar;
    
    
    AI_Calculator_score AI_Calculator_score;
    private int maxFrames;
    //private int timeTutorial;

    private void Start()
    {

        AI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
        AI_Calculator_score.OnFramesCountChanged += Handle_OnFramesCountChanged;
        maxFrames = AI_Calculator_score.maxFramesForRound;
        //timeTutorial = AI_Calculator_score.timeTutorial;
    }

    private void Handle_OnFramesCountChanged(int frames)
    {
        if (maxFrames > 0)
        {
            float progress = map(frames, 0, maxFrames, 0, 1.0f);
            progressbar.fillAmount = 1 - progress;
            
        }
        
    }

    private void OnDisable()
    {
        AI_Calculator_score.OnFramesCountChanged -= Handle_OnFramesCountChanged;
    }

    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
