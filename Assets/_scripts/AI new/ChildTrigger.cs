using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour
{

    LeanTranslateAlong leantranslate;
    LeanPlane leanplane;

    void OnTriggerEnter(Collider c)
    {
        gameObject.GetComponentInParent<Comp_agent_float_move_child>().PullTrigger(c);
    }

    private void Awake()
    {
        leantranslate = GetComponent<LeanTranslateAlong>();
        leanplane = FindObjectOfType<LeanPlane>();
    }

    private void OnEnable()
    {
        leantranslate.ScreenDepth.Object = leanplane;
    }

}
