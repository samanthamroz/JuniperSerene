using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamespaceController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 movementInput;
    private bool canMove;
    private List<Transform> partyCharacters;

    void Awake()
    {
        canMove = true;

        partyCharacters = new();
        for (int i = 0; i < transform.childCount; i++) {
            partyCharacters.Add(transform.GetChild(i));
        }
    }
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnCollisionHit(bool isColliding) {
        if (isColliding) {
            canMove = false;
        } else {
            canMove = true;
        }
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero) {
            partyCharacters[0].Translate(movementSpeed * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y));
        }

        if (canMove) {
            MoveParty();
        }
    }

    private void MoveParty() {
        foreach (Transform character in partyCharacters.GetRange(1,4)) {
            character.Translate(movementSpeed * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y));
        }
    }
}
