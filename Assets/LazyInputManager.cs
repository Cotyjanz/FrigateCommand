/* LazyInputManager.cs
 * 
 * Handle custom input actions of player here. Use the default XRI Default input actions as much as possible
 * 
 * 
 * 
 * 
 */ 
//using Unity.Android.Gradle.Manifest;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class ControllerController : MonoBehaviour
{
    public InputActionReference buttonAction; // Reference to Input Action -- The usage of "InputActionRefernce" as a type VS InputAction is not super clear in the docs but this seems to work
    public XROrigin xrorigin;
    public CharacterController playerCharController;
    public Transform xrTransform;
    public Rigidbody rb;

    private static bool isCrouched = false;
    private Vector3 verticalVelocity;
    private Vector3 previousShipPosition;




    void Start()
    {
        previousShipPosition = xrTransform.position;
    }
    private void OnEnable()
    {
        // Add callback functions to the list of callbacks for this input action
        buttonAction.action.performed += ActionPerformed;
    }

    private void OnDisable()
    {
        // Remove the callback function from the list of callbacks for the  input action
        buttonAction.action.performed -= ActionPerformed;
    }

    private void FixedUpdate()
    {
        //If the player controller is grounded, stop accumulating velocity
        /* 
        if (playerCharController.isGrounded && velocity.y < 0) {
            velocity.y = 0f;
        }

        velocity += Physics.gravity * Time.deltaTime;
        playerCharController.Move(velocity * Time.deltaTime);
        */


       // Vector3 shipGravity = -xrTransform.up * 9.8f;
      //rb.AddForce(shipGravity, ForceMode.Acceleration);
    }
    private void StartElevator()
    {

    }

    //Callback function for pressing the crouch button. Crouching is needed for working in the "obrien tubes" 
    private void ActionPerformed(InputAction.CallbackContext context)
    {
        // UPDATE THIS TO WORK WITH CAPSULE COLLIDER INSTEAD OF ChARACTER CONTROLLER
        if (!isCrouched) {
            xrorigin.CameraYOffset = 0.68072f;
            playerCharController.height = 0.68072f;
            isCrouched = true;
        } else {
            xrorigin.CameraYOffset = 1.2f;
            playerCharController.height = 1.2f;
            isCrouched = false;
        }   
    }
}





