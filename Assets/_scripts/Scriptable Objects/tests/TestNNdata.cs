using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNNdata : MonoBehaviour
{


    [SerializeField]
    private DataForNeuralNetwork dataNN; // 1


    [SerializeField]
    private GameEvent OnSwordSelected; // 1

    private void OnMouseDown()
    {

        dataNN.Name = "Giulio";

        foreach(string name in dataNN.Names)
        {
            Debug.Log(name);
        }

        Debug.Log(dataNN.Name); // 3
        OnSwordSelected.Raise(); // 2
    }


}
