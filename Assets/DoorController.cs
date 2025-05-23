using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { doorAnimator.SetTrigger("open"); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { doorAnimator.SetTrigger("closed"); }
    }

}