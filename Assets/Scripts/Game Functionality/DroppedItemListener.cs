using UnityEngine;
using MLAgents;

public class DroppedItemListener : MonoBehaviour
{
    public MachineLearningManager m_MLManager;
    public Agent m_CookingAgent;

    private void OnCollisionEnter(Collision collision)
    {
        // End episode and provide points  
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 11)
        {
            m_CookingAgent.SetReward(m_MLManager.m_IsDroppedPoints);
            Debug.Log("Floor");
            m_CookingAgent.EndEpisode();
        }
    }
}
