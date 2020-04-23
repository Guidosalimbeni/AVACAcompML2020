using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CalculateIsolationBalance : MonoBehaviour
{
    /// <summary>
    ///  this is for calculate the balance as a perceived weight semantic of the object apporximated from the volume
    ///  the isolation is a factor that increases the perceived weight of ab object 
    ///  so the balance is given by the sum of weight on one side compared to the other
    ///  but if one element is isolated its weight is higher 
    /// </summary>

    public float ScoreIsolationBalance { get; set; }

    private List<GameObject> ElementsOnTheRight;
    private List<GameObject> ElementsOnTheLeft;
    private List<float> Distances; 
    private GamePopulationController gamePopulationController;

    private void Awake()
    {
        gamePopulationController = FindObjectOfType<GamePopulationController>();
    }

    private void Start()
    {
        ElementsOnTheRight = new List<GameObject> ();
        ElementsOnTheLeft = new List<GameObject>();
        Distances = new List<float>();
    }

    public float CalculateScoreIsolationBalance()
    {

        float totalWeightLeft = 0.0f;
        float totalWeightRight = 0.0f;
        double ratio;

        foreach (var Element in gamePopulationController.ElementsCompositions)
        {
            if (Element.transform.position.x < 0.0f)
            {
                ElementsOnTheLeft.Add(Element);
                totalWeightLeft += Element.GetComponent<CalculateVolumeOfElementComp>().GetVolumeOftheItem();
            }
            if (Element.transform.position.x > 0.0f)
            {
                ElementsOnTheRight.Add(Element);
                totalWeightRight += Element.GetComponent<CalculateVolumeOfElementComp>().GetVolumeOftheItem();
            }

        }

        float TotalSceneWeight = totalWeightRight + totalWeightLeft;

        // this is for isolation on the left
        if (ElementsOnTheLeft.Count == 1 && ElementsOnTheRight.Count > 1)
        {
            float closestDistance = 0.0f;

            foreach (var Item in ElementsOnTheRight)
            {
                float DistanceToOther = Vector3.Distance(Item.transform.position, ElementsOnTheLeft[0].transform.position);
                Distances.Add(DistanceToOther);
            }

            Distances.Sort();
            closestDistance = Distances[0];
            totalWeightLeft += totalWeightLeft * closestDistance;
            TotalSceneWeight += totalWeightLeft;
            ratio = Mathf.Abs(totalWeightRight - totalWeightLeft) / TotalSceneWeight;
            ScoreIsolationBalance = 1.0f - (float)ratio;
        }

        // this is for isolation on the Right
        if (ElementsOnTheRight.Count == 1 && ElementsOnTheLeft.Count > 1)
        {
            float closestDistance = 0.0f;

            foreach (var Item in ElementsOnTheLeft)
            {
                float DistanceToOther = Vector3.Distance(Item.transform.position, ElementsOnTheRight[0].transform.position);
                Distances.Add(DistanceToOther);
            }

            Distances.Sort();
            closestDistance = Distances[0];
            totalWeightRight += totalWeightRight * closestDistance;
            TotalSceneWeight += totalWeightRight;
            ratio = Mathf.Abs(totalWeightRight - totalWeightLeft) / TotalSceneWeight;
            ScoreIsolationBalance = 1.0f - (float)ratio;
        }

        else
        {
            ratio = Mathf.Abs(totalWeightRight - totalWeightLeft) / TotalSceneWeight;
            ScoreIsolationBalance = 1.0f - (float)ratio;
        }

        ElementsOnTheLeft.Clear();
        ElementsOnTheRight.Clear();
        Distances.Clear();
        
        return ScoreIsolationBalance; 
    }
}
