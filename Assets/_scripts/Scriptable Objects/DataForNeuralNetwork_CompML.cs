using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new compMLData", menuName = "CompML Neural Network Data", order = 51)]

public class DataForNeuralNetwork_CompML : ScriptableObject
{

    // eventually change this to get the only 3 scores summary for training.. but better not to train in unity and train offline ... NOT USED

    [SerializeField]
    private List<string> names;
    [SerializeField]
    private List<double> scorePixelsBalance;
    [SerializeField]
    private List<double> scoreUnityVisual;
    [SerializeField]
    private List<double> scoreLawOfLever;
    [SerializeField]
    private List<double> scoreIsolationBalance;
    [SerializeField]
    private List<double> scoreNNFrontTop;
    [SerializeField]
    private List<double> scoreMobileNet;
    [SerializeField]
    private List<double> judges;

    public List<string> Names { get { return names; } set { names = value; } }
    public List<double> ScorePixelsBalance { get { return scorePixelsBalance; } set { scorePixelsBalance = value; } }
    public List<double> ScoreUnityVisual { get { return scoreUnityVisual; } set { scoreUnityVisual = value; } }
    public List<double> ScoreLawOfLever { get { return scoreLawOfLever; } set { scoreLawOfLever = value; } }
    public List<double> ScoreIsolationBalance { get { return scoreIsolationBalance; } set { scoreIsolationBalance = value; } }
    public List<double> ScoreNNFrontTop { get { return scoreNNFrontTop; } set { scoreNNFrontTop = value; } }
    public List<double> ScoreMobileNet { get { return scoreMobileNet; } set { scoreMobileNet = value; } }
    public List<double> JUDGE { get { return judges; } set { judges = value; } }

}

