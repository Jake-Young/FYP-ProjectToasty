using UnityEngine;
using MLAgents;

public class DroppedItemListener : MonoBehaviour
{
    public Agent m_CookingAgent;
    public int m_IsDroppedPoints;

    private void OnCollisionEnter(Collision collision)
    {
        // End episode and provide points  
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 11)
        {
            //m_CookingAgent.SetReward(m_IsDroppedPoints);
            Debug.Log("Floor");
            m_CookingAgent.EndEpisode();
        }
    }
}
