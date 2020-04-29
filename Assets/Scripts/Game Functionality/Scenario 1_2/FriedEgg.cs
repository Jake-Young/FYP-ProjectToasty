using UnityEngine;
using MLAgents;
using TMPro;

public class FriedEgg : MonoBehaviour
{
    public enum EggState
    {
        RawEgg = 0,
        FriedEgg = 1,
        Burnt = 2,
    }

    public MachineLearningManager m_MachineLearningManager;
    public Agent m_CookingAgent;
    public int m_EggTimer = 0;
    public EggState m_CurrentEggState;
    public TMP_Text m_GameStateUI;

    [SerializeField]
    private Transform m_ParentObject;
    private Vector3 m_OriginalPositionInKitchen;

    private void Start()
    {
        m_CurrentEggState = EggState.RawEgg;
        m_OriginalPositionInKitchen = this.transform.localPosition;
    }

    private void Update()
    {
        if (m_EggTimer >= 15 && m_CurrentEggState == EggState.RawEgg)
        {
            m_CurrentEggState = EggState.FriedEgg;

            m_MachineLearningManager.m_IsFriedEgg = true;
            m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsFriedEggPoints;
            
            m_CookingAgent.AddReward(m_MachineLearningManager.m_IsFriedEggPoints);
        }
        else if (m_EggTimer >= 20 && m_CurrentEggState == EggState.FriedEgg)
        {
            m_CurrentEggState = EggState.Burnt;

            m_MachineLearningManager.m_IsEggBurnt = true;
            m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsBurntPoints;

            m_CookingAgent.AddReward(m_MachineLearningManager.m_IsBurntPoints);

            m_MachineLearningManager.m_GameState.m_IsGameOver = true;
            Debug.Log("End");
            m_CookingAgent.EndEpisode();
        }

        if (m_CurrentEggState == EggState.Burnt)
        {
            m_GameStateUI.text = "You Lose.";
        }
    }

    public void ResetEgg ()
    {
        m_EggTimer = 0;
        m_CurrentEggState = EggState.RawEgg;
        this.transform.parent = m_ParentObject;
        this.transform.localPosition = m_OriginalPositionInKitchen;
    }
}
