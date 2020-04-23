using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalculateLawOfLever : MonoBehaviour
{
    private GamePopulationController gamePopulationController;
    //private ColliderBounds colliderBounds;
    //private CalculateVolumeOfElementComp calculateVolumeOfElementComp;
    private float TotalVolumeLeft;
    private float TotalVolumeRight;
    private float averageDistanceLeftToFulcrum;
    private float averageDistanceRightToFulcrum;

    public float scoreLawOfLever { get; set; }

    private void Awake()
    {
        gamePopulationController = FindObjectOfType<GamePopulationController>();
    }

    public float CalculateLawOfLeverLeftRigth()
    {
        TotalVolumeLeft = 0.0f;
        TotalVolumeRight = 0.0f;
        List<float> leftX = new List<float>();
        List<float> RightX = new List<float>();

        foreach (var item in gamePopulationController.ElementsCompositions)
        {
            //Vector3 centerOfMass = item.GetComponent<ColliderBounds>().GetCenterOfMass(); // I can change with a locator at some point

            if (item.transform.position.x < 0)
            {
                float volumeOfItem = item.GetComponent<CalculateVolumeOfElementComp>().Volume;
                TotalVolumeLeft += volumeOfItem;
                leftX.Add(item.transform.position.x);

            }
            if (item.transform.position.x > 0)
            {
                float volumeOfItem = item.GetComponent<CalculateVolumeOfElementComp>().Volume;
                TotalVolumeRight += volumeOfItem;
                RightX.Add(item.transform.position.x);
            }

        }

        float meanLeftX = 0.0f;
        float tot = 0.0f;
        foreach (var x in leftX)
        {
            tot += x;
        }

        if (leftX.Count > 0.0f)
        {
            meanLeftX = tot / leftX.Count;
        }

        float meanRightX = 0.0f;
        float tot2 = 0.0f;
        foreach (var x in RightX)
        {
            tot2 += x;
        }
        if (RightX.Count > 0.0f)
        {
            meanRightX = tot2 / RightX.Count;
        }

        // since the fulcrum is at 0.0

        float distL = Mathf.Abs(meanLeftX);
        float distR = Mathf.Abs(meanRightX);
        float difference;
        double Force;
        float error;


        // https://en.wikipedia.org/wiki/Sigmoid_function#/media/File:Gjl-t(x).svg
        if (distL > 0.0f && distR > 0.0f)
        {
            difference = (TotalVolumeRight * distR) / distL;
            Force = Mathf.Abs(TotalVolumeLeft - difference);
            double cost = Math.Tanh( Force);

            error = (float)cost;
        }

        else
        {
            error = 1;
        }

        leftX.Clear();
        RightX.Clear();


        scoreLawOfLever = 1 - error;

        return scoreLawOfLever;
    }

    
}
