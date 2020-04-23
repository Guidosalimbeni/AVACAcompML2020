using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{

    public bool CollisionWithOtherItemFoundForAIReward { get; set; }
    public bool CollisionWithOtherBoundsFrames { get; set; }

    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;

    

    private void Awake()
    {
        calculateCollisionDistanceVisualUnity = FindObjectOfType<CalculateCollisionDistanceVisualUnity>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<TagMeElementOfComposition>() != null)
        {
            calculateCollisionDistanceVisualUnity.FoundCollisionOfCompositionalElements(); 
            CollisionWithOtherItemFoundForAIReward = true; 
        }

        if (other.transform.GetComponent<TagMeFrameBounds>() != null)
        {
            calculateCollisionDistanceVisualUnity.FoundCollisionOfElementsINCompositionWithFrameBounds(); 
            CollisionWithOtherBoundsFrames= true; 
        }


    }

    private void OnTriggerExit (Collider other)
    {
        if (other.transform.GetComponent<TagMeElementOfComposition>() != null)
        {
            
            CollisionWithOtherItemFoundForAIReward = false;
            CollisionWithOtherBoundsFrames = false;
        }
    }

}
