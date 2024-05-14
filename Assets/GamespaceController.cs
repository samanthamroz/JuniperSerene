using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamespaceController : MonoBehaviour
{
    private PlayerControls controller;
    void Awake() {
        controller = new PlayerControls();
        controller.Worldspace.Move.performed += OnMovePerformed;
        controller.Worldspace.Move.canceled += OnMoveCanceled;
    }
    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
    /*private void OnMove(InputValue value) {
        Vector2 input = value.Get<Vector2>();

        if (input.x == 0) {
            if (input.y == 1) { //up
                Debug.Log("Up");
            } else if (input.y == -1) { //down
                Debug.Log("Down");
            }
        } else {
            if (input.x == 1) { //right
                Debug.Log("Right"); 
            } else if (input.x == -1) { //left
                Debug.Log("Left");
            }
        }
    }*/

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 position = gameObject.transform.position;
        if (input != Vector2.zero)
        {
            if (input.y > 0)
            {
                gameObject.transform.position = new Vector3(position.x + 0.1f, position.y, position.z + 0.1f);
                Debug.Log("Up");
            }
            else if (input.y < 0)
            {
                Debug.Log("Down");
            }
            else if (input.x > 0)
            {
                Debug.Log("Right");
            }
            else if (input.x < 0)
            {
                Debug.Log("Left");
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        
    }
}
