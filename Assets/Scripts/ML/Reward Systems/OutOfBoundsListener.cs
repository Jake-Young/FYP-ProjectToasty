using UnityEngine;
using MLAgents;

public class OutOfBoundsListener : MonoBehaviour
{ 
    public MachineLearningManager m_MLManager;
    public Agent m_CookingAgent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            m_CookingAgent.SetReward(m_MLManager.m_IsOutOfBoundsPoints);
            Debug.Log("End");
            m_CookingAgent.EndEpisode();
        }
    }
}
