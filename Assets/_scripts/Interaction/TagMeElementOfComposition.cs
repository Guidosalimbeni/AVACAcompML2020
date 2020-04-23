using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagMeElementOfComposition : MonoBehaviour, IElementOfComposition
{
    public int ElementOfCompositionID;

    public GameObject TagMe()
    {
        return gameObject;
    }

}
