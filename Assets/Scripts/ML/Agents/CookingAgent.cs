using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class CookingAgent : Agent
{
    public GameState m_GameState;
    public MachineLearningManager m_MLManager;
    public Transform m_EndEffector;
    public Vector3 m_StartingPosition;
    public Transform m_FollowMe = null;
    public Animator m_AgentPlayback;
    public Animator m_GrabObjectPlayback;
    public bool m_IsLive;
    public float m_TransformMultiplier;

    private float m_PreviousDistanceToPan;
    private float m_PreviousDistanceToEgg;
    private float m_PreviousDistanceToFriedEgg;
    private float m_PreviousDistanceToPlate;

    public override void OnEpisodeBegin()
    {
        Debug.Log("Begin Episode");

        m_MLManager.EnvironmentReset();

        m_PreviousDistanceToPan = 50.0f;
        m_PreviousDistanceToEgg = 50.0f;
        m_PreviousDistanceToFriedEgg = 50.0f;
        m_PreviousDistanceToPlate = 50.0f;

        if (!m_IsLive)
        {
            if (m_AgentPlayback != null)
            {
                m_AgentPlayback.Play("AgentCookingClip_Teacher", -1, 0.0f);
            }

            if (m_GrabObjectPlayback != null)
            {
                m_GrabObjectPlayback.Play("GrabObjectCookingClip_Teacher", -1, 0.0f);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target Position, 3
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(m_EndEffector.transform.position); // 3

        // Machine Learning Manager Params
        sensor.AddObservation(m_MLManager.m_GrabObject ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsEndEffectorAtTarget ? 1.0f : 0.0f); // 1

        // Interactable Transforms
        sensor.AddObservation(m_MLManager.m_Pan.transform.localPosition); // 3
        sensor.AddObservation(m_MLManager.m_Stove.localPosition); // 3
        sensor.AddObservation(m_MLManager.m_UncrackedEggs[0].transform.localPosition); // 3
        sensor.AddObservation(m_MLManager.m_CrackEggs[0].transform.localPosition); // 3
        sensor.AddObservation(m_MLManager.m_Plate.transform.localPosition); // 3

        // Current Recorded States
        sensor.AddObservation(m_MLManager.m_IsPanOnStove ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsEggOnPan ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsFriedEgg ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsEggBurnt ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsEggOnToast ? 1.0f : 0.0f); // 1
        sensor.AddObservation(m_MLManager.m_IsEggToastOnPlate ? 1.0f : 0.0f); // 1

        // Timers for both the egg and toast
        sensor.AddObservation(m_MLManager.m_CurrentEggTime); // 1
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Target Position, 3
        Vector3 controlSignal = new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]);

        this.transform.localPosition = controlSignal * m_TransformMultiplier;

        float distanceToPan = Vector3.Distance(this.transform.localPosition, m_MLManager.m_Pan.transform.localPosition);
        float distanceToEgg = Vector3.Distance(this.transform.localPosition, m_MLManager.m_UncrackedEggs[0].transform.localPosition);
        float distanceToFriedEgg = Vector3.Distance(this.transform.localPosition, m_MLManager.m_CrackEggs[0].transform.localPosition);
        float distanceToPlate = Vector3.Distance(this.transform.localPosition, m_MLManager.m_Plate.transform.localPosition);

        if (distanceToPan >= 0.5f)
        {
            AddReward(0.5f / m_PreviousDistanceToPan);
        }
        m_PreviousDistanceToPan = distanceToPan;

        if (distanceToEgg >= 0.5f)
        {
            if (m_MLManager.m_UncrackedEggs[0].activeSelf)
            {
                AddReward(0.5f / m_PreviousDistanceToEgg);
            }
        }
        m_PreviousDistanceToEgg = distanceToEgg;

        if (distanceToEgg >= 0.5f)
        {
            if (m_MLManager.m_CrackEggs[0].activeSelf)
            {
                AddReward(0.5f / m_PreviousDistanceToFriedEgg);
            }
        }
        m_PreviousDistanceToFriedEgg = distanceToFriedEgg;

        if (distanceToPan >= 0.5f)
        {
            AddReward(0.5f / m_PreviousDistanceToPlate);
        }
        m_PreviousDistanceToPlate = distanceToPlate;

        // m_GrabObject, 1
        bool grabObject = Mathf.Clamp(vectorAction[3], 0.0f, 1.0f) >= 0.5f ? true : false;

        m_MLManager.m_GrabObject = grabObject;

        if (m_GameState.m_DidPlayerWin)
        {
            AddReward(1.0f);
            EndEpisode();
        }
        else
        {
            AddReward(-0.005f);
        }

        if (!m_IsLive)
        { 
   /*     {
            if (this.transform.localPosition.x >= 4.0f || this.transform.localPosition.x <= -4.0f)
            {
                Debug.Log("HEloo 1");
                SetReward(-1);
                if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }
                EndEpisode();
            }

            if (this.transform.localPosition.y <= 0.75f)
            {
                Debug.Log("HEloo 2");
                SetReward(-1);
                if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

                EndEpisode();
            }

            if (this.transform.localPosition.y >= 6.0f)
            {
                Debug.Log("HEloo 4");
                SetReward(-1);
                if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

                EndEpisode();
            }

            if (this.transform.localPosition.z >= 3.0f || this.transform.localPosition.z <= -3.0f)
            {
                Debug.Log("HEloo 3");
                SetReward(-1);
                if (!m_IsLive) { this.transform.localPosition = m_StartingPosition; }

                EndEpisode();
            }*/
            
        }
        
    }

    // Enable heuristics to follow controller and look at ml manager
    public override float[] Heuristic()
    {
        float[] actions = new float[4];
        actions[0] = m_FollowMe.transform.localPosition.x;
        actions[1] = m_FollowMe.transform.localPosition.y;
        actions[2] = m_FollowMe.transform.localPosition.z;

        bool grab = m_MLManager.m_GrabObject;

        Debug.Log(grab);

        actions[3] = grab ? 1.0f : 0.0f;

        return actions;
    }
}
