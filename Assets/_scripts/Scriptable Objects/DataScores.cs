using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new NNScore", menuName = "CompML NN score", order = 51)]

public class DataScores : ScriptableObject
{

    [SerializeField]
    private double nnScore;

    public double NNSCORE { get { return nnScore; } set { nnScore = value; } }
  

}
