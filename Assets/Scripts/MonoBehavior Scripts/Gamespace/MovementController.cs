using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 movementInput;
    private List<Vector3> characterPositionList;
    private List<Transform> partyCharacters;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;

        partyCharacters = new();
        for (int i = 0; i < transform.childCount; i++) {
            partyCharacters.Add(transform.GetChild(i));
        }

        characterPositionList = new();
        foreach (Transform character in partyCharacters) {
            characterPositionList.Add(character.localPosition);
        }
        gameObject.GetComponent<PlayerInput>().camera.transform.position = new Vector3(partyCharacters[0].position.x, partyCharacters[0].position.y + 1f, partyCharacters[0].position.z - 2);
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero) {
            //move character 0
            partyCharacters[0].Translate(movementSpeed * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y));
            Vector3 leadPosition = partyCharacters[0].position;
            gameObject.GetComponent<PlayerInput>().camera.transform.position = new Vector3(leadPosition.x, leadPosition.y + 1f, leadPosition.z - 2);
            
            //calculate new positions for remaining characters
            characterPositionList = new() { partyCharacters[0].localPosition };
            for (int i = 1; i < partyCharacters.Count; i++) {
                characterPositionList.Add(new Vector3(characterPositionList[i - 1].x + 0.75f * -movementInput.x, partyCharacters[i].localPosition.y, characterPositionList[i - 1].z + 0.25f * -movementInput.y));
            }
            //move the remaining characters
            MoveParty();
        } else {
            LeanTween.cancelAll();
        }

        
    }

    private void MoveParty() {
        for (int i = 1; i < partyCharacters.Count; i++) {
            LeanTween.cancel(partyCharacters[i].gameObject);
            LeanTween.moveLocal(partyCharacters[i].gameObject, 
                characterPositionList[i], 
                .3f);
        }
    }
}
