using UnityEngine;
using MLAgents;
using TMPro;

public class ToastyBreadLite : MonoBehaviour
{
    public enum BreadState
    {
        Bread = 0,
        Toast = 1,
        Burnt = 2,
    }

    public MachineLearningManager m_MachineLearningManager;
    public BreadState m_CurrentBreadState;

    private void Start()
    {
        m_CurrentBreadState = BreadState.Toast;
        m_MachineLearningManager.m_IsBreadToast = true;
    }

}
