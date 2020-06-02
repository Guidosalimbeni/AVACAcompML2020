using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Comp_agent : Agent
{
    
    public bool useVectorObs;
    public float agentRunSpeed = 10;
    public float target = 0.95f;
    public Transform centerPoint;
    Rigidbody m_AgentRb;

    private ScoreCalculator scoreCalculator;
    private bool targetReached = false;

    private void Awake()
    {
        
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bounds")
        {
            Debug.Log(" out of table");
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            sensor.AddObservation(StepCount / (float)MaxStep);
            sensor.AddObservation(gameObject.transform.rotation.y);
            sensor.AddObservation(centerPoint.transform.position - gameObject.transform.position);
            sensor.AddObservation(m_AgentRb.velocity);
            sensor.AddObservation(m_AgentRb.mass);
        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }

        if (targetReached == false)
        {
            transform.Rotate(rotateDir, Time.deltaTime * 150f);
            m_AgentRb.AddForce(dirToGo * agentRunSpeed, ForceMode.VelocityChange);
        }
        

        FireScoreCalculation();
    }

    private void FireScoreCalculation()
    {
        float scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
        //float visualScoreBalancePixelsCount = scoreCalculator.visualScoreBalancePixelsCount;
        float scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions
        float scoreNNFrontTop = scoreCalculator.scoreNNFrontTop;
        float scoreMobileNet = scoreCalculator.scoreMobileNet;
        float scoreAllscorefeatures = scoreCalculator.scoreAllscorefeatures;


        if (scoreFinalOut > target)
        {
            SetReward(1f);
            Debug.Log(" WINNNNNNIIIINNG");
            //EndEpisode(); // comment out during inference
            targetReached = true;
            m_AgentRb.velocity *= 0f;

        }

        else
        {
            targetReached = false;
        }

        //else {

        //    targetReached = false;

        //    if (scoreMobileNet > target * 0.6)
        //    {
        //        Debug.Log(" got CNN");
        //        AddReward(1f / MaxStep / 3);

        //    }

        //    if (scoreNNFrontTop > target * 0.6)
        //    {
        //        Debug.Log(" got NN front TOP");
        //        AddReward(1f / MaxStep / 3);
        //    }

        //    if (scoreAllscorefeatures > target * 0.6)
        //    {
        //        Debug.Log(" got all features");
        //        AddReward(1f / MaxStep / 3);
        //    }
        //} 

        if (scoreUnityVisual == 0)
        {
            Debug.Log(" collision ");
            //SetReward(-0.05f);
            //EndEpisode(); // will make everything restarts
        }
        

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-1f / MaxStep);
        MoveAgent(vectorAction);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        m_AgentRb.velocity *= 0f;
    }

}

