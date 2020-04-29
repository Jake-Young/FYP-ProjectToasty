using System.Collections;
using UnityEngine;
using MLAgents;
using TMPro;

public class CookEgg : MonoBehaviour
{
    public MachineLearningManager m_MachineLearningManager;
    public Agent m_CookingAgent;
    public FriedEgg m_FriedEgg = null; // hide this
    public TMP_Text m_EggTimerUI;
    
    private bool m_Running = false;
    private bool m_IsEggOnPan = false;

    public void StartPan()
    {
        if (!m_MachineLearningManager.m_IsPanOnStove)
        {
            return;
        }

        StartCoroutine(EggTimer());
        m_Running = true;
    }

    public void StopPan()
    {
        StopCoroutine(EggTimer());
    }

    public void AddEggIsOnPanPoints()
    {
        m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsEggOnPanPoints;
        m_CookingAgent.AddReward(m_MachineLearningManager.m_IsEggOnPanPoints);
    }

    private IEnumerator EggTimer()
    {
        while (m_Running)
        {
            if ((int)m_FriedEgg.m_CurrentEggState == 2)
            {
                // Lost Condition
                m_EggTimerUI.text = "BURNT";
                break;
            }

            yield return new WaitForSeconds(1.0f);

            if (m_FriedEgg != null)
            {
                m_FriedEgg.m_EggTimer += 1;
                m_MachineLearningManager.m_CurrentEggTime = m_FriedEgg.m_EggTimer;
            }

            m_EggTimerUI.text = m_MachineLearningManager.m_GameState.timerFormat(m_FriedEgg.m_EggTimer);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Egg" && m_FriedEgg == null)
        {
            if (!m_IsEggOnPan)
            {
                m_IsEggOnPan = true;
                m_MachineLearningManager.m_IsEggOnPan = m_IsEggOnPan;
                AddEggIsOnPanPoints();

                m_FriedEgg = other.gameObject.GetComponent<FriedEgg>();
            }
        }

        if (tag == "Egg" && m_FriedEgg != null)
        {
            if (!m_Running && m_MachineLearningManager.m_IsPanOnStove)
            {
                m_Running = true;
                StartPan();
                Debug.Log("Running: " + m_Running);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Egg" && m_FriedEgg != null)
        {
            m_IsEggOnPan = false;
            m_MachineLearningManager.m_IsEggOnPan = m_IsEggOnPan;

            m_FriedEgg = null;
        }

        if (tag == "Egg" && m_FriedEgg == null)
        {
            if (m_Running)
            {
                m_Running = false;
                StopPan();
                Debug.Log("Running: " + m_Running);
            }
        }
    }

    public void ResetEgg()
    {
        m_FriedEgg = null;
        m_IsEggOnPan = false;
        m_EggTimerUI.text = "00:00";
        m_Running = false;
        StopCoroutine(EggTimer());
    }
}
