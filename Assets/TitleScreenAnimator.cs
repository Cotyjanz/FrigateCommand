using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenAnimator : MonoBehaviour
{
    public GameObject Object1;
    public float speed = 10.0f;

    public InputActionReference buttonAction; // Reference to Input Action -- The usage of "InputActionRefernce" as a type VS InputAction is not super clear in the docs but this seems to work

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

    void Update()
    {
        Object1.transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
    }

    private void ActionPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadSceneAsync("Game Scene");
    }
}





