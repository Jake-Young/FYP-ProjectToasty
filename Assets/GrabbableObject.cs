using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public int m_GrabLimit = 0;

    public string m_GrabberTag;

    private void OnTriggerExit(Collider other)
    {
        if (m_GrabberTag == other.gameObject.tag)
        {
            if (m_GrabLimit >= 1)
            {
                m_GrabLimit -= 1;
            }
        }
    }
}
