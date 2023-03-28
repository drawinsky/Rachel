using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class RubeGoldbergMachine : MonoBehaviour
{
    public GameObject machine; // the Rube Goldberg machine game object
    public Rigidbody startTrigger; // the trigger that starts the machine
    public bool isPaused = false; // whether the machine is currently paused
    public bool isTransparent = false; // whether the machine is currently transparent
    public float scaleMultiplier = 1.0f; // the scale multiplier for the user's view of the machine
    public float rotationMultiplier = 1.0f; // the rotation multiplier for the user's view of the machine

    void Start()
    {
        XRSettings.enabled = true; // enable XR functionality
    }

    void Update()
    {
        // check for trigger input to start the machine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTrigger.AddForce(Vector3.left * 300.0f); // apply force to the trigger to start the machine
        }

        // check for pause input
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused; // toggle pause state
            if (isPaused)
            {
                Physics.autoSimulation = false; // pause physics simulation
            }
            else
            {
                Physics.autoSimulation = true; // resume physics simulation
            }
        }

        // check for reset input
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reset machine without resetting user's view
            machine.transform.position = Vector3.zero;
            machine.transform.rotation = Quaternion.identity;
            foreach (Rigidbody rb in machine.GetComponentsInChildren<Rigidbody>())
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

         // check for toggle view input
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTransparent = !isTransparent; // toggle transparency state
            if (isTransparent)
            {
                // set machine to transparent
                foreach (Renderer renderer in machine.GetComponentsInChildren<Renderer>())
                {
                    renderer.material.SetFloat("_Mode", 2);
                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    renderer.material.SetInt("_ZWrite", 0);
                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.EnableKeyword("_ALPHABLEND_ON");
                    renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    Color color = renderer.material.color;
                    color.a = 0.5f;
                    renderer.material.color = color;
                }
            }
            else
            {
                // set machine to opaque
                foreach (Renderer renderer in machine.GetComponentsInChildren<Renderer>())
                {
                    renderer.material.SetFloat("_Mode", 0);
                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    renderer.material.SetInt("_ZWrite", 1);
                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    renderer.material.renderQueue = -1;
                    Color color = renderer.material.color;
                    color.a = 1.0f;
                    renderer.material.color = color;
                }
            }
        }

        // check for view manipulation input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Rotate");
        float scaleInput = Input.GetAxis("Scale");

        // move user's view around the world
        transform.position += new Vector3(horizontalInput, 0.0f, verticalInput) * Time.deltaTime * scaleMultiplier;
        transform.Rotate(Vector3.up, rotateInput * Time.deltaTime * rotationMultiplier, Space.World);
        transform.localScale = new Vector3(transform.localScale.x + scaleInput * Time.deltaTime * scaleMultiplier, transform.localScale.y + scaleInput * Time.deltaTime * scaleMultiplier, transform.localScale.z + scaleInput * Time.deltaTime * scaleMultiplier);
    }
}

