using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAIRobotOnSelected : MonoBehaviour
{
    private AI_Calculator_score AI_Calculator_score;

    private void Awake()
    {
        AI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
    }
    public void InferenceOnSelected()
    {
        TagMeElementOfComposition[] items = FindObjectsOfType<TagMeElementOfComposition>();

        for (int i = 0; i < items.Length; i++)
        {
            LeanSelectable lean = items[i].GetComponent<LeanSelectable>();
            if (lean.IsSelected)
            {
                Comp_agent_float agent = items[i].GetComponent<Comp_agent_float>();
                agent.agentActive = !agent.agentActive;
                agent.enabled = !agent.enabled;
                AI_Calculator_score.buttonRunRobot = !AI_Calculator_score.buttonRunRobot;
            }
        }
    }
    public void StopInferenceOnSelected()
    {
        TagMeElementOfComposition[] items = FindObjectsOfType<TagMeElementOfComposition>();

        for (int i = 0; i < items.Length; i++)
        {
            LeanSelectable lean = items[i].GetComponent<LeanSelectable>();
            if (lean.IsSelected)
            {
                Comp_agent_float agent = items[i].GetComponent<Comp_agent_float>();
                agent.agentActive = false;

            }
        }
    }
}
