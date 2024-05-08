using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ButtonState {
    CLEAR,
    HIGHLIGHTED,
    PRESSED,
    LOCKED
}

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject overlayCanvas, currentTurnContainer, nextTurnContainer, healthBarsContainer, actionMenuButtonsContainer, tabInformationContainer;
    [SerializeField] private GameObject characterImagePrefab, healthBarPrefab, damageTextPrefab, selectionPointerPrefab, textActionMenuButtonPrefab, imageActionMenuButtonPrefab;
    private List<GameObject> pointerObjects = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Color buttonClearColor, buttonHighlightColor, buttonPressedColor, buttonLockedColor;
    private List<GameObject> currentTurnUI, nextTurnUI;

    public void CreateNewTurnUI(List<Character> currentTurn, List<Character> nextTurn) {
        //Clear display
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UI Image"))
        {
            Destroy(obj);
        }

        //display current turn
        currentTurnUI = new List<GameObject>();
        foreach (Character character in currentTurn) {
            GameObject curr;
            curr = Instantiate(characterImagePrefab, currentTurnContainer.transform.GetChild(1));

            curr.GetComponent<Image>().sprite = character.uiSprite;
            currentTurnUI.Add(curr);
        }
        
        nextTurnUI = new List<GameObject>();
        foreach (Character character in nextTurn) {
            GameObject temp;
            temp = Instantiate(characterImagePrefab, nextTurnContainer.transform.GetChild(1));

            temp.GetComponent<Image>().sprite = character.uiSprite;
            nextTurnUI.Add(temp);
        }

        if (currentTurnUI.Count > 7) {
            currentTurnContainer.GetComponent<GridLayoutGroup>().cellSize = new Vector2(700 / currentTurnUI.Count, 700 / currentTurnUI.Count);
        } else {
            currentTurnContainer.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,100);
        }

        if (nextTurnUI.Count > 7) {
            nextTurnContainer.GetComponent<GridLayoutGroup>().cellSize = new Vector2(700 / currentTurnUI.Count, 700 / currentTurnUI.Count);
        } else {
            nextTurnContainer.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,100);
        }
    }
    public void RemoveFromCurrentTurnUIAt(int index) {
        Destroy(currentTurnUI[index]);
        currentTurnUI.RemoveAt(index);
    }
    public void RemoveFromFrontOfCurrentTurnUI() {
        RemoveFromCurrentTurnUIAt(0);
    }
    public void RemoveFromNextTurnUIAt(int index) {
        Destroy(nextTurnUI[index]);
        nextTurnUI.RemoveAt(index);
    }

    public void DrawNewHealthBars(Party playerParty) {
        foreach (Character player in playerParty.partyCharacters) {
            GameObject temp = Instantiate(healthBarPrefab, healthBarsContainer.transform);
            temp.name = player.name;
            UpdateHealthBar(player);
        }
    }
    public void UpdateHealthBar(Character character) {
        //find associated health bar and update it with the character's health values
        for (int i = 0; i < healthBarsContainer.transform.childCount; i++) {
            if (healthBarsContainer.transform.GetChild(i).gameObject.name == character.name) {
                healthBarsContainer.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = character.name;
                healthBarsContainer.transform.GetChild(i).GetChild(1).GetComponent<Slider>().maxValue = character.maxHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(1).GetComponent<Slider>().value = character.currentHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = character.currentHealth + "/" + character.maxHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(2).GetComponent<Slider>().maxValue = character.maxHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(2).GetComponent<Slider>().value = character.currentVie;
                healthBarsContainer.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = character.currentVie + "/" + character.currentHealth;
            }
        }
    }

    public void EnableActionMenu() {
        actionMenuButtonsContainer.transform.parent.gameObject.SetActive(true);
    }
    public void DisableActionMenu() {
        actionMenuButtonsContainer.transform.parent.gameObject.SetActive(false);
    }
    public void DrawNewActionMenu(List<BattleAction> actions, Character character, bool isBaseMenu) {
        //Destroy all existing buttons (except basic button container)
        for (int i = actionMenuButtonsContainer.transform.childCount - 1; i >= 1; i--) {
            Destroy(actionMenuButtonsContainer.transform.GetChild(i).gameObject);
        }
        foreach (Transform child in actionMenuButtonsContainer.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        if (!isBaseMenu) {
            DrawNonBaseActionMenu(actions, character);
            return;
        }

        List<GameObject> buttonObjects = new List<GameObject>();
        List<BattleAction> basicAttackActions = new List<BattleAction>();
        List<BattleAction> otherActions = new List<BattleAction>();
        foreach (BattleAction ba in actions) {
            if (ba.displayName == "") {
                basicAttackActions.Add(ba);
            } else {
                otherActions.Add(ba);
            }
        }
        
        //Make basic attack buttons
        actionMenuButtonsContainer.transform.GetChild(0).gameObject.SetActive(true);
        if (basicAttackActions.Count == 0) {
            actionMenuButtonsContainer.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.grey;
        } else {
            GameObject basicButtonsContainer = actionMenuButtonsContainer.transform.GetChild(0).gameObject;
            basicButtonsContainer.GetComponent<Image>().color = Color.white;
            foreach (Weapon weapon in character.weaponsList) {
                buttonObjects.Add(Instantiate(imageActionMenuButtonPrefab, basicButtonsContainer.transform));
                buttonObjects[^1].GetComponent<ActionMenuButton>().associatedAction = basicAttackActions[0];
                buttonObjects[^1].GetComponent<ActionMenuButton>().associatedWeapon = weapon;
                buttonObjects[^1].transform.GetChild(0).GetComponent<Image>().sprite = weapon.sprite;
            }
        }
        
        //Make text buttons
        foreach (BattleAction action in otherActions) {
            buttonObjects.Add(Instantiate(textActionMenuButtonPrefab, actionMenuButtonsContainer.transform));
            buttonObjects[^1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = action.displayName;
            buttonObjects[^1].GetComponent<ActionMenuButton>().associatedAction = action;
        }

        //Make prev-next double-linked list
        DoubleLinkedListHelper(buttonObjects);
        for (int i = 0; i < character.weaponsList.Count; i++) {
            buttonObjects[i].GetComponent<ActionMenuButton>().prev = buttonObjects[i];
            buttonObjects[i].GetComponent<ActionMenuButton>().next = buttonObjects[character.weaponsList.Count];
        }

        //Make the left-right double-linked list
        if (buttonObjects.Count > 0) {
            buttonObjects[0].GetComponent<ActionMenuButton>().left = buttonObjects[0];
            if (buttonObjects.Count == 2) {
                buttonObjects[0].GetComponent<ActionMenuButton>().right = buttonObjects[1];
                buttonObjects[1].GetComponent<ActionMenuButton>().left = buttonObjects[0];
            } else {
                for (int i = 1; i < character.weaponsList.Count - 1; i++) {
                    buttonObjects[i - 1].GetComponent<ActionMenuButton>().right = buttonObjects[i];
                    buttonObjects[i].GetComponent<ActionMenuButton>().left = buttonObjects[i - 1];
                    buttonObjects[i].GetComponent<ActionMenuButton>().right = buttonObjects[i + 1];
                    buttonObjects[i + 1].GetComponent<ActionMenuButton>().left = buttonObjects[i];
                }
            }
            buttonObjects[character.weaponsList.Count - 1].GetComponent<ActionMenuButton>().right = buttonObjects[character.weaponsList.Count - 1];
        }

        //description text formatting
        foreach (GameObject button in buttonObjects) {
            DescriptionFormattingHelper(character, button);
        }
    }
    private void DrawNonBaseActionMenu(List<BattleAction> actions, Character character) {
        List<GameObject> buttonObjects = new List<GameObject>();
        //gameObject.GetComponent<Image>().color = Color.grey;
        actionMenuButtonsContainer.transform.GetChild(0).gameObject.SetActive(false);

        //Make text buttons
        foreach (BattleAction action in actions) {
            buttonObjects.Add(Instantiate(textActionMenuButtonPrefab, actionMenuButtonsContainer.transform));
            buttonObjects[^1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = action.displayName;
            buttonObjects[^1].GetComponent<ActionMenuButton>().associatedAction = action;
            if (action.weaponTypeNeeded != WeaponType.NONE) {
                foreach (Weapon weapon in character.weaponsList) {
                    if (weapon.weaponType == action.weaponTypeNeeded) {
                        buttonObjects[^1].GetComponent<ActionMenuButton>().associatedWeapon = weapon;
                    }
                }
            }
        }

        //Make prev-next double-linked list
        DoubleLinkedListHelper(buttonObjects);

        //description text formatting
        foreach (GameObject button in buttonObjects) {
            DescriptionFormattingHelper(character, button);
        }
    }
    private void DoubleLinkedListHelper(List<GameObject> buttonObjects) {
        buttonObjects[0].GetComponent<ActionMenuButton>().prev = buttonObjects[0];
        for (int i = 1; i < buttonObjects.Count - 1; i++) {
            buttonObjects[i - 1].GetComponent<ActionMenuButton>().next = buttonObjects[i];
            buttonObjects[i].GetComponent<ActionMenuButton>().prev = buttonObjects[i - 1];
            buttonObjects[i].GetComponent<ActionMenuButton>().next = buttonObjects[i + 1];
            buttonObjects[i + 1].GetComponent<ActionMenuButton>().prev = buttonObjects[i];
        }
        buttonObjects[^1].GetComponent<ActionMenuButton>().next = buttonObjects[^1];
    }
    private void DescriptionFormattingHelper(Character character, GameObject button) {
        BattleAction associatedAction = button.GetComponent<ActionMenuButton>().associatedAction;
        string associatedDescription = button.GetComponent<ActionMenuButton>().associatedAction.description;
        if (associatedAction.needsCharacterData) {
            associatedDescription = associatedDescription.Replace("/character/", character.name);
            associatedDescription = associatedDescription.Replace("/role/", character.role);
        }
        if (associatedAction.weaponTypeNeeded != WeaponType.NONE && associatedAction.weaponTypeNeeded != WeaponType.ANY) {
            associatedDescription = associatedDescription.Replace("/weapon/", button.GetComponent<ActionMenuButton>().associatedWeapon.name);
        }
        button.GetComponent<ActionMenuButton>().associatedDescription = associatedDescription;
    }

    public void SetButtonColor(GameObject buttonObject, ButtonState buttonState) {
        if (buttonState == ButtonState.CLEAR) {
            buttonObject.GetComponent<Image>().color = buttonClearColor;
        } else if (buttonState == ButtonState.HIGHLIGHTED) {
            buttonObject.GetComponent<Image>().color = buttonHighlightColor;
        } else if (buttonState == ButtonState.PRESSED) {
            buttonObject.GetComponent<Image>().color = buttonPressedColor;
        } else if (buttonState == ButtonState.LOCKED) {
            buttonObject.GetComponent<Image>().color = buttonLockedColor;
        }
    }
    
    public void DrawSelectionPointer(Vector3 screenCoords, bool isMainTarget) {
        int verticalOffset = 2;
        if (isMainTarget) {
            verticalOffset = 4;
        }

        GameObject pointerObj = Instantiate(selectionPointerPrefab, overlayCanvas.transform);

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(screenCoords.x, screenCoords.y + verticalOffset, screenCoords.z));
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            overlayCanvas.GetComponent<RectTransform>(), 
            screenPoint, 
            null, 
            out Vector2 canvasPos
        );
        pointerObj.transform.localPosition = canvasPos;

        if (isMainTarget) {
            pointerObjects.Insert(0, pointerObj);
        } else {
            pointerObjects.Add(pointerObj);  
        }
        
    }
    public IEnumerator RemoveAllSelectionPointers() {
        foreach (GameObject pointer in pointerObjects) {
            Destroy(pointer);
        }
        pointerObjects.Clear();
        yield return null;
    }
    public IEnumerator RemoveMainSelectionPointer() {
        Destroy(pointerObjects[0]);
        pointerObjects.RemoveAt(0);
        yield return null;
    }

    public void ChangeActionText(string newText) {
        actionText.text = "[" + newText + "]";
    }
    public void RemoveActionText() {
        actionText.text = "";
    }

    public void DrawDamageText(int[] damage, Vector3 screenCoords) {
        GameObject[] textObjs = new GameObject[damage.Length];
        for (int i = 0; i < damage.Length; i++) {
            textObjs[i] = Instantiate(damageTextPrefab, overlayCanvas.transform);
        }
        StartCoroutine(DrawDamageText(textObjs, damage, screenCoords));
    }
    private IEnumerator DrawDamageText(GameObject[] textObjs, int[] damage, Vector3 screenCoords) {
        for (int i = 0; i < damage.Length; i++) {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(screenCoords.x + .5f, screenCoords.y + 1, screenCoords.z));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                overlayCanvas.GetComponent<RectTransform>(), 
                screenPoint, 
                null, 
                out Vector2 canvasPos
            );
            textObjs[i].transform.localPosition = canvasPos;
            if (i % 2 == 1) {
                textObjs[i].transform.localPosition = new Vector3(
                    textObjs[i].transform.localPosition.x - 80, 
                    textObjs[i].transform.localPosition.y, 
                    textObjs[i].transform.localPosition.z
                );
            }
            textObjs[i].GetComponent<TextMeshProUGUI>().text = damage[i].ToString();

            RemoveDamageText(textObjs[i], gameObject.GetComponent<BattleAnimations>().DamageText(textObjs[i])); 

            // Pause for 1 second before processing the next damage
            yield return new WaitForSeconds(.2f);
        }
    }
    private IEnumerator RemoveDamageText(GameObject damageText, float delayInSeconds) {
        Destroy(damageText, delayInSeconds);
        yield return null;
    }

    public void ToggleTab(GameObject selectedAction)
    {
        if (tabInformationContainer.activeSelf) {
            tabInformationContainer.SetActive(false);
        } else {
            tabInformationContainer.SetActive(true);
            UpdateTab(selectedAction);
        }
    }
    public void UpdateTab(GameObject selectedAction) {
        string description = selectedAction.GetComponent<ActionMenuButton>().associatedDescription;
        tabInformationContainer.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = description;
    }
}
