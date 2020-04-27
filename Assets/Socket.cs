using UnityEngine;
using MLAgents;

public class Socket : MonoBehaviour
{
    public MachineLearningManager m_MLManager;
    public Agent m_CookingAgent;
    public PointCounter m_Points;
    public string m_PositiveCollisionTag;
    public string m_NegativeCollisionTag;
}
