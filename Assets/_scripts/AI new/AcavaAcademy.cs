﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AcavaAcademy : MonoBehaviour
{

    public GameObject[] elementsOfAiComposition;
    private List <GameObject> itemToPassForCalculation;

    private GamePopulationController GamePopulationController;
    private CalculateCollisionDistanceVisualUnity CalculateCollisionDistanceVisualUnity;
    private int NumberOfAgentsInScene;
    

    private void Awake()
    {
        
        GamePopulationController = FindObjectOfType<GamePopulationController>();
        CalculateCollisionDistanceVisualUnity = FindObjectOfType<CalculateCollisionDistanceVisualUnity>();
        Comp_agent_float_move_child[] agents = FindObjectsOfType<Comp_agent_float_move_child>();
        NumberOfAgentsInScene = agents.Length;
        itemToPassForCalculation = new List<GameObject>();
    }


    public void EnvironmentReset(Comp_agent_float_move_child agent)
    {
        TagMeElementOfComposition elem = agent.GetComponentInChildren<TagMeElementOfComposition>();
        
        if(elem != null)
        {
            GameObject itemGO = elem.gameObject;
            Destroy(itemGO);
            
        }

        Vector3 spawnLocation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0f, UnityEngine.Random.Range(-1.5f, 1.5f));
        //var spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject Parent = agent.gameObject;
        GameObject item = Instantiate(elementsOfAiComposition[Random.Range(0, elementsOfAiComposition.Length)], Parent.transform.localPosition, Parent.transform.localRotation);
        
        item.transform.parent = Parent.transform;

        item.transform.position = spawnLocation;


        itemToPassForCalculation.Add(item);
        if (itemToPassForCalculation.Count > NumberOfAgentsInScene)
        {
            itemToPassForCalculation.RemoveAt(0);
        }

        PopulateTheElementsOfCompositionInTheSceneFromAcademy();
    }


    private void PopulateTheElementsOfCompositionInTheSceneFromAcademy()
    {

        GamePopulationController.ElementsCompositions = itemToPassForCalculation;

        CalculateCollisionDistanceVisualUnity.UpdatePairWiseElementForVisualUnityScoreFromAIAcademy();

        SetLayerToForeground();
    }

    private void SetLayerToForeground()
    {
        foreach (var item in GamePopulationController.ElementsCompositions)
        {
            item.layer = 9; // set layer to Foreground
        }
    }


}
