using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerClone : MonoBehaviour
{
    public XRRig m_RigToFollow;

    // Update is called once per frame
    void Update()
    {
        Vector3 posToMoveTo = m_RigToFollow.transform.position;
        posToMoveTo *= 1.25f;
        //posToMoveTo.y += m_RigToFollow.cameraInRigSpaceHeight;
        
        this.transform.localPosition = posToMoveTo;
        this.transform.rotation = m_RigToFollow.transform.rotation;
    }
}
