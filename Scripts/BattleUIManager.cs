using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnDisplay, nextTurnDisplay, actionMenu;
    [SerializeField] private GameObject playerImage, enemyImage;
    [SerializeField] private TextMeshProUGUI actionText;
    private List<GameObject> currentTurnUI, nextTurnUI;

    public void CreateNewTurnUI(List<CharacterCombatBehavior> currentTurn, List<CharacterCombatBehavior> nextTurn) {
        //Clear display
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UI Image"))
        {
            Destroy(obj);
        }

        //display current turn
        currentTurnUI = new List<GameObject>();
        foreach (CharacterCombatBehavior character in currentTurn) {
            GameObject curr;
            if (character.isPlayable) {
                curr = Instantiate(playerImage, currentTurnDisplay.transform);
            } else {
                curr = Instantiate(enemyImage, currentTurnDisplay.transform);
            }
            curr.GetComponent<Image>().color = character.uiColor;
            currentTurnUI.Add(curr);
        }
        
        nextTurnUI = new List<GameObject>();
        foreach (CharacterCombatBehavior character in nextTurn) {
            GameObject temp;
            if (character.isPlayable) {
                temp = Instantiate(playerImage, nextTurnDisplay.transform);
            } else {
                temp = Instantiate(enemyImage, nextTurnDisplay.transform);
            }
            temp.GetComponent<Image>().color = character.uiColor;
            nextTurnUI.Add(temp);
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

    public void EnableActionMenu() {
        actionMenu.SetActive(true);
    }
    public void DisableActionMenu() {
        actionMenu.SetActive(false);
    }
    
    public void ChangeActionText(string newText) {
        actionText.text = "[" + newText + "]";
    }
    public void RemoveActionText() {
        actionText.text = "";
    }
}
