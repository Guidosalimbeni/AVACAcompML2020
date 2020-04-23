using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolVisualClue : MonoBehaviour
{
    public Image Spiral;
    public Image RuleOfThird;

    public void TurnOnOffSpiral()
    {
        Spiral.enabled = !Spiral.enabled;
    }

    public void TurnOnOffRuleOfThird()
    {
        RuleOfThird.enabled = !RuleOfThird.enabled;
    }
}
