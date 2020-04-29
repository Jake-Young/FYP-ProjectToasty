using UnityEngine;
using MLAgents;

public class DroppedItemListener : MonoBehaviour
{
    public MachineLearningManager m_MLManager;
    public Agent m_CookingAgent;
    public int m_IsDroppedPoints;

    private void OnCollisionEnter(Collision collision)
    {
        // End episode and provide points  
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 11)
        {
            Debug.Log("Floor");
            //m_CookingAgent.SetReward(m_IsDroppedPoints);
            if (m_CookingAgent != null)
            {
                m_CookingAgent.EndEpisode();
            }
            else
            {
                m_MLManager.EnvironmentReset();
            }
        }
    }
}
