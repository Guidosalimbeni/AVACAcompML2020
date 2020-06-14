using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Comp_agent_float_move_child : Agent
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

    public void PullTrigger(Collider other)
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


    public override void CollectObservations(VectorSensor sensor) // get from child
    {

        ChildTrigger child = GetComponentInChildren<ChildTrigger>();
        GameObject itemChild = child.gameObject;
        
        if (useVectorObs)
        {
            sensor.AddObservation(itemChild.transform.rotation.y);
            sensor.AddObservation(itemChild.transform.position);
            sensor.AddObservation(centerPoint.transform.position - itemChild.transform.position);

        }
    }

    public void MoveAgent(float[] vectorAction) // send to child
    {
        ChildTrigger child = GetComponentInChildren<ChildTrigger>();
        GameObject itemChild = child.gameObject;

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
                    itemChild.transform.position = Vector3.MoveTowards(itemChild.transform.position, targetPosition, step);
                    itemChild.transform.rotation = Quaternion.Euler(0f, rotateY * 360, 0f);
                }
                else
                {
                    itemChild.transform.position = new Vector3(posX,
                    itemChild.transform.position.y,
                    PosZ);
                    itemChild.transform.rotation = Quaternion.Euler(0f, rotateY * 360, 0f);
                }

            }

            else
            {
                itemChild.transform.position = new Vector3(posX,
                    itemChild.transform.position.y,
                    PosZ);
                itemChild.transform.rotation = Quaternion.Euler(0f, rotateY * 360, 0f);
            }

        }

        FireScoreCalculation();

    }


    private void FireScoreCalculation()
    {

        if (aI_Calculator_score.inferenceMode == false)
        {
            scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
            scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions

        }

        else
        {
            scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions


            //float visualScoreBalancePixelsCount = scoreCalculator.visualScoreBalancePixelsCount;
            //float scoreLawOfLever = scoreCalculator.scoreLawOfLever;
            //float scoreIsolationBalance = scoreCalculator.scoreIsolationBalance;

            //scoreFinalOut = (scoreUnityVisual + visualScoreBalancePixelsCount + scoreLawOfLever + scoreIsolationBalance) / 4;

            scoreFinalOut = scoreCalculator.scoreFinalOut;
            
        }


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

        if (targetReached == false)
            acavaAcademy.EnvironmentReset(this);
    }

    public void ResetEnviromentFromPlayMode()
    {
        acavaAcademy.EnvironmentReset(this);
    }

}

