using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class GrabbingAgent : Agent
{
    public GrabberEnvironmentListener m_Listener;
    public Transform m_FollowMe;
    public Vector3 m_StartingPosition;
    public Transform m_Worktop;
    public Transform m_Oven;
    public Transform m_GripPlate;
   
    public bool m_IsLive;

    public override void OnEpisodeBegin()
    {
        Debug.Log("Begin Episode");

        m_Listener.ResetGrabber();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Object Positions and Rotations
        sensor.AddObservation(this.transform.localPosition); // 3
        sensor.AddObservation(m_Worktop.localPosition); // 6
        sensor.AddObservation(m_Oven.localPosition); // 9
        sensor.AddObservation(m_Listener.m_Pan.transform.localPosition); // 12
        sensor.AddObservation(m_Listener.m_Pan.transform.rotation); // 15
        sensor.AddObservation(m_GripPlate.localPosition); // 18
        sensor.AddObservation(m_GripPlate.rotation); // 21
        sensor.AddObservation(m_Listener.m_Stove.position); // 24

        // Important Parameters
        sensor.AddObservation(m_Listener.m_GrabObject ? 1.0f : 0.0f); // 25
        sensor.AddObservation(m_Listener.m_IsEndEffectorAtTarget ? 1.0f : 0.0f); // 26

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]);
        this.transform.localPosition = controlSignal;

        bool grabObject = Mathf.Clamp(vectorAction[3], 0.0f, 1.0f) >= 0.5f ? true : false;
        m_Listener.m_GrabObject = grabObject;

        if (m_Listener.m_GameState.m_DidPlayerWin)
        {
            AddReward(1.0f);
            m_Listener.ResetGrabber();
        }
        else
        {
            AddReward(-0.005f);
        }

        if (this.transform.localPosition.x >= 1.285f || this.transform.localPosition.x <= -1.285f)
        {
            Debug.Log("Breached: X");
            SetReward(-1);
            if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }
            EndEpisode();
        }

        if (this.transform.localPosition.y < 0.9f)
        {
            Debug.Log("Too Low: " + this.transform.localPosition.y);
            SetReward(-1);
            if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

            EndEpisode();
        }

        if (this.transform.localPosition.y >= 6.0f)
        {
            Debug.Log("Too High");
            SetReward(-1);
            if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

            EndEpisode();
        }

        if (this.transform.localPosition.z >= 1.285 || this.transform.localPosition.z <= -1.285)
        {
            Debug.Log("Breached: Z");
            SetReward(-1);
            if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

            EndEpisode();
        }
       
    }

    public override float[] Heuristic()
    {
        float[] actions = new float[4];

        if (m_IsLive)
        {
            actions[0] = m_FollowMe.position.x;
            actions[1] = m_FollowMe.position.y;
            actions[2] = m_FollowMe.position.z;
        }
        else
        {
            actions[0] = m_FollowMe.localPosition.x;
            actions[1] = m_FollowMe.localPosition.y;
            actions[2] = m_FollowMe.localPosition.z;
        }

        actions[3] = m_Listener.m_GrabObject ? 1.0f : 0.0f;

        return actions;
    }
}
