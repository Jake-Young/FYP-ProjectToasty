using System.Collections;
using UnityEngine;
using MLAgents;
using TMPro;

public class Bread2Toast : MonoBehaviour
{
    public MachineLearningManager m_MachineLearningManager;
    public Agent m_CookingAgent;
    public ToastyBread m_ToastyBread = null;
    public TMP_Text m_ToasterTimerUI;

    private bool m_StopTimer = true;

    public void StartToaster ()
    {
        m_StopTimer = false;
        // Can add to ui faster here, do later
        m_MachineLearningManager.m_IsBreadInToaster = true;
        m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsBreadInToasterPoints;

        m_CookingAgent.AddReward(m_MachineLearningManager.m_IsBreadInToasterPoints);

        StartCoroutine(ToasterTimer());
    }

    public void StopToaster ()
    {
        m_StopTimer = true;

        m_MachineLearningManager.m_IsBreadInToaster = false;

        StopCoroutine(ToasterTimer());
    }

    private IEnumerator ToasterTimer ()
    {
        while (!m_StopTimer)
        {
            if ((int)m_ToastyBread.m_CurrentBreadState == 2)
            {
                // Lose Condition
                m_ToasterTimerUI.text = "BURNT";
                break;
            }

            yield return new WaitForSeconds(1.0f);

            m_ToastyBread.m_ToastTimer += 1;
            m_MachineLearningManager.m_CurrentToastTime = m_ToastyBread.m_ToastTimer;
            m_ToasterTimerUI.text = m_MachineLearningManager.m_GameState.timerFormat(m_ToastyBread.m_ToastTimer);
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if (other.gameObject.tag == "Bread" && m_ToastyBread == null)
        {
            m_ToastyBread = other.gameObject.GetComponent<ToastyBread>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_ToastyBread = null;
    }
}
