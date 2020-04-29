using UnityEngine;
using MLAgents;

public class GrabberEnvironmentListener : MonoBehaviour
{
    [Header("The Agent")]
    public Agent m_Agent;

    // Arm target values to learn from for imitation learning
    [Header("Arm Target")]
    public bool m_ArmTargetActivated = false;
    public Transform m_Target = null;

    // Forces the target to obey the limitattions of the arm.
    public bool m_IsEndEffectorAtTarget = false;

    // Interaction Values
    [Header("Interaction Values")]
    public bool m_GrabObject = false;
    public float m_GrabObjectPoints = 0.0f;

    // On collision with objects
    public float m_ObjectGrabbablePoints = 0.0f;

    [Space]
    [Header("Pan")]
    public GameObject m_Pan;
    public Transform m_PanSpawner;
    public bool m_IsPanOnStove = false;
    public float m_IsPanOnStovePoints = 0.0f;

    [Space]
    [Header("Environment Targets")]
    public Transform[] m_SpawnPoints;
    public Transform m_Worktop;
    public Transform m_Oven;
    public Transform m_Stove;

    // Win/Lose Condition Values
    [Header("Win/Lose Condition Values")]
    // Check Game State for win lose
    public GameState m_GameState;

    private void Start()
    {
        if (m_Pan != null)
        {
            RandomPanRotation();
        }
    }

    private void FixedUpdate()
    {
        if (m_IsPanOnStove)
        {
            m_GameState.m_DidPlayerWin = true;
        }
    }

    public void GrabObject()
    {
        Debug.Log("Pickup");
        if (m_IsEndEffectorAtTarget)
        {
            Debug.Log("Pickup True");
            m_GrabObject = true;
        }
    }

    public void DropObject(float grabValue)
    {
        if (grabValue < 0.2 && m_IsEndEffectorAtTarget && m_GrabObject == true)
        {
            m_GrabObject = false;
        }
    }

    public void ResetListener()
    {
        m_ArmTargetActivated = false;
        m_Target = null;
        m_IsEndEffectorAtTarget = false;

        // Interaction Values
        m_GrabObject = false;

        // Pan
        m_IsPanOnStove = false;
    }

    public void ResetGrabber()
    {
        Debug.Log("Grabber Reset");

        int randomWorktopPos = Random.Range(0, 8);

        m_Worktop.transform.localPosition = m_SpawnPoints[randomWorktopPos].localPosition;
        m_Worktop.transform.rotation = m_SpawnPoints[randomWorktopPos].rotation;

        int randomOverPos = Random.Range(0, 8);
        while (randomOverPos == randomWorktopPos)
        {
            randomOverPos = Random.Range(0, 8);
        }
        m_Oven.transform.localPosition = m_SpawnPoints[randomOverPos].localPosition;
        m_Oven.transform.rotation = m_SpawnPoints[randomOverPos].rotation;

        m_Pan.transform.parent = m_PanSpawner.transform;
        m_Pan.transform.localPosition = Vector3.zero;

        RandomPanRotation();

        m_Pan.layer = 9;

        m_GameState.ResetGameState();
        ResetListener();
    }

    private void RandomPanRotation ()
    {
        Vector3 randomRot = new Vector3(m_Pan.transform.rotation.eulerAngles.x,
                                        m_Pan.transform.rotation.eulerAngles.y * Random.Range(1.0f, 10.0f),
                                        m_Pan.transform.rotation.eulerAngles.z);

        m_Pan.transform.rotation = Quaternion.Euler(randomRot);
    }
}
