using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    private BattleManager bm;
    private BattleUIManager bui;
    
    //Action Menu Variables
    [SerializeField] private GameObject actionMenuButtonsContainer;

    //Controller Variables
    private bool isInActionMenu, isControllerActive;

    //Selected Action Variables
    private ActionMenuButton selectedActionButton;
    private BattleAction currAction { get { return selectedActionButton.associatedAction; } }
    private BattleAction previousSelectedAction;
    private List<CharacterBattleBehavior> selectedActionTargets;
    private CharacterBattleBehavior selectedActionMainTarget;

    void Awake()
    {
        bm = gameObject.GetComponent<BattleManager>();
        bui = gameObject.GetComponent<BattleUIManager>();
    }
    public void StopController() {
        isControllerActive = false;
    }
    public void RestartController(bool usePreviousAction, bool overwritePrevious) {
        if (usePreviousAction) {
            StartCoroutine(ChoosePreviousSelectedAction(overwritePrevious));
        } else {
            StartCoroutine(ChooseNewSelectedAction(overwritePrevious));
        }
        
        isControllerActive = true;
        isInActionMenu = true;
    }
    private IEnumerator ChooseNewSelectedAction(bool overwritePrevious) {
        yield return null;
        //Reset
        selectedActionTargets = null;
        selectedActionMainTarget = null;
        selectedActionButton = null;
        
        //search for top button in menu
        if (actionMenuButtonsContainer.transform.GetChild(0).childCount > 0) {
            selectedActionButton = actionMenuButtonsContainer.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ActionMenuButton>();
        } else {
            for (int i = 0; i < actionMenuButtonsContainer.transform.childCount; i++) {
                if (selectedActionButton == null) {
                    actionMenuButtonsContainer.transform.GetChild(i).gameObject.TryGetComponent<ActionMenuButton>(out selectedActionButton);
                }
            }
            if (selectedActionButton == null) {
                throw new Exception("No valid starting button found");
            }
        }

        if (overwritePrevious) {
            previousSelectedAction = null;
        }
       
        bui.SetButtonColor(selectedActionButton.gameObject, ButtonState.HIGHLIGHTED);
        bui.UpdateTab(selectedActionButton.gameObject);
    }
    private IEnumerator ChoosePreviousSelectedAction(bool overwritePrevious) {
        yield return null;
        
        selectedActionTargets = null;
        selectedActionMainTarget = null;
        selectedActionButton = null;

        int startIndex = 0;
        if (actionMenuButtonsContainer.transform.GetChild(0).childCount > 0) {
            for (int i = 0; i < actionMenuButtonsContainer.transform.GetChild(0).childCount; i++) {
                if (actionMenuButtonsContainer.transform.GetChild(0).GetChild(i).gameObject.GetComponent<ActionMenuButton>().associatedAction == previousSelectedAction) {
                    selectedActionButton = actionMenuButtonsContainer.transform.GetChild(0).GetChild(i).gameObject.GetComponent<ActionMenuButton>();
                    break;
                }
            }
            startIndex = 1;
        } 

        ActionMenuButton temp;
        if (selectedActionButton == null) {
            for (int i = startIndex; i < actionMenuButtonsContainer.transform.childCount; i++) {
                actionMenuButtonsContainer.transform.GetChild(i).gameObject.TryGetComponent<ActionMenuButton>(out temp);
                try { 
                    if (temp.GetComponent<ActionMenuButton>().associatedAction == previousSelectedAction) {
                        selectedActionButton = temp;
                        break;
                    }
                } catch {

                }
            }
            if (selectedActionButton == null) {
                throw new Exception("No valid starting button found");
            }
        }

        if (overwritePrevious) {
            previousSelectedAction = null;
        }

        bui.SetButtonColor(selectedActionButton.gameObject, ButtonState.HIGHLIGHTED);
        bui.UpdateTab(selectedActionButton.gameObject);
    }
    
    //NAVIGATE
    private void OnNavigate(InputValue value) {
        if (!isControllerActive) {
            return;
        }

        Vector2 input = value.Get<Vector2>();

        if (isInActionMenu) {
            ActionMenuControls(input);
            gameObject.GetComponent<BattleUIManager>().UpdateTab(selectedActionButton.gameObject);
        } else {
            TargetMenuControls(input);
        }
    }
    private void ActionMenuControls(Vector2 input) {
        bui.SetButtonColor(selectedActionButton.gameObject, ButtonState.CLEAR);

        if (input.x == 0) {
            if (input.y == 1) { //up
                selectedActionButton = selectedActionButton.prev.GetComponent<ActionMenuButton>();
            } else if (input.y == -1) { //down
                selectedActionButton = selectedActionButton.next.GetComponent<ActionMenuButton>();
            }
        } else {
            if (input.x == 1) {
                if (selectedActionButton.right != null) {
                    selectedActionButton = selectedActionButton.right.GetComponent<ActionMenuButton>();
                }
            } else if (input.x == -1) {
                if (selectedActionButton.left != null) {
                    selectedActionButton = selectedActionButton.left.GetComponent<ActionMenuButton>();
                }
            }
        }

        bui.SetButtonColor(selectedActionButton.gameObject, ButtonState.HIGHLIGHTED);
    }
    private void TargetMenuControls(Vector2 input) {
        StartCoroutine(bui.RemoveAllSelectionPointers());

        if (input.x == 0) {
            if (input.y == 1) { //up
                selectedActionMainTarget = selectedActionMainTarget.prev;
            } else if (input.y == -1) { //down
                selectedActionMainTarget = selectedActionMainTarget.next;
            }
        } else {
            if (input.x == 1) { //right
                selectedActionMainTarget = selectedActionMainTarget.prev;
            } else if (input.x == -1) { //left
                selectedActionMainTarget = selectedActionMainTarget.next;
            }
        }

        foreach (CharacterBattleBehavior target in selectedActionTargets) {
            if (target != selectedActionMainTarget) {
                bui.DrawSelectionPointer(target.gameObject.transform.position, 
                    currAction.targetNeeded == TargetType.ALL || currAction.targetNeeded == TargetType.PARTY || currAction.targetNeeded == TargetType.MULTI);
            }
        }
        bui.DrawSelectionPointer(selectedActionMainTarget.gameObject.transform.position, true);
    }

    //SUBMIT
    private void OnSubmit() {
        if (!isControllerActive) {
            return;
        }

        previousSelectedAction = currAction;

        if (isInActionMenu) {
            SubmitAction();
        } else {
            SubmitTarget();
        }
    }
    private void SubmitAction() {
        if (currAction.targetNeeded == TargetType.NONE) {
            bm.DoAction(currAction);
        } else {
            selectedActionTargets = bm.GetEligibleTargets(currAction);
            selectedActionMainTarget = selectedActionTargets[0];

            foreach (CharacterBattleBehavior target in selectedActionTargets) {
                if (target != selectedActionMainTarget) {
                    bui.DrawSelectionPointer(target.gameObject.transform.position, 
                        currAction.targetNeeded == TargetType.ALL || currAction.targetNeeded == TargetType.PARTY || currAction.targetNeeded == TargetType.MULTI);
                }
            }
            bui.DrawSelectionPointer(selectedActionMainTarget.gameObject.transform.position, true);

            isInActionMenu = false;
        }
    }
    private void SubmitTarget() {
        if (selectedActionButton.associatedWeapon != null && 
                (currAction.targetNeeded == TargetType.ALL || currAction.targetNeeded == TargetType.PARTY || currAction.targetNeeded == TargetType.MULTI)) {
            bm.DoAction(currAction, selectedActionTargets, selectedActionButton.associatedWeapon);
        } else if (selectedActionButton.associatedWeapon != null) {
            bm.DoAction(currAction, selectedActionMainTarget, selectedActionButton.associatedWeapon);
        } else {
            bm.DoAction(currAction, selectedActionMainTarget);
        }

        StartCoroutine(bui.RemoveAllSelectionPointers());
        isInActionMenu = true;
    }

    //CANCEL
    private void OnCancel() {
        if (!isControllerActive) {
            return;
        }

        if (isInActionMenu) {
            CancelFromAction();
        } else {
            CancelFromTarget();
        }
    }
    
    private void CancelFromAction() {
        if (previousSelectedAction != null) {
            bm.RestartCurrActionFromBaseMenu();
        }
    }
    private void CancelFromTarget() {
        StartCoroutine(bui.RemoveAllSelectionPointers());
        bm.RestartCurrActionFromLastMenu(); //<- only works since the menus only go 1 layer deep. if you add another layer somehow, this will have to be fixed
    }

    //TAB
    private void OnTab() {
        if (!isControllerActive) {
            return;
        }

        gameObject.GetComponent<BattleUIManager>().ToggleTab(selectedActionButton.gameObject);
    }
}