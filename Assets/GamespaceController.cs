using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamespaceController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 movementInput;
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (movementInput != Vector2.zero) {
            transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime);
        }
    }
}
