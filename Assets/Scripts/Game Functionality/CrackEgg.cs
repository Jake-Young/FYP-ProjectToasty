using UnityEngine;
using MLAgents;

public class CrackEgg : MonoBehaviour
{
    public MachineLearningManager m_MLManager;
    public Agent m_CookingAgent;
    public GameObject m_CrackedEgg;
    public Transform m_ParentPan;
    public Transform m_PanSocket;
    public Transform m_MyParent;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private void Start()
    {
        m_OriginalPosition = this.transform.localPosition;
        m_OriginalRotation = this.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pan")
        {
            Vector3 socketPos = m_PanSocket.transform.position;
            Quaternion socketRot = m_PanSocket.transform.rotation;

            m_CrackedEgg.transform.position = socketPos;
            m_CrackedEgg.transform.rotation = socketRot;
            m_CrackedEgg.transform.parent = m_ParentPan.transform;
            m_CrackedEgg.SetActive(true);

            m_CookingAgent.AddReward(m_MLManager.m_IsEggOnPanPoints);

            this.gameObject.SetActive(false);
        }
    }

    public void ResetUncrackedEgg()
    {
        this.transform.parent = m_MyParent;
        this.transform.localPosition = m_OriginalPosition;
        this.transform.rotation = m_OriginalRotation;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}
