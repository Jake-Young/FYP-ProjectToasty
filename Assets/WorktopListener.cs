using System.Collections;
using UnityEngine;
using MLAgents;

public class WorktopListener : MonoBehaviour
{
    public int m_PenaltyTimeLimit;
    public Agent m_CookingAgent;

    public bool m_StopTimer = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            m_StopTimer = false;
            StartCoroutine(OnWorktopCounter(m_PenaltyTimeLimit));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            m_StopTimer = true;
        }
    }

    private IEnumerator OnWorktopCounter(int timeLimit)
    {
        while (timeLimit >= 0)
        {
            if (m_StopTimer)
            {
                break;
            }

            //'counter' is the counter that appears to the player
            timeLimit--;
            m_CookingAgent.AddReward(-0.05f);
            Debug.Log("Time: " + timeLimit);
            yield return new WaitForSeconds(1f);
        }

        if (timeLimit == -1)
        {
            m_CookingAgent.SetReward(-1);
            Debug.Log("End");
            m_CookingAgent.EndEpisode();
        }

        yield return null;
    }
}
