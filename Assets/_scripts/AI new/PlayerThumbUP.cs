using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThumbUP : MonoBehaviour
{
    private AI_Calculator_score aI_Calculator_Score;

    private void Awake()
    {
        aI_Calculator_Score = FindObjectOfType<AI_Calculator_score>();
    }

    public void ThumbUpPlayerAcceptComposition() 
    {
        aI_Calculator_Score.playerButtonOK = true; // not used

    }

}
