using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientBoundShapeBalance : MonoBehaviour
{
    public RawImage balanceBoundsShapesUI;

    private float visualScoreBalanceBoundsShapes;
    Color lerpedColor = Color.white;
    private AreasObjects areaobjects;

    private GameVisualManager gameManagerNotOpenCV;

    private void Awake()
    {
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += HandleOnScoreBoundsBalanceChanged;
        balanceBoundsShapesUI = balanceBoundsShapesUI.GetComponent<RawImage>();
    }

    private void HandleOnScoreBoundsBalanceChanged(float visualScoreBalanceBoundsShapes)
    {
        UpdateBalancePixelsUI(visualScoreBalanceBoundsShapes);
    }


    public void UpdateBalancePixelsUI(float score)
    {
        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }
    }
}
