using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class VRRig : MonoBehaviour
{
    public Transform head, rightHand, leftHand;

    // Update is called once per frame 
    void Update()
    {
        // Check if there are headset and hand controllers available in XR system.
        // Then set the position and rotation to the GameObjects

        // Update transforms of the components of VR rig (head and hands)
        if (XRController.leftHand != null)
        {
            Vector3 leftPos = XRController.leftHand.devicePosition.ReadValue();
            Quaternion leftRot = XRController.leftHand.deviceRotation.ReadValue();

            //leftHand.SetPositionAndRotation(leftPos, leftRot);
            leftHand.localPosition = leftPos;
            leftHand.localRotation = leftRot;
        }

        if (XRController.rightHand != null)
        {
            Vector3 rightPos = XRController.rightHand.devicePosition.ReadValue();
            Quaternion rightRot = XRController.rightHand.deviceRotation.ReadValue();

            //rightHand.SetPositionAndRotation(rightPos, rightRot);
            rightHand.localPosition = rightPos;
            rightHand.localRotation = rightRot;
        }

        // (HMD) head-mounted display => headset used with VR system
        XRHMD hmd = InputSystem.GetDevice<XRHMD>();

        if (hmd != null)
        {
            Vector3 headPos = hmd.devicePosition.ReadValue();
            Quaternion headRot = hmd.deviceRotation.ReadValue();

            //head.SetPositionAndRotation(headPos, headRot);
            head.localPosition = headPos;
            head.localRotation = headRot;
        }
    }
}
