using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInteraction : MonoBehaviour
{
    public Transform grabOrigin;
    public float grabRadius = 0.1f;

    public bool triggerPressed;

    private GrabbableObject highlightedObject;
    private GrabbableObject heldObject;

    private VRInputController input;


    // Update is called once per frame
    void Update()
    {
        // When player hand's collider is overlaping grabbable object
        // Check if player is holding object
        // If not highlight and allow grabbing
        if (heldObject != null)
        {
            if (!triggerPressed)
            {
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;

                heldObject = null;
            }
        }
        // If not, highlight and allow grabbing.
        else
        {
            if (highlightedObject != null)
            {
                highlightedObject.SetHighLight(false);
                highlightedObject = null;
            }

            // Are we hovering over any objects?
            // If so, which one?
            Collider[] cols = Physics.OverlapSphere(grabOrigin.position, grabRadius);

            // Did we hit anything at all?
            foreach (Collider col in cols)
            {
                GrabbableObject grabbable = col.GetComponent<GrabbableObject>();

                if (grabbable != null)
                {
                    // Grab the object if the user wants to (i.e., presses the trigger).
                    if (triggerPressed)
                    {
                        heldObject = grabbable;

                        heldObject.transform.parent = transform;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    else
                    {
                        highlightedObject = grabbable;
                        highlightedObject.SetHighLight(true);
                    }

                    // Exit the loop, we've found something to grab!
                    break;
                }
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(grabOrigin.position, grabRadius);
    }
}
