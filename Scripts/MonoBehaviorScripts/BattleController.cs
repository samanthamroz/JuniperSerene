using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    private BattleManager bm;
    
    //Action Menu Variables
    [SerializeField] private GameObject selectedAction;
    public Color selectionColor;
    private float actionCode;
    private bool isInActionMenu;
    public bool isControllerActive;

    //Target Menu Variables
    private GameObject selectedTarget;
    private GameObject selectedTargetArrow {
        get {
            if (selectedTarget != null) {
                return selectedTarget.transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void StartController() {
        bm = gameObject.GetComponent<BattleManager>();

        selectedAction.GetComponent<Image>().color = selectionColor;
        isInActionMenu = true;
    }

    private void OnNavigate(InputValue value) {
        if (!isControllerActive) {
            return;
        }

        Vector2 input = value.Get<Vector2>();

        if (isInActionMenu) {
            //action menu controls
            selectedAction.GetComponent<Image>().color = new Color(1,1,1);

            if (input.x == 0) {
                if (input.y == 1) { //up
                    selectedAction = selectedAction.GetComponent<ActionMenuButton>().prev;
                } else if (input.y == -1) { //down
                    selectedAction = selectedAction.GetComponent<ActionMenuButton>().next;
                }
            } else {
                if (input.x == 1) {
                    Debug.Log("right");
                } else if (input.x == -1) {
                    Debug.Log("left");
                }
            }

            selectedAction.GetComponent<Image>().color = selectionColor;
        } else {
            //target menu controls
            selectedTargetArrow.SetActive(false);

            if (input.x == 0) {
                if (input.y == 1) { //up
                    selectedTarget = selectedTarget.GetComponent<DoubleLinkedList>().next;
                } else if (input.y == -1) { //down
                    selectedTarget = selectedTarget.GetComponent<DoubleLinkedList>().prev;
                }
            } else {
                if (input.x == 1) {
                    Debug.Log("right");
                } else if (input.x == -1) {
                    Debug.Log("left");
                }
            }

            selectedTargetArrow.SetActive(true);
        }
    }

    private void OnSubmit() {
        if (!isControllerActive) {
            return;
        }

        if (isInActionMenu) {
            //save action code selected
            actionCode = selectedAction.GetComponent<ActionMenuButton>().actionCode;

            //get eligible targets for action code
            List<GameObject> targets = bm.GetActionTargets(actionCode);
                
            //do actionCode if it does not require a target, else go to target selection mode
            if (targets.Count == 0) {
                bm.DoAction(actionCode);
            } else {
                selectedTarget = targets[0];
                selectedTargetArrow.SetActive(true);
                isInActionMenu = false;
            }
        } else {
            //do the action based on actionCode to the selected target
            bm.DoAction(actionCode, selectedTarget);

            //go back to action menu mode
            if (selectedTargetArrow != null) {
                selectedTargetArrow.SetActive(false);
            }
            selectedTarget = null;
            selectedAction.GetComponent<Image>().color = selectionColor;
            isInActionMenu = true;
        }
    }

    private void OnCancel() {
        if (!isControllerActive) {
            return;
        }

        if (!isInActionMenu) {
            //go back to action menu mode
            selectedTargetArrow.SetActive(false);
            selectedAction.GetComponent<Image>().color = selectionColor;
            isInActionMenu = true;
        }
    }
}
