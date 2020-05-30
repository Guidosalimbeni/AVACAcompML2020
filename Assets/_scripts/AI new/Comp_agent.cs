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
    Rigidbody m_AgentRb;

    private BarracudaFinalOut barracudaFinalOut;
    private float scoreFinalOut;
    
    // mettere altri score e dare rewards a seconda... dello score...

    private void Awake()
    {

        barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        barracudaFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
    }

    private void Handle_OnScoreFinalOutChanged(float scoreFinalOutPassed)
    {
        scoreFinalOut = scoreFinalOutPassed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bounds")
        {
            AddReward(-0.01f);
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
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        m_AgentRb.AddForce(dirToGo * agentRunSpeed, ForceMode.VelocityChange);

        FireScoreCalculation();
    }

    private void FireScoreCalculation()
    {
        if (scoreFinalOut > target)
        {
            SetReward(1f);
            EndEpisode();
        }

        if (scoreFinalOut > 0.5f)
        {
            Debug.Log(" got at 0.5");
            AddReward(0.05f);
            EndEpisode();
        }

        if (scoreFinalOut > 0.2f)
        {
            Debug.Log(" got at 0.2");
            AddReward(0.0025f);
            EndEpisode();
        }

        if (scoreFinalOut > 0.1f)
        {
            Debug.Log(" got at 0.1");
            AddReward(0.0005f);
            EndEpisode();
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
        transform.position = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0f, UnityEngine.Random.Range(-2f, 2f));
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        m_AgentRb.velocity *= 0f;

    }

    private void OnDisable()
    {
        barracudaFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;
    }
}

