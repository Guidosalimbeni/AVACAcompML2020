using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePopulationController : MonoBehaviour
{
    
    public float MaxValues_x = 1.5f;
    public float MinValues_z = -1.5f;
    public Transform parent;
    public List<GameObject> ElementsCompositions { get; set; } 

    private void Awake() // important to come first
    {

        if (GameManagerCompML.Instance != null)
        {
            if (GameManagerCompML.Instance.ElementsCompositionsSelected != null)
            {
                List<GameObject> prefabs = GameManagerCompML.Instance.ElementsCompositionsSelected;

                foreach (GameObject prefab in prefabs)
                {
                    GameObject go = Instantiate(prefab, parent);
                }
            }
        }
        
        // instantiate from game manager... (if there already in scene for debugging ... no problem...)

        PopulateTheElementsOfCompositionInTheScene();
        SetLayerToForeground();
    }

    private void PopulateTheElementsOfCompositionInTheScene() 
    {

        ElementsCompositions = new List<GameObject>();

        TagMeElementOfComposition[] elementstags = FindObjectsOfType<TagMeElementOfComposition>();
        foreach (var elementtag in elementstags)
        {
            GameObject go = elementtag.gameObject;
            ElementsCompositions.Add(go);
        }
    }

    private void SetLayerToForeground()
    {
        foreach (var item in ElementsCompositions)
        {
            item.layer = 9; // set layer to Foreground
        }
    }

}
