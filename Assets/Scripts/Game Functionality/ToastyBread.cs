using UnityEngine;
using MLAgents;
using TMPro;

public class ToastyBread : MonoBehaviour
{
    public enum BreadState
    {
        Bread = 0,
        Toast = 1,
        Burnt = 2,
    }

    public MachineLearningManager m_MachineLearningManager;
    public Agent m_CookingAgent;
    public int m_ToastTimer = 0;
    public BreadState m_CurrentBreadState;
    public TMP_Text m_GameStateUI;

    private Vector3 m_OriginalPositionInKitchen;

    private void Start()
    {
        m_CurrentBreadState = BreadState.Bread;
        m_OriginalPositionInKitchen = this.transform.position;
    }

    private void Update()
    {
        if (m_ToastTimer > 10 && m_CurrentBreadState == BreadState.Bread)
        {
            m_CurrentBreadState = BreadState.Toast;

            m_MachineLearningManager.m_IsBreadToast = true;
            m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsBreadToastPoints;

            m_CookingAgent.AddReward(m_MachineLearningManager.m_IsBreadToastPoints);
        }
        else if (m_ToastTimer >= 15 && m_CurrentBreadState == BreadState.Toast)
        {
            m_CurrentBreadState = BreadState.Burnt;

            m_MachineLearningManager.m_IsToastBurnt = true;
            m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsBurntPoints; 

            m_CookingAgent.SetReward(m_MachineLearningManager.m_IsBurntPoints);

            m_MachineLearningManager.m_GameState.m_IsGameOver = true;
            Debug.Log("End");
            m_CookingAgent.EndEpisode();
        }

        if (m_CurrentBreadState == BreadState.Burnt)
        {
            m_GameStateUI.text = "You Lose.";
        }
    }

    public void ResetToast ()
    {
        m_ToastTimer = 0;
        m_CurrentBreadState = BreadState.Bread;
        this.transform.position = m_OriginalPositionInKitchen;
    }
}
