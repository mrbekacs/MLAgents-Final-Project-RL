using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentCtrl : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 4f;

    public override void OnEpisodeBegin()
    {
        // Agent position
        transform.localPosition = new Vector3(Random.Range(-5.5f, 5.5f), 0.3f, Random.Range(-5.5f, 5.5f));

        //Target position
        target.localPosition = new Vector3(Random.Range(-5.5f, 5.5f), 0.3f, Random.Range(-5.5f, 5.5f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 velocity = new Vector3(moveX, 0f, moveZ).normalized * Time.deltaTime * moveSpeed;

        transform.localPosition += velocity;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Diamond")
        {
            AddReward(2f);
            EndEpisode();
        }

        if(other.gameObject.tag == "Wall")
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
