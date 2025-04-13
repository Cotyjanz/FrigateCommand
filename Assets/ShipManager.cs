/* 
 * This is intended to be added to an empty game object to represent t
 * 
 * 
 * 
 * NOTE: Most of the ships flight characteristics are mostly modelled in this script. 
 *       - Be sure to set the linear and angular dampening on the ship's rigid body to 0. Uncheck use gravity. 
 *       - velocity dampening occurs here
 *       - Rigid body may need to be applied to the object holding the exterior mesh to make the thrusters work.
 * 
 * 
 *
 */
using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;


public class ShipManager : MonoBehaviour
{
    //-----------------------------------------------------------------------------------
    public GameObject playerShip;
    public GameObject player;

    public float shipGravity = 9.0f;
    public float dampenFactor = 0.85f;  // dampening over a curve is probably nicer...

    [Range(0.2f, 3.0f)]
    public float dampenLimitMax;        //Max amount of angular velocity the ship may be alllowed to retain after dampening.
    private float dampenLimit;          // use for angular velocity we want to leave
    
    public bool stopRotation = true;    // set to false to allow some angular velocity when not in control

    private bool isHelmActive = false;
    private bool wasActive = true;

    private Rigidbody rb;
    //-----------------------------------------------------------------------------------


    void Start()
    {
        rb = playerShip.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 desiredUp = rb.transform.up;
        // Rotate the character so that its up matches the desiredUp vector
       // Quaternion targetRotation = Quaternion.FromToRotation(transform.up, desiredUp) * transform.rotation;
       // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        //set global gravity over and over with hopefully no impact. A more robust solution to per ship gravity is needed later...
        Physics.gravity = -rb.transform.up * shipGravity;

        if (!isHelmActive) {  //Dont dampen angular velocity if the helm controls are being used
            
            if (wasActive == true)
            {
                dampenLimit = UnityEngine.Random.Range(0.2f, dampenLimitMax);
                wasActive = false;
            }

            //Real Physics is boring / hard + this works for engagement from the player, also has considerations for comfort:
            // - This should eventually be updated to trigger the effect after a period of no updates from the helm console
            // when the player is not controling the ship directly, we gracefully reduce the angular velocity of the ship to 0. 
            // The threshhold can be set to stop or allow a small enough angular velocity to keep the view changing outside the windows (slowely)
            if (rb.angularVelocity.magnitude > dampenLimit)
            {
                //Re-enable this stuff when ship is no longer rigid body
                //rb.angularVelocity = rb.angularVelocity * dampenFactor;
            }
            else if (stopRotation == true)      // Make sure to set the dampen limit low if stop rotation is on, or there are abrupt stops
            {
                //rb.angularVelocity = Vector3.zero;
            }

        } else {
            wasActive = true; 
      }
        
    }
}
