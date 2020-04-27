using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPath : MonoBehaviour
{
    public CinemachinePathBase m_PathToFollow;
    public CinemachineDollyCart m_DollyCart;
    public Transform m_Target;
    public Transform m_EndEffector;

    public int m_StartingSegment = 0;
    [Tooltip("How many segments on each side. -1 means the whole path.")]
    public int m_SearchRadius = -1;
    public int m_NumOfSegments = 10;
    public float m_RailSpeed = 2.0f;

    private void FixedUpdate()
    {
        float closestPathPos = m_PathToFollow.FindClosestPoint(m_Target.position, m_StartingSegment, m_SearchRadius, m_NumOfSegments);
        float distanceFromMaxPos = m_PathToFollow.MaxPos - m_DollyCart.m_Position;

        // Mid point of path based on max length
        float midPoint = m_PathToFollow.MaxPos / 2.0f;

        if (m_DollyCart.m_Position >= closestPathPos + 0.025f)
        {
            m_DollyCart.m_Position -= Time.deltaTime * m_RailSpeed;
        }
        else if (m_DollyCart.m_Position <= closestPathPos - 0.025f)
        {
            m_DollyCart.m_Position += Time.deltaTime * m_RailSpeed;
        }
        
    }
}
