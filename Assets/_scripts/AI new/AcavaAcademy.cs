using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AcavaAcademy : MonoBehaviour
{

    public GameObject[] elementsOfAiComposition;
    private List <GameObject> itemToPassForCalculation;

    private GamePopulationController GamePopulationController;
    private int NumberOfAgentsInScene;
    private int count = 0;

    private void Awake()
    {
        
        GamePopulationController = FindObjectOfType<GamePopulationController>();

        Comp_agent_float[] agents = FindObjectsOfType<Comp_agent_float>();
        NumberOfAgentsInScene = agents.Length;
        itemToPassForCalculation = new List<GameObject>();
    }


    public void EnvironmentReset(Comp_agent_float agent)
    {


        TagMeElementOfComposition elem = agent.GetComponentInChildren<TagMeElementOfComposition>();
        
        if(elem != null)
        {
            GameObject itemGO = elem.gameObject;
            Destroy(itemGO);
            
        }


        //Vector3 spawnLocation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0f, UnityEngine.Random.Range(-1.0f, 1.0f));
        //var spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        Vector3 spawnLocation = new Vector3(0f, 0f, 0f);
        var spawnRotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject item = Instantiate(elementsOfAiComposition[Random.Range(0, elementsOfAiComposition.Length)], spawnLocation, spawnRotation);
        GameObject Parent = agent.gameObject;
        item.transform.parent = Parent.transform;

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
