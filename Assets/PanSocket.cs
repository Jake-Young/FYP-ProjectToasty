using UnityEngine;

public class PanSocket : Socket
{
    public RawEgg2FriedEgg m_EggCooker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_PositiveCollisionTag)
        {
            Debug.Log(m_PositiveCollisionTag);
            other.gameObject.layer = 11;
            m_EggCooker.PanIsOnStove();
        }
        else if (m_NegativeCollisionTag != "" && other.gameObject.tag == m_NegativeCollisionTag)
        {
            m_CookingAgent.AddReward(m_MLManager.m_IsBurntPoints);
            Debug.Log("End");
            m_CookingAgent.EndEpisode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == m_PositiveCollisionTag)
        {
            m_EggCooker.PanIsOffStove();
        }
    }
}
