using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Comp_agent_float_move_child : Agent
{

    public bool useVectorObs;
    public float speed = 10.0f;
    public bool WEBbuild = false;
    //public bool webtraining = false;
    private ScoreCalculator scoreCalculator;
    private AI_Calculator_score aI_Calculator_score;
    private bool targetReached = false;
    private Vector3 targetPosition;
    //private Camera cameraPaint;
    //private Camera cameraTop;
    public Transform centerPoint;
    private AcavaAcademy acavaAcademy;
    private float scoreFinalOut;
    private float scoreUnityVisual;
    private Comp_agent_float_move_child[] agentsChild;

    private void Awake()
    {

        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        aI_Calculator_score = FindObjectOfType<AI_Calculator_score>();
        acavaAcademy = FindObjectOfType<AcavaAcademy>();
        agentsChild = FindObjectsOfType<Comp_agent_float_move_child>();

        //if (webtraining == false)
        //{
        //    cameraPaint = Camera.main;
        //    cameraTop = GameObject.Find("renderCam[NN_Top]").GetComponent<Camera>();

        //    CameraSensorComponent[] camerasensors = gameObject.GetComponents<CameraSensorComponent>();

        //    for (int i = 0; i < camerasensors.Length; i++)
        //    {

        //        if (camerasensors[i].SensorName == "CameraPaint")
        //        {
        //            camerasensors[i].Camera = cameraPaint;
        //        }

        //        if (camerasensors[i].SensorName == "CameraSensor")
        //        {
        //            camerasensors[i].Camera = cameraTop;
        //        }
        //    }
        //}
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
        TagMeElementOfComposition TagChild = GetComponentInChildren<TagMeElementOfComposition>();

        CalculateVolumeOfElementComp VolChild = GetComponentInChildren<CalculateVolumeOfElementComp>();
        GameObject itemChild = child.gameObject;

        float Distance = 0.0f;

        for (int i = 0; i < agentsChild.Length; i++)
        {

            if (agentsChild[i].GetComponentInChildren<TagMeElementOfComposition>() != TagChild)
            {

                TagMeElementOfComposition tag = agentsChild[i].GetComponentInChildren<TagMeElementOfComposition>();
                GameObject otherItem = tag.gameObject;

                Distance += Vector3.Distance(transform.position, otherItem.transform.position);
            }
        }
        
        if (useVectorObs)
        {
            sensor.AddObservation(Distance); // +1
            sensor.AddObservation(VolChild.Volume); // +1
            sensor.AddObservation(TagChild.ElementOfCompositionID); // +1
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
            if (WEBbuild == false)
            {
                scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
                scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions
            }
            

            if (WEBbuild)
            {
                scoreFinalOut = (scoreCalculator.visualScoreBalancePixelsCount + scoreCalculator.scoreUnityVisual + scoreCalculator.scoreIsolationBalance + scoreCalculator.scoreLawOfLever) / 4;
                if (scoreCalculator.scoreUnityVisual == 0.0f)
                {
                    scoreFinalOut = scoreFinalOut * 0.5f;
                }
            }

        }
        
        else
        {
            if (WEBbuild == false)
            {
                scoreFinalOut = scoreCalculator.scoreFinalOut; // top reward
                scoreUnityVisual = scoreCalculator.scoreUnityVisual; // for collisions
            }

            if (WEBbuild)
            {
                scoreFinalOut = (scoreCalculator.visualScoreBalancePixelsCount + scoreCalculator.scoreUnityVisual + scoreCalculator.scoreIsolationBalance + scoreCalculator.scoreLawOfLever) / 4;
                if (scoreCalculator.scoreUnityVisual == 0.0f)
                {
                    scoreFinalOut = scoreFinalOut * 0.5f;
                }
            }
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
                AddReward(-10 / MaxStep);
                //AddReward(-0.5f);
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

