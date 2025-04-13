/* 
 * 
 * Trying to make this work with physics was stupid and wastful given PhysX limitations
 * 
 * Leave for now but instead just parent the player to the platform and maybe turn off their gravity or something to
 * avoid rigid body shenanigans.... could also unparent the elevator from the ship hierarchy or something...
 * */





using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ElevatorController : MonoBehaviour
{  
    private Rigidbody rb;
    private float startTime;
    private bool atTop = true;
    private Vector3 newPosition;
    public bool shouldMove = false;

    public Vector3 topLocationLocal = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 bottomLocationLocal = new Vector3(0.0f, -12.88f, 0.0f);
    public float elevatorSpeed = 0.2f;
    public Transform shipTransform; //we are going to have to manually sync to the ship i guess... nothing else works consistantly

    private Vector3 topLocationGlobal;
    private Vector3 bottomLocationGlobal;

    private Vector3 offset = new Vector3(2, 0, 26);

    public XRSimpleInteractable interactable;

    void Start()
    {
        interactable.selectEntered.AddListener(OnButtonPressed);
        startTime = Time.time;
       // rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;

        // Ignore collisions between the Elevator and other Ship colliders because there is a high likelyhood of internal collision shenanigans
        int elevatorLayer = LayerMask.NameToLayer("ElevatorLayer");
        int shipLayer = LayerMask.NameToLayer("ShipLayer");

        Physics.IgnoreLayerCollision(elevatorLayer, shipLayer, true); // This can be set in the collision matrix but it is also set here as a safety measure.
    }

    private void FixedUpdate()
    {
        //topLocationGlobal = transform.TransformPoint(topLocationLocal); //local to global - remember these both change frequently 
        // bottomLocationGlobal = transform.TransformPoint(bottomLocationLocal);
        if (shouldMove && atTop) {
            moveElevatorDown();
        } else if (shouldMove && !atTop) {
            moveElevatorUp();
        }  
    }

    private void moveElevatorUp()
    {
        if (gameObject.transform.localPosition.y < topLocationLocal.y) {

            // Use MoveTowards to interpolate from the current world position to the target global position.
            // newPosition = Vector3.MoveTowards(transform.position, topLocationGlobal, Time.fixedDeltaTime * elevatorSpeed);

            //rb.MovePosition(newPosition);

            //Fallback Method -- this will keep it with the ship but is not physics safe, use to demo if nothing else works
            Vector3 newLocalPos = Vector3.MoveTowards(transform.localPosition, topLocationLocal, Time.fixedDeltaTime * elevatorSpeed);
            transform.localPosition = newLocalPos;

        } else {
            atTop = true;
            shouldMove = false;
        }
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        Debug.Log("button pressed");
        if (!shouldMove) {
            shouldMove = true;
        }
    }

    private void moveElevatorDown()
    {
        if (gameObject.transform.localPosition.y > bottomLocationLocal.y) {

           //Use MoveTowards to interpolate from the current world position to the target global position.
           //newPosition = Vector3.MoveTowards(transform.position, bottomLocationGlobal, Time.fixedDeltaTime * elevatorSpeed);

           //rb.MovePosition(newPosition);

          // Fallback Method -- this will keep it with the ship but is not physics safe, use to demo if nothing else works
           Vector3 newLocalPos = Vector3.MoveTowards(transform.localPosition, bottomLocationLocal, Time.fixedDeltaTime * elevatorSpeed);
           transform.localPosition = newLocalPos;
        }
        else {
            atTop = false;
            shouldMove = false;
        }
    }
}





