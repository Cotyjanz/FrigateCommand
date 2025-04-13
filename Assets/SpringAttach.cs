/* This is a hacky way to allow other rigid bodies to have physics added to them while also 
 * following the ships rotation and translation. The object but be correctly set relative to 
 * the ship on statup. 
 * 
 * This is done because simply moving the objects by overriding transforms or calling RigidBody.MovePosition
 * does not allow for other physics to be applied .... this probably still wont work for character controller
 * 
 * 
 * Heavily referenced for this code:
 * https://gafferongames.com/post/spring_physics/
 * 
 * ALso relavent: 
 * https://en.wikipedia.org/wiki/Hooke's_law
 * 
 * 
 * 
 */



using UnityEngine;

public class SpringAttach : MonoBehaviour
{
    public Transform shipTransform;
    private Rigidbody rb;

    private Vector3 offset;
    private Quaternion relativeRotation;

    // Spring-damper parameters for position correction
    // F = -kx -bv where k is the stifness of the spring and b is the 
    public float posStiffness = 100f;
    public float posDamping = 10f;

    // Spring-damper parameters for rotation correction
    public float rotStiffness = 100f;
    public float rotDamping = 10f;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        // Calculate the relative offset
        offset = shipTransform.InverseTransformPoint(transform.position);
        // Calculate the relative rotation
        relativeRotation = Quaternion.Inverse(shipTransform.rotation) * transform.rotation;
    }

    void FixedUpdate()
    {
        // local -> world with offset
        Vector3 targetPosition = shipTransform.TransformPoint(offset); //world coord target calculated from ship local coords
        Vector3 posDifference = targetPosition - transform.position;

        //Attachment using a spring: F = -kx -bv
        // in the gaffer post, velocity is calculated, PhysX makes it available to us.
        Vector3 posForce = posStiffness * posDifference - posDamping * rb.linearVelocity;
        rb.AddForce(posForce);



        Quaternion targetRotation = shipTransform.rotation * relativeRotation;

        // difference between the current rotation and the target rotation.
        Quaternion offsetRotation = targetRotation * Quaternion.Inverse(transform.rotation);


        //The following lines lifted from a google auto-generated response to a query about calculating corrective torque:
        // Convert deltaRotation to an angle-axis representation.
        offsetRotation.ToAngleAxis(out float angleDegrees, out Vector3 axis);
        if (angleDegrees > 180f) {
            angleDegrees -= 360f;
        }
        // Convert the angle to radians.
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        Vector3 angularError = axis.normalized * angleRadians;

        // Torque = -(k*theta) - (b*w) where theta is the angular displacement and w is angular velocity which is obtained from the 
        // In this case we want the corrective force and ignore the -
        Vector3 angularTorque = rotStiffness * angularError - rotDamping * rb.angularVelocity;
        rb.AddTorque(angularTorque);
    }
}