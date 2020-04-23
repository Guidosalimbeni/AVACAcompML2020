using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCompML : MonoBehaviour
{

    public static GameManagerCompML Instance { get; private set; }

    public List<GameObject> ElementsCompositionsSelected { get; set; }
    public int Lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ElementsCompositionsSelected = new List<GameObject>();
        }
    }

    internal void PopulateSelectedElementOfComposition(List<GameObject> ElementsCompositionsSelectedPassed)
    {
        ElementsCompositionsSelected = ElementsCompositionsSelectedPassed;
    }

}
