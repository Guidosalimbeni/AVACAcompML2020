using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TrainingSet
{
    public double[] input;
    public double output;
}

public class Perceptron : MonoBehaviour {

    [SerializeField]
    private DataForNeuralNetwork_CompML dataNN;

    public bool TrainPerceptron = false; // not used but kept the code in case for other project all training better to be offline with Keras and then TFsharp import so I can see the evaluation in python

    public TrainingSet[] ts;
    public double[] weights = { 0, 0,0,0,0,0 };
    double bias = 0;
    double totalError = 0;


    void Start() // to call from the getFromDatabase script
    {

        if (TrainPerceptron)
        {
            //ts[0].input = new double[5];

            ts = new TrainingSet[dataNN.Names.Count];

            for (int i = 0; i < dataNN.Names.Count; i++)
            {
                ts[i].input = new double[6];
                ts[i].input[0] = dataNN.ScorePixelsBalance[i];
                ts[i].input[1] = dataNN.ScoreUnityVisual[i];
                ts[i].input[2] = dataNN.ScoreLawOfLever[i];
                ts[i].input[3] = dataNN.ScoreIsolationBalance[i];
                ts[i].input[4] = dataNN.ScoreNNFrontTop[i];
                ts[i].input[5] = dataNN.ScoreMobileNet[i];
                ts[i].output = dataNN.JUDGE[i];
            }


            Train(100);   // to uncomment

            //Debug.Log("Test 0 0: " + CalcOutput(0, 0));
            //Debug.Log("Test 0 1: " + CalcOutput(0, 1));
            //Debug.Log("Test 1 0: " + CalcOutput(1, 0));
            //Debug.Log("Test 1 1: " + CalcOutput(1, 1));

        }

    }

	double DotProductBias(double[] v1, double[] v2) 
	{
		if (v1 == null || v2 == null)
			return -1;
	 
		if (v1.Length != v2.Length)
			return -1;
	 
		double d = 0;
		for (int x = 0; x < v1.Length; x++)
		{
			d += v1[x] * v2[x];
		}

		d += bias;
	 
		return d;
	}

	double CalcOutput(int i)
	{
		double dp = DotProductBias(weights,ts[i].input);
		if(dp > 0) return(1);
		return (0);
	}

	void InitialiseWeights()
	{
		for(int i = 0; i < weights.Length; i++)
		{
			weights[i] = Random.Range(-1.0f,1.0f);
		}
		bias = Random.Range(-1.0f,1.0f);
	}

	void UpdateWeights(int j)
	{
		double error = ts[j].output - CalcOutput(j);
		totalError += Mathf.Abs((float)error);
		for(int i = 0; i < weights.Length; i++)
		{			
			weights[i] = weights[i] + error*ts[j].input[i]; 
		}
		bias += error;
	}

	double CalcOutput(double i1, double i2)
	{
		double[] inp = new double[] {i1, i2};
		double dp = DotProductBias(weights,inp);
		if(dp > 0) return(1);
		return (0);
	}

	void Train(int epochs)
	{
		InitialiseWeights();
		
		for(int e = 0; e < epochs; e++)
		{
			totalError = 0;
			for(int t = 0; t < ts.Length; t++)
			{
				UpdateWeights(t);
				//Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " W3: " + (weights[2]) + " W4: " 
                    //+ (weights[3]) + " W5: " + (weights[4]) + " B: " + bias);
			}
			Debug.Log("TOTAL ERROR: " + totalError);


            // create a bool to run or use weights on 1.

            // call from Database loading ... check consistency with linear regression .. leave them as they are to discover that all contribute positivaly  debug...
		}
	}
}
