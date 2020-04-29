using UnityEngine;
using MLAgents;

public class PanOnStove : MonoBehaviour
{
    public MachineLearningManager m_MLManager;
    public GrabberEnvironmentListener m_Listener;
    public Agent m_Agent;

    private bool m_PanIsOnStove = false;

    public void PanIsOnStove ()
    {
        m_PanIsOnStove = true;
        if (m_MLManager != null)
        {
            m_MLManager.m_IsPanOnStove = m_PanIsOnStove;
            m_MLManager.m_GameState.m_GamePoints += m_MLManager.m_IsPanOnStovePoints;
        }
        else if (m_Listener != null) 
        {
            m_Listener.m_IsPanOnStove = m_PanIsOnStove;
            m_Listener.m_GameState.m_GamePoints += m_Listener.m_IsPanOnStovePoints;
            m_Listener.ResetGrabber();
        }
        
        if (m_Agent)
        {
            m_Agent.AddReward(m_Listener.m_IsPanOnStovePoints);
        }
    }

    public void PanIsOffStove ()
    {
        m_PanIsOnStove = false;
        m_Listener.m_IsPanOnStove = m_PanIsOnStove;
    }
}
