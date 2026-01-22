using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;
        float diffAngle= Quaternion.Angle(portal.rotation, otherPortal.rotation);
        Quaternion portalDiffRotation = Quaternion.AngleAxis(diffAngle,Vector3.up);
        Vector3 newCamDir = portalDiffRotation * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCamDir,Vector3.up);
    }
}