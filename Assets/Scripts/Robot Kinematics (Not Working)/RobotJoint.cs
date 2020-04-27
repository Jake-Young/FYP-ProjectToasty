using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 m_Axis;
    public Vector3 m_StartOffset;

    public float m_MinAngle;
    public float m_MaxAngle;

    private void Awake()
    {
        m_StartOffset = transform.localPosition;
    }
}
