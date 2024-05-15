using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamespaceController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 movementInput;
    private List<Vector3> moveToPosition;
    private bool canMove;
    private List<Transform> partyCharacters;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;

        canMove = true;

        partyCharacters = new();
        for (int i = 0; i < transform.childCount; i++) {
            partyCharacters.Add(transform.GetChild(i));
        }

        moveToPosition = new();
        foreach (Transform character in partyCharacters) {
            moveToPosition.Add(character.localPosition);
        }

        gameObject.GetComponent<PlayerInput>().camera.transform.position = new Vector3(partyCharacters[0].position.x, partyCharacters[0].position.y + 1, partyCharacters[0].position.z - 2);
    }
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnCollisionHit(bool isColliding) { //Message called by children
        if (isColliding) {
            canMove = false;
        } else {
            canMove = true;
        }
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero) {
            moveToPosition = new();

            partyCharacters[0].Translate(movementSpeed * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y));
            Vector3 leadPosition = partyCharacters[0].position;
            gameObject.GetComponent<PlayerInput>().camera.transform.position = new Vector3(leadPosition.x, leadPosition.y + 1, leadPosition.z - 2);
        }

        moveToPosition.Add(partyCharacters[0].localPosition);
        for (int i = 1; i < partyCharacters.Count; i++) {
            moveToPosition.Add(new Vector3(moveToPosition[i - 1].x + 0.75f * -movementInput.x, moveToPosition[i - 1].y, moveToPosition[i - 1].z + 0.25f * -movementInput.y));
        }

        MoveParty();
    }

    private void MoveParty() {
        for (int i = 1; i < partyCharacters.Count; i++) {
            LeanTween.cancel(partyCharacters[i].gameObject);
            LeanTween.moveLocal(partyCharacters[i].gameObject, 
                moveToPosition[i], 
                .3f);
        }
    }
}
