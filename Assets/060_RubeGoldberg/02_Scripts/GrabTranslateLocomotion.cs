using UnityEngine;

public class GrabTranslateLocomotion : MonoBehaviour
{
    public Transform right;
    public float moveSpeed = 1;

    private VRInputController input;

    private Vector3 previousPosition;

    private bool isMoving;

    private void Awake()
    {
        input = GetComponent<VRInputController>();

        previousPosition = right.position;
    }

    private void Update()
    {
        if (isMoving == true)
        {
            Vector3 displacement = previousPosition - right.position;

            transform.position += displacement * moveSpeed;

            if (input.RightTrigger < 0.5f)
            {
                isMoving = false;
            }
        }
        else
        {
            if (input.RightTrigger > 0.7f)
            {
                isMoving = true;
            }
        }

        previousPosition = right.position;
    }
}
