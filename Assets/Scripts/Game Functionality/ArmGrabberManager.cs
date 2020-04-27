﻿using UnityEngine;
using MLAgents;

public class ArmGrabberManager : MonoBehaviour
{
    public GrabberEnvironmentListener m_Listerner;
    public Agent m_Agent;

    [SerializeField]
    private Transform m_ControllerToFollow = null;
    [SerializeField]
    private Collider m_EndEffectorCollider = null;

    // Pickup stuff
    private bool m_Attach = false;
    private bool m_Attached = false;
    private Transform m_OriginalParent = null;

    private void FixedUpdate()
    {
        if (m_Listerner.m_ArmTargetActivated)
        {
            this.transform.position = m_ControllerToFollow.position;
            this.transform.rotation = m_ControllerToFollow.rotation;
        }

        m_Attach = m_Listerner.m_GrabObject;
    }

    public void ActivateArmTarget(Transform controllerToFollow)
    {
        if (!m_Listerner.m_ArmTargetActivated)
        {
            m_ControllerToFollow = controllerToFollow;
            m_Listerner.m_Target = this.transform;
            m_Listerner.m_ArmTargetActivated = true;
        }
    }

    public void DeactivateArmTarget()
    {
        if (m_Listerner.m_ArmTargetActivated)
        {
            m_ControllerToFollow = null;
            m_Listerner.m_Target = null;
            m_Listerner.m_ArmTargetActivated = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "End Effector")
        {
            m_EndEffectorCollider = other;
            m_Listerner.m_IsEndEffectorAtTarget = true;
        }

        // Constantly add a small amount of points on each trigger stay call
        if (other.gameObject.layer == 9)
        {
            m_Agent.AddReward(m_Listerner.m_ObjectGrabbablePoints);

            if (m_Attach && m_EndEffectorCollider != null)
            {
                m_Agent.AddReward(m_Listerner.m_ObjectGrabbablePoints);

                if (!m_Attached)
                {
                    m_Agent.AddReward(m_Listerner.m_GrabObjectPoints);

                    m_OriginalParent = other.gameObject.transform.parent;

                    other.gameObject.transform.position = this.transform.position;
                    other.gameObject.transform.parent = this.transform;

                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    Debug.Log("Attached: " + this.gameObject.name);

                    m_Attached = true;
                }
            }

            if (!m_Attach && m_EndEffectorCollider != null)
            {
                if (m_Attached)
                {
                    other.gameObject.transform.parent = m_OriginalParent;

                    other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                    Debug.Log("Detached: " + this.gameObject.name);

                    m_Attached = false;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_EndEffectorCollider != null && other.gameObject.tag == "End Effector")
        {
            m_EndEffectorCollider = null;
            m_Listerner.m_IsEndEffectorAtTarget = false;
        }
    }
}
