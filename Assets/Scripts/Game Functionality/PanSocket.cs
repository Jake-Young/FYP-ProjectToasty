using UnityEngine;

public class PanSocket : Socket
{
    public PanOnStove m_PanOnStove;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_PositiveCollisionTag)
        {
            Debug.Log(m_PositiveCollisionTag);
            other.gameObject.layer = 11;
            m_PanOnStove.PanIsOnStove();
        }
        else if (m_NegativeCollisionTag != "" && other.gameObject.tag == m_NegativeCollisionTag)
        {
            if (m_CookingAgent != null)
            {
                m_CookingAgent.SetReward(-1);
                Debug.Log("End");
                m_CookingAgent.EndEpisode();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == m_PositiveCollisionTag)
        {
            m_PanOnStove.PanIsOffStove();
        }
    }
}
