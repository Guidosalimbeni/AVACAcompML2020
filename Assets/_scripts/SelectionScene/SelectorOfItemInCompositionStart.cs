using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SelectorOfItemInCompositionStart : MonoBehaviour
{
    public List<GameObject> CompositionElementsPrefabs;

    private List<int> selectedItemIDS;
    private List<GameObject> CompositionElementsPrefabsSELECTED;

    public event Action<int> OnSelectionADDEDOfItemID;
    public event Action<int> OnSelectionREMOVEDOfItemID;

    private void Start()
    {
        selectedItemIDS = new List<int>();
        CompositionElementsPrefabsSELECTED = new List<GameObject>();
    }

    public void PopulateSelectedItemIdsFromButton(int ID)
    {
        selectedItemIDS.Add(ID);
        if (OnSelectionADDEDOfItemID != null)
            OnSelectionADDEDOfItemID(ID);
    }

    public void RemoveUnselectedItemIdsFromButton(int ID)
    {
        selectedItemIDS.Remove(ID);
        if (OnSelectionREMOVEDOfItemID != null)
            OnSelectionREMOVEDOfItemID(ID);
    }

    public void PopulateElementsCompositionsSelected()
    {
        CompositionElementsPrefabsSELECTED.Clear();

        foreach ( int id in selectedItemIDS)
        {
            CompositionElementsPrefabsSELECTED.Add(CompositionElementsPrefabs[id - 1]);
        }

        GameManagerCompML.Instance.PopulateSelectedElementOfComposition(CompositionElementsPrefabsSELECTED);
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);

    }

}
