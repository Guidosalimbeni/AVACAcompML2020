using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAIRobotOnSelected : MonoBehaviour
{
    private int countClic = 0;
    public void InferenceOnSelected()
    {
        TagMeElementOfComposition[] items = FindObjectsOfType<TagMeElementOfComposition>();

        for (int i = 0; i < items.Length; i++)
        {
            LeanSelectable lean = items[i].GetComponent<LeanSelectable>();
            if (lean.IsSelected)
            {
                Comp_agent_float agent = items[i].GetComponent<Comp_agent_float>();
                agent.agentActive = true;

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
