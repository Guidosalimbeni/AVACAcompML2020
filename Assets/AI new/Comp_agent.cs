using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Comp_agent : Agent
{
    
    public bool useVectorObs;
    public float agentRunSpeed = 10;
    Rigidbody m_AgentRb;
    
    int m_Selection;

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
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-1f / MaxStep);
        MoveAgent(vectorAction);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("symbol_O_Goal") || col.gameObject.CompareTag("symbol_X_Goal"))
        {
            if ((m_Selection == 0 && col.gameObject.CompareTag("symbol_O_Goal")) ||
                (m_Selection == 1 && col.gameObject.CompareTag("symbol_X_Goal")))
            {
                SetReward(1f);
                
            }
            else
            {
                SetReward(-0.1f);
                
            }
            EndEpisode();
        }
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
        //var agentOffset = -15f;
        //var blockOffset = 0f;
        //m_Selection = Random.Range(0, 2);
        //if (m_Selection == 0)
        //{
        //    symbolO.transform.position =
        //        new Vector3(0f + Random.Range(-3f, 3f), 2f, blockOffset + Random.Range(-5f, 5f))
        //        + ground.transform.position;
        //    symbolX.transform.position =
        //        new Vector3(0f, -1000f, blockOffset + Random.Range(-5f, 5f))
        //        + ground.transform.position;
        //}
        //else
        //{
        //    symbolO.transform.position =
        //        new Vector3(0f, -1000f, blockOffset + Random.Range(-5f, 5f))
        //        + ground.transform.position;
        //    symbolX.transform.position =
        //        new Vector3(0f, 2f, blockOffset + Random.Range(-5f, 5f))
        //        + ground.transform.position;
        //}

        //transform.position = new Vector3(0f + Random.Range(-3f, 3f),
        //    1f, agentOffset + Random.Range(-5f, 5f))
        //    + ground.transform.position;
        //transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        //m_AgentRb.velocity *= 0f;

        //var goalPos = Random.Range(0, 2);
        //if (goalPos == 0)
        //{
        //    symbolOGoal.transform.position = new Vector3(7f, 0.5f, 22.29f) + area.transform.position;
        //    symbolXGoal.transform.position = new Vector3(-7f, 0.5f, 22.29f) + area.transform.position;
        //}
        //else
        //{
        //    symbolXGoal.transform.position = new Vector3(7f, 0.5f, 22.29f) + area.transform.position;
        //    symbolOGoal.transform.position = new Vector3(-7f, 0.5f, 22.29f) + area.transform.position;
        //}
    }
}

