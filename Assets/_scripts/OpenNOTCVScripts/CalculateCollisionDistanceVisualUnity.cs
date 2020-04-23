using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalculateCollisionDistanceVisualUnity : MonoBehaviour
{
    public bool drawRenderedLinesDebug;
    public float weight = 1.0f;

    private LineRenderer lineRenderer;
    private List<List<GameObject>> PairwaiseElementsOfComposition;
    private List<GameObject> pair;
    private List<GameObject> LineRendererObjectsList;
    private bool OverlappingColliders = false;

    public float VisualUnityScore { get; private set; }

    //private GameVisualManager gamemanager;
    private GamePopulationController gamePopulationController;

    private void Awake()
    {
        gamePopulationController = FindObjectOfType<GamePopulationController>();
    }

    private void Start()
    {
       

        PairwaiseElementsOfComposition = PairwaiseOperation(gamePopulationController.ElementsCompositions);

        if (drawRenderedLinesDebug == true)
        {
            LineRendererObjectsList = new List<GameObject>();
            for (int i = 0; i < PairwaiseElementsOfComposition.Count; i++)
            {
                GameObject ObjectForLineRenderer = new GameObject("GameObject_LineRenderer_" + i.ToString());
                lineRenderer = ObjectForLineRenderer.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.widthMultiplier = 0.02f;

                LineRendererObjectsList.Add(ObjectForLineRenderer);
            }
        }
        
    }
    
    public float CalculateUnityVisualScoreAndDrawLine()
    {
        VisualUnityScore = DrawAndCalculateDistances(PairwaiseElementsOfComposition);
        return VisualUnityScore;
    }

    private float DrawAndCalculateDistances(List<List<GameObject>>  PairwaiseElementsOfComposition)
    {
        float sumOfDistances = 0;
        int i = 0;
        foreach (List<GameObject> pair in PairwaiseElementsOfComposition)
        {
            MeshCollider MeshColStart = pair[0].GetComponent<MeshCollider>();
            Vector3 MeshCollStart = MeshColStart.ClosestPointOnBounds(pair[1].transform.position);

            MeshCollider MeshColEnd = pair[1].GetComponent<MeshCollider>();
            Vector3 MeshCollEnd = MeshColEnd.ClosestPointOnBounds(pair[0].transform.position);

            if (drawRenderedLinesDebug == true)
            {
                LineRenderer lineRendererPair = LineRendererObjectsList[i].GetComponent<LineRenderer>();
                lineRendererPair.SetPosition(0, MeshCollStart);
                lineRendererPair.SetPosition(1, MeshCollEnd);
                i++;
            }

            if (OverlappingColliders == false)
            {
                float dist = CalculateDistanceOf2MeshColliders(MeshCollStart, MeshCollEnd);
                sumOfDistances += dist;
            }

            else if (OverlappingColliders == true)
            {
                VisualUnityScore = 0;
                OverlappingColliders = false;
                return VisualUnityScore;
            }
        }
        double meanDistance = 0.0f;
        if (PairwaiseElementsOfComposition.Count > 0)
        {
            meanDistance  = sumOfDistances / PairwaiseElementsOfComposition.Count;
        }
        else
        {
            VisualUnityScore = 1.0f;
        }

        double VisualUnityScoreDouble = Math.Tanh(meanDistance * weight);
        VisualUnityScore = 1 - (float)VisualUnityScoreDouble;

        return VisualUnityScore;
    }

    List<List<GameObject>> PairwaiseOperation(List<GameObject> ElementsOfCompositionForPaiwiseOperation)
    {
        List<List<GameObject>>  result = new List<List<GameObject>>();

        for (int i = 0; i < ElementsOfCompositionForPaiwiseOperation.Count; i++)
        {
            for (int j = i; j < ElementsOfCompositionForPaiwiseOperation.Count; j++)
            {
                if (i != j)
                {
                    pair = new List<GameObject>();
                    pair.Add(ElementsOfCompositionForPaiwiseOperation[i]);
                    pair.Add(ElementsOfCompositionForPaiwiseOperation[j]);
                    result.Add(pair);
                }
            }
        }
        return result;
    }

    float CalculateDistanceOf2MeshColliders(Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        return dist;
    }

    public void FoundCollisionOfCompositionalElements()
    {
        OverlappingColliders = true;
    }

    public void FoundCollisionOfElementsINCompositionWithFrameBounds()
    {
        OverlappingColliders = true;
    }
}
