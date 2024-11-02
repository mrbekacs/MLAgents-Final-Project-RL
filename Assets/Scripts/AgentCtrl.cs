using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents; //To change the base class from MonoBehaviour to Agent
using Unity.MLAgents.Actuators; //To change the base class from MonoBehaviour to Agent
using Unity.MLAgents.Sensors; //To change the base class from MonoBehaviour to Agent

public class AgentCtrl : Agent
{
    [SerializeField] private Transform target; //It's a reference which stores a GameObject's position, orientation and scale in the 3D world
    [SerializeField] private float moveSpeed = 4f; //Global variable for the speed of the agent

    public override void OnEpisodeBegin() // This method is called to set-up the environment for a new episode
    {
        // Agent position (random)
        transform.localPosition = new Vector3(Random.Range(-5.5f, 5.5f), 0.3f, Random.Range(-5.5f, 5.5f));

        //Target position (random)
        target.localPosition = new Vector3(Random.Range(-5.5f, 5.5f), 0.3f, Random.Range(-5.5f, 5.5f));
    }
    public override void CollectObservations(VectorSensor sensor) // This method collects the necessary information
    {
        // Target and Agent positions
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions) //This method receives actions
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        // the speed of the Agent
        Vector3 velocity = new Vector3(moveX, 0f, moveZ).normalized * Time.deltaTime * moveSpeed;

        transform.localPosition += velocity;
    }

    public override void Heuristic(in ActionBuffers actionsOut) //This method enables us to move the Agent with W-A-S-D buttons or Arrow buttons
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }

    private void OnTriggerEnter(Collider other) //This method is used to detect the collision of the Agent
    {
        if(other.gameObject.tag == "Diamond") //Tag for the target and condition to add reward
        {
            AddReward(2f);
            EndEpisode();
        }

        if(other.gameObject.tag == "Wall") // Tag for the wall and condition to penalty 
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
