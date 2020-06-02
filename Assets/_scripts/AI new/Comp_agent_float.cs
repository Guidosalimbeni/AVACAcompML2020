using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Comp_agent_float : Agent
{
    
    public bool useVectorObs;

    public float speed = 1.0f;
    public Transform centerPoint;
    Rigidbody m_AgentRb;

    private ScoreCalculator scoreCalculator;
    private AI_Calculator_score aI_Calculator_score;
    private bool targetReached = false;
    private Vector3 targetPosition;
    

    private void Awake()
    {
        
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        aI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
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

    public void MoveAgent(float[] vectorAction)
    {

        if (targetReached == false)
        {
            var posX = Mathf.Clamp(vectorAction[0], -1.5f, 1.5f);
            var PosZ = Mathf.Clamp(vectorAction[1], -1.5f, 1.5f);
            var rotateY = Mathf.Clamp(vectorAction[2], -1f, 1f);

            if (aI_Calculator_score.inferenceMode)
            {
                if (aI_Calculator_score.movetotarget)
                {
                    targetPosition = new Vector3(posX,
                    transform.position.y,
                    PosZ);

                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                    //StartCoroutine(MovetoTarget(targetPosition));
                }
                else
                {
                    transform.position = new Vector3(posX,
                    transform.position.y,
                    PosZ);
                }

            }

            else
            {
                transform.position = new Vector3(posX,
                    transform.position.y,
                    PosZ);
            }


            transform.rotation = Quaternion.Euler(0f, rotateY * 360, 0f);

        }
        

        FireScoreCalculation();
    }

    //private IEnumerator MovetoTarget(Vector3 targetPosition)
    //{
    //    float step = speed * Time.deltaTime; 
    //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

    //    yield return new WaitForSeconds(0.01f);
    //}

    private void FireScoreCalculation()
    {
        float scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
        //float visualScoreBalancePixelsCount = scoreCalculator.visualScoreBalancePixelsCount;
        float scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions
        float scoreNNFrontTop = scoreCalculator.scoreNNFrontTop;
        float scoreMobileNet = scoreCalculator.scoreMobileNet;
        float scoreAllscorefeatures = scoreCalculator.scoreAllscorefeatures;


        if (scoreFinalOut > aI_Calculator_score.target)
        {
            SetReward(1f);
            Debug.Log(" WINNNNNNIIIINNG");
            if (aI_Calculator_score.inferenceMode)
            {
                targetReached = true;
            }
            else
            {
                EndEpisode(); 
                targetReached = true;
            }
            
            

        }

        else
        {
            targetReached = false;
        }

        //else {

        //    targetReached = false;

        //    if (scoreMobileNet > aI_Calculator_score.target * 0.6)
        //    {
        //        Debug.Log(" got CNN");
        //        AddReward(1f / MaxStep / 3);

        //    }

        //    if (scoreNNFrontTop > aI_Calculator_score.target * 0.6)
        //    {
        //        Debug.Log(" got NN front TOP");
        //        AddReward(1f / MaxStep / 3);
        //    }

        //    if (scoreAllscorefeatures > aI_Calculator_score.target * 0.6)
        //    {
        //        Debug.Log(" got all features");
        //        AddReward(1f / MaxStep / 3);
        //    }
        //} 

        if (scoreUnityVisual == 0)
        {
            Debug.Log(" collision ");
            AddReward(-1/MaxStep);
            //EndEpisode(); // will make everything restarts
        }

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-1f / MaxStep);
        MoveAgent(vectorAction);
    }


    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), 0f, UnityEngine.Random.Range(-1.5f, 1.5f));
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        m_AgentRb.velocity *= 0f;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");    
        
        actionsOut[1] = Input.GetAxis("Vertical");   
        actionsOut[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;   
    }
}

