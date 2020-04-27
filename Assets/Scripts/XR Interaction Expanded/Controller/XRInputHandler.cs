using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
[AddComponentMenu("XR/XR Input Handler")]
public class XRInputHandler : MonoBehaviour
{
    /// <summary>
    /// Can use this object to add your own functionality for input events
    /// 
    /// To Do:
    /// - Find a better home for recognising and using these objects
    /// - Create other interactor types
    /// 
    /// </summary>

    [SerializeField]
    private float m_Deadzone = 0.75f;

    private GameObject m_PreviousControllerUsed;
    private GameObject m_CurrentControllerUsed;

    public XRControllerInput m_ControllerInput;

    public GameObject m_DirectInteractor;
    public GameObject m_RayInteractor;
    public GameObject m_TeleportInteractor;

    private void Start()
    {
        m_CurrentControllerUsed = m_ControllerInput.m_Controller;
    }

    public void SwitchToDirectInteraction()
    {
        if (m_CurrentControllerUsed == m_RayInteractor)
        {
            m_PreviousControllerUsed = m_CurrentControllerUsed;
            m_PreviousControllerUsed.SetActive(false);

            m_DirectInteractor.SetActive(true);
            m_CurrentControllerUsed = m_DirectInteractor;
        } 
    }

    public void SwitchToRayInteraction()
    {
        if (m_CurrentControllerUsed == m_DirectInteractor)
        {
            m_PreviousControllerUsed = m_CurrentControllerUsed;
            m_PreviousControllerUsed.SetActive(false);

            m_RayInteractor.SetActive(true);
            m_CurrentControllerUsed = m_RayInteractor;
        }
    }

    public void SwitchTeleportInteraction(Vector2 axisPosition)
    {
        if (m_CurrentControllerUsed != m_TeleportInteractor)
        {
            if (Mathf.Abs(axisPosition.x) >= m_Deadzone || Mathf.Abs(axisPosition.y) >= m_Deadzone)
            {
                m_PreviousControllerUsed = m_CurrentControllerUsed;
                m_PreviousControllerUsed.SetActive(false);

                m_TeleportInteractor.SetActive(true);
                m_CurrentControllerUsed = m_TeleportInteractor;
            }
        }
        else if (m_CurrentControllerUsed == m_TeleportInteractor)
        {
            if (Mathf.Abs(axisPosition.magnitude) < m_Deadzone)
            {
                m_CurrentControllerUsed.SetActive(false);
                m_PreviousControllerUsed.SetActive(true);

                m_CurrentControllerUsed = m_PreviousControllerUsed;
                m_PreviousControllerUsed = m_TeleportInteractor; 
            }
        }
    }
}
