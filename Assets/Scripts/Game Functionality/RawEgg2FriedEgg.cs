using System.Collections;
using UnityEngine;
using MLAgents;
using TMPro;

public class RawEgg2FriedEgg : MonoBehaviour
{
    public MachineLearningManager m_MachineLearningManager;
    public Agent m_CookingAgent;
    public FriedEgg m_FriedEgg = null;
    public TMP_Text m_EggTimerUI;
    public bool m_IsPanOnStove = false;
    public bool m_IsEggOnPan = false;

    private bool m_Running = false;
    public void StartPan() 
    {
        if (!m_IsPanOnStove)
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

    public void PanIsOnStove()
    {
        m_IsPanOnStove = true;
        m_MachineLearningManager.m_IsPanOnStove = m_IsPanOnStove;

        m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsPanOnStovePoints;
        m_CookingAgent.AddReward(m_MachineLearningManager.m_IsPanOnStovePoints);

        if (m_IsEggOnPan)
        {
            m_MachineLearningManager.m_GameState.m_GamePoints += m_MachineLearningManager.m_IsEggOnPanPoints;

            m_CookingAgent.AddReward(m_MachineLearningManager.m_IsEggOnPanPoints);
        }
    }

    public void PanIsOffStove()
    {
        m_IsPanOnStove = false;
        m_MachineLearningManager.m_IsPanOnStove = m_IsPanOnStove;

        if (m_Running)
        {
            StopCoroutine(EggTimer());
        }
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

                m_FriedEgg = other.gameObject.GetComponent<FriedEgg>();
            }
        }

        if (tag == "Egg" && m_FriedEgg != null)
        {
            if (!m_Running && m_IsPanOnStove)
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

    public void Reset2RawEgg ()
    {
        m_FriedEgg = null;
        m_IsPanOnStove = false;
        m_IsEggOnPan = false;
        m_EggTimerUI.text = "00:00";
        m_Running = false;
        StopCoroutine(EggTimer());
    }
}
