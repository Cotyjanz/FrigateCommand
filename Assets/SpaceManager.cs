/*
 * 
 * When using this script, parent all game objects that are to be transformed under a common root. 
 * The floating origin system as implimented here moves the world around the player's ship, with everything 
 * inside the ship existing within the ship's local coordinate system. 
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class SpaceManager : MonoBehaviour
{
    //-----------------------------------------------------------------------------------
    public GameObject player;
    public GameObject activeShip;
    public Transform worldObjects;

    private Rigidbody rb;

    public float floatingOriginThreshhold = 60000f; //Start with 6000km and go from there...I think this number was used by kerbal devs.

    private static Vector3 globalOffset = Vector3.zero;

    //-----------------------------------------------------------------------------------
    void Start()
    {
        rb = activeShip.GetComponent<Rigidbody>();
  
    }
    void FixedUpdate() //Originally done in update trying fixed to make sure our physics objects correct nicely
    {
        Vector3 shipPosition = activeShip.transform.position;

        //check if we are far enough to trigger a shift
        if (shipPosition.magnitude > floatingOriginThreshhold) {
            shiftTransforms(shipPosition);
        }
    }

    void shiftTransforms(Vector3 offset)
    {
        globalOffset += offset;     //watch the signs!

        //Update objects in the game world
        foreach (Transform obj in worldObjects){
            obj.position -= offset;
        }

        //update the player's ship
        //activeShip.transform.position -= offset;
        rb.MovePosition(activeShip.transform.position - offset); //may have to uncomment above line if this doesnt work for some reason

       //TODO: Update particle positions once the space dust is implimented.

       Physics.SyncTransforms(); // correct rigid bodies and colliders on children objects
    }
}