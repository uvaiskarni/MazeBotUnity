using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MazeAgent : Agent
{
    Rigidbody player;
    public float agentSpeed;
    public Transform Suspect;

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // reseting player
        //player.transform.localPosition = new Vector3(Random.value * 25-20, 0f, Random.value * 25-20);
        //player.transform.localPosition = new Vector3(-29f, 0f, -18f);
        player.transform.localPosition = new Vector3(42.3f, 0f, -17.4f);
        player.angularVelocity = Vector3.zero;
        player.velocity = Vector3.zero;
        // Move the Suspect to a new spot
        //Suspect.localPosition = new Vector3(Random.value * 0,0f,Random.value * 0);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Suspect positions
        sensor.AddObservation(Suspect.localPosition);
        // player positions
        sensor.AddObservation(player.transform.localPosition);
        // player velocity
        sensor.AddObservation(player.velocity);
        // player Distance towards suspect
        sensor.AddObservation(Vector3.Distance(Suspect.transform.localPosition, player.transform.localPosition));
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
        player.AddForce(dirToGo * agentSpeed, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float distanceToTarget = Vector3.Distance(player.transform.localPosition, Suspect.localPosition);

        AddReward(-0.001f);
        MoveAgent(vectorAction);

        if (distanceToTarget < 2f)
        {
            SetReward(10f);
            EndEpisode();
        }

    }

    void OnCollisionEnter(Collision exampleCol)
    {

        if (exampleCol.collider.tag == "Wall")
        {
            AddReward(-0.1f);
            //EndEpisode();
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
}
