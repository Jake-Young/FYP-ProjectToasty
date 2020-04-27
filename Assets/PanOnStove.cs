using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PanOnStove : MonoBehaviour
{
    public GrabberEnvironmentListener m_Listener;
    public Agent m_Agent;

    private bool m_PanIsOnStove = false;

    public void PanIsOnStove ()
    {
        m_PanIsOnStove = true;
        m_Listener.m_IsPanOnStove = m_PanIsOnStove;
        m_Agent.AddReward(m_Listener.m_IsPanOnStovePoints);
    }

    public void PanIsOffStove ()
    {
        m_PanIsOnStove = false;
        m_Listener.m_IsPanOnStove = m_PanIsOnStove;
    }
}
