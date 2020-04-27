using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    public float m_LearningRate = 1.0f;
    public float m_SamplingDistance = 1.0f;
    public float m_DistanceThreshold = 0.5f;
    public GameObject m_Target;
    public List<RobotJoint> m_Joints = new List<RobotJoint>();

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = m_Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < m_Joints.Count; i++)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], m_Joints[i - 1].m_Axis);
            Vector3 nextPoint = prevPoint + rotation * m_Joints[i].m_StartOffset;

            prevPoint = nextPoint;
        }

        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int i) {

        float angle = angles[i];

        float f_x = DistanceFromTarget(target, angles);

        angles[i] += m_SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / m_SamplingDistance;

        angles[i] = angle;

        return gradient;
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < m_DistanceThreshold)
        {
            return;
        }
            
        for (int i = m_Joints.Count - 1; i >= 0; i--)
        {
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= m_LearningRate * gradient;

            if (DistanceFromTarget(target, angles) < m_DistanceThreshold)
            {
                return;
            }
        }
    }

}
