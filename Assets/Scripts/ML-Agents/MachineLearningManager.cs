using UnityEngine;
using UnityEditor.Animations;
using MLAgents;

public class MachineLearningManager : MonoBehaviour
{
    [Header("The Agent")]
    public Agent m_CookingAgent;

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
    // When item is picked up points
    /*public float m_PanPickupPoints = 0.0f;
    public float m_ToastPickupPoints = 0.0f;
    public float m_EggPickupPoints = 0.0f;*/

    // Environment Values
    [Header("Parent Environment")]
    public Transform m_ParentEnvrionment;

    [Space]
    [Header("Environment Targets")]
    public GameObject m_Pan;
    private Vector3 m_OriginalPanPos;
    private Quaternion m_OriginalPanRot;

    public GameObject m_Plate;
    [HideInInspector]
    private Vector3 m_OriginalPlatePos;
    private Quaternion m_OriginalPlateRot;

    public Transform m_Toaster;

    public Transform m_Stove;

    public RawEgg2FriedEgg m_RawEgg2FriedEgg;

    public GameObject[] m_UncrackedEggs;
    private Vector3[] m_OriginalUncrackedEggPos;
    private Quaternion[] m_OriginalUncrackedEggRot;

    public GameObject[] m_Breads;
    private Vector3[] m_OriginalBreadPos;
    private Quaternion[] m_OriginalBreadRot;

    public GameObject[] m_CrackEggs; // add cracked eggs to list
    private Vector3[] m_OriginalCrackedEggPos;
    private Quaternion[] m_OriginalCrackedEggRot;

    public Transform[] m_SpawnPoints;
    public Transform m_WorktopSpawner;
    public Transform m_OvenSpawner;

    [Space]
    [Header("Pan")]
    public bool m_IsPanOnStove = false;
    public float m_IsPanOnStovePoints = 0.0f;

    [Header("Toast")]
    public bool m_IsBreadInToaster = false;
    public float m_IsBreadInToasterPoints = 0.0f;

    [Space]
    public bool m_IsBreadToast = false;
    public float m_IsBreadToastPoints = 0.0f;

    [Space]
    public bool m_IsToastBurnt = false;
    public float m_CurrentToastTime = 0.0f;

    [Space]
    public bool m_IsToastOnPlate = true;
    public float m_IsToastOnPlatePoints = 0.0f;

    [Header("Egg")]
    public bool m_IsEggOnPan = false;
    public float m_IsEggOnPanPoints = 0.0f;

    [Space]
    public bool m_IsFriedEgg = false;
    public float m_IsFriedEggPoints = 0.0f;

    [Space]
    public bool m_IsEggBurnt = false;
    public float m_CurrentEggTime = 0.0f;

    [Space]
    public bool m_IsEggOnToast = false;
    public float m_IsEggOnToastPoints = 0.0f;

    [Header("Stacking Interactions")]
    public bool m_IsEggToastOnPlate = false;
    public float m_IsEggToastOnPlatePoint = 0.0f;

    // Win/Lose Condition Values
    [Header("Win/Lose Condition Values")]
    // Check Game State for win lose
    public GameState m_GameState;

    public float m_IsBurntPoints = 0.0f;
    public float m_IsDroppedPoints = 0.0f;
    public float m_IsOutOfBoundsPoints = 0.0f;

    private void Start()
    {
        if (m_Plate != null)
        {
            m_OriginalPlatePos = m_Plate.transform.position;
            m_OriginalPlateRot = m_Plate.transform.rotation;
        }

        if (m_Pan != null)
        {
            m_OriginalPanPos = m_Pan.transform.position;
            Debug.Log("OG Pan Pos 1: " + m_OriginalPanPos);
            m_OriginalPanRot = m_Pan.transform.rotation;
        }

        if (m_Breads.Length == 0)
        {
            m_OriginalBreadPos = new Vector3[2];
            m_OriginalBreadRot = new Quaternion[2];
            for (int i = 0; i < m_Breads.Length; i++)
            {
                m_OriginalBreadPos[i] = m_Breads[i].transform.position;
                m_OriginalBreadRot[i] = m_Breads[i].transform.rotation;
            }
        }

        if (m_UncrackedEggs.Length == 0)
        {
            m_OriginalUncrackedEggPos = new Vector3[2];
            m_OriginalUncrackedEggRot = new Quaternion[2];
            for (int i = 0; i < m_UncrackedEggs.Length; i++)
            {
                m_OriginalUncrackedEggPos[i] = m_UncrackedEggs[i].transform.position;
                m_OriginalUncrackedEggRot[i] = m_UncrackedEggs[i].transform.rotation;
            }
        }

        if (m_CrackEggs.Length == 0)
        {
            m_OriginalCrackedEggPos = new Vector3[2];
            m_OriginalCrackedEggRot = new Quaternion[2];
            for (int i = 0; i < m_CrackEggs.Length; i++)
            {
                m_OriginalCrackedEggPos[i] = m_CrackEggs[i].transform.position;
                m_OriginalCrackedEggRot[i] = m_CrackEggs[i].transform.rotation;
            }
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

    // Set OnGrabPoints
    public void OnGrabAddPoints ()
    {
        m_CookingAgent.AddReward(m_GrabObjectPoints);
    }

    // Set is egg on toast
    public void IsEggOnToast ()
    {
        m_IsEggOnToast = true;
        m_GameState.m_GamePoints += m_IsEggOnToastPoints;
        m_CookingAgent.AddReward(m_IsEggOnToastPoints);
    }

    public void IsEggOffToast ()
    {
        m_IsEggOnToast = false;
    }

    // Set is toast on plate
    public void IsToastOnPlate ()
    {
        m_IsToastOnPlate = true;
    }

    public void IsToastOffPlate ()
    {
        m_IsToastOnPlate = false;
    }

    // Set is Egg and Toast On Plate
    public void EggToastOnPlate ()
    {
        if (m_IsToastOnPlate)
        {
            if (m_IsEggOnToast)
            {
                m_IsEggToastOnPlate = true;
                m_GameState.m_GamePoints += m_IsEggToastOnPlatePoint;
                m_GameState.m_DidPlayerWin = true;
                m_CookingAgent.AddReward(m_IsEggToastOnPlatePoint);
                Debug.Log("Win");
                m_CookingAgent.EndEpisode();
            }
        }
    }

    public void ResetManager()
    {
        m_ArmTargetActivated = false;
        m_Target = null;
        m_IsEndEffectorAtTarget = false;

        // Interaction Values
        m_GrabObject = false;

        // Environment Values

        // Pan
        m_IsPanOnStove = false;
        // Toast
        m_IsBreadInToaster = false;
        m_IsBreadToast = false;
        m_IsToastBurnt = false;
        m_CurrentToastTime = 0.0f;
        m_IsToastOnPlate = true;
        // Egg
        m_IsEggOnPan = false;
        m_IsFriedEgg = false;
        m_IsEggBurnt = false;
        m_CurrentEggTime = 0.0f;
        m_IsEggOnToast = false;
        // Stacking Interactions
        m_IsEggToastOnPlate = false;
    }

    public void EnvironmentReset ()
    {
        Debug.Log("Environment Reset");

        m_RawEgg2FriedEgg.Reset2RawEgg();

        foreach (var egg in m_CrackEggs)
        {
            egg.GetComponent<FriedEgg>().ResetEgg();
            egg.gameObject.SetActive(false);
        }

        foreach (var uncrackedEgg in m_UncrackedEggs)
        {
            uncrackedEgg.SetActive(true);
            uncrackedEgg.GetComponent<CrackEgg>().ResetUncrackedEgg();
        }

        m_Pan.GetComponent<Rigidbody>().isKinematic = true;
        m_Pan.transform.parent = m_ParentEnvrionment;
        m_Pan.transform.position = m_OriginalPanPos;
        m_Pan.transform.rotation = m_OriginalPanRot;
        m_Pan.layer = 9;
        
        m_GameState.ResetGameState();
        ResetManager();
    }

    public void ResetGrabber()
    {
        Debug.Log("Grabber Reset");

        int randomNum = Random.Range(0, 8);

        m_WorktopSpawner.transform.localPosition = m_SpawnPoints[randomNum].localPosition;
        m_WorktopSpawner.transform.rotation = m_SpawnPoints[randomNum].rotation;

        m_OvenSpawner.transform.localPosition = m_SpawnPoints[randomNum].localPosition;
        m_OvenSpawner.transform.rotation = m_SpawnPoints[randomNum].rotation;

        m_Pan.transform.parent = m_ParentEnvrionment;
        m_Pan.transform.position = m_OriginalPanPos;

        Vector3 randomRot = new Vector3(m_OriginalPanRot.eulerAngles.x,
                                        m_OriginalPanRot.eulerAngles.y * Random.Range(1.0f, 10.0f),
                                        m_OriginalPanRot.eulerAngles.z);

        m_Pan.transform.rotation = Quaternion.Euler(randomRot);
        m_Pan.layer = 9;

        m_GameState.ResetGameState();
        ResetManager();
    }
}
