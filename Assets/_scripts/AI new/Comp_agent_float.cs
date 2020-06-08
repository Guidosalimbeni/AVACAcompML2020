using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Comp_agent_float : Agent
{
    
    public bool useVectorObs;
    public float speed = 10.0f;
    public bool webtraining = false;
    private ScoreCalculator scoreCalculator;
    private AI_Calculator_score aI_Calculator_score;
    private bool targetReached = false;
    private Vector3 targetPosition;
    private Camera cameraPaint;
    private Camera cameraTop;
    public Transform centerPoint;
    private AcavaAcademy acavaAcademy;
    private float scoreFinalOut;
    private float scoreUnityVisual;

    float a = 0.0f;

    private void Awake()
    {
        
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        aI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
        acavaAcademy = FindObjectOfType<AcavaAcademy>();
        
        
        if (webtraining == false)
        {
            cameraPaint = Camera.main;
            cameraTop = GameObject.Find("renderCam[NN_Top]").GetComponent<Camera>();

            CameraSensorComponent[] camerasensors = gameObject.GetComponents<CameraSensorComponent>();

            for (int i = 0; i < camerasensors.Length; i++)
            {

                if (camerasensors[i].SensorName == "CameraPaint")
                {
                    camerasensors[i].Camera = cameraPaint;
                }

                if (camerasensors[i].SensorName == "CameraSensor")
                {
                    camerasensors[i].Camera = cameraTop;
                }
            }
        }
        

    }


private void OnTriggerEnter(Collider other)
    {

        if (aI_Calculator_score.inferenceMode == false)
        {
            if (other.gameObject.tag == "bounds")
            {
                Debug.Log(" out of table");
                
                AddReward(-1 / MaxStep);

            }
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            //sensor.AddObservation(StepCount / (float)MaxStep);
            sensor.AddObservation(gameObject.transform.rotation.y);
            sensor.AddObservation(gameObject.transform.position);
            sensor.AddObservation(centerPoint.transform.position - gameObject.transform.position);
            
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


    private void FireScoreCalculation()
    {
        
        
        
        

        if (aI_Calculator_score.inferenceMode == false)
        {
            scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
            scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions



            if (scoreFinalOut > a)
            {
                Debug.Log(scoreFinalOut);
                a = scoreFinalOut;
            }
        }

        else
        {
            scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions


            float visualScoreBalancePixelsCount = scoreCalculator.visualScoreBalancePixelsCount;
            float scoreLawOfLever = scoreCalculator.scoreLawOfLever;
            float scoreIsolationBalance = scoreCalculator.scoreIsolationBalance;

            scoreFinalOut = (scoreUnityVisual + visualScoreBalancePixelsCount + scoreLawOfLever + scoreIsolationBalance) / 4;
        }
            

        //Debug.Log(a);
        

        if (scoreFinalOut > aI_Calculator_score.target)
        {
            
            if (aI_Calculator_score.inferenceMode)
            {
                targetReached = true;
                
            }
            else
            {
                Debug.Log(" WINNNNNNIIIINNG");
                SetReward(1f);
                EndEpisode(); 
            }
        }

        else
        {
            targetReached = false;
        }


        if (scoreUnityVisual == 0)
        {
            if (aI_Calculator_score.inferenceMode == false)
                AddReward(-1 / MaxStep);
            //EndEpisode(); // will make everything restarts
        }

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (aI_Calculator_score.inferenceMode == false)
            AddReward(-1f / MaxStep);
        MoveAgent(vectorAction);
    }


    public override void OnEpisodeBegin()
    {
        a = 0.0f;



        //transform.position = new Vector3(UnityEngine.Random.Range(-1.2f, 1.2f), 0f, UnityEngine.Random.Range(-1.5f, 1.5f));
        //transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);


        if (targetReached == false)
            acavaAcademy.EnvironmentReset(this);
    }

    //public override void Heuristic(float[] actionsOut)
    //{
    //    actionsOut[0] = Input.GetAxis("Horizontal");    

    //    actionsOut[1] = Input.GetAxis("Vertical");   
    //    actionsOut[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;   
    //}
}

