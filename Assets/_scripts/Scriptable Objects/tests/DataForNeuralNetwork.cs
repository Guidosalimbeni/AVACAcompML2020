using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewNNData", menuName = "Neural Network Data", order = 51)]

public class DataForNeuralNetwork : ScriptableObject
{

    [SerializeField]
    private List<string> names;

    [SerializeField]
    private string name;
    [SerializeField]
    private float g0;
    [SerializeField]
    private float g1;
    [SerializeField]
    private float g2;
    [SerializeField]
    private float g3;
    [SerializeField]
    private float g4;
    [SerializeField]
    private float g5;
    [SerializeField]
    private int judge;

    public List<string> Names { get { return names; } set { names = value; } }


    public string Name { get { return name;} set { name = value; } }
    public float G0 { get { return g0; } }
    public float G1 { get { return g1; } }
    public float G2 { get { return g2; } }
    public float G3 { get { return g3; } }
    public float G4 { get { return g4; } }
    public float G5 { get { return g5; } }
    public int JUDGE { get { return judge; } }
}
