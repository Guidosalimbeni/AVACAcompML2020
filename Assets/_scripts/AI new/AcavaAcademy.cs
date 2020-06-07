using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AcavaAcademy : MonoBehaviour
{

    public GameObject[] elementsOfAiComposition;
    private Comp_agent_float[] agents;

    private void Awake()
    {
        agents = FindObjectsOfType<Comp_agent_float>();
        //populateScene();
        
    }

    public void EnvironmentReset(Comp_agent_float agent)
    {

        TagMeElementOfComposition elem = agent.GetComponentInChildren<TagMeElementOfComposition>();
        
        if(elem != null)
        {
            GameObject itemGO = elem.gameObject;
            Destroy(itemGO);
            Debug.Log(" I am here ");
        }
        

        Vector3 spawnLocation = new Vector3(UnityEngine.Random.Range(-1.2f, 1.2f), 0f, UnityEngine.Random.Range(-1.5f, 1.5f));
        var spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject item = Instantiate(elementsOfAiComposition[Random.Range(0, elementsOfAiComposition.Length)], spawnLocation, spawnRotation);
        GameObject Parent = agent.gameObject;
        item.transform.parent = Parent.transform;


        Debug.Log(" I am resetting academy");
    }

    
}
