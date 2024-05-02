using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnContainer, nextTurnContainer, healthBarsContainer, actionMenu;
    [SerializeField] private GameObject characterImagePrefab, healthBarPrefab, damageText;
    [SerializeField] private TextMeshProUGUI actionText;
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

    public void WriteNewHealthBars(Party playerParty) {
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
                healthBarsContainer.transform.GetChild(i).GetChild(1).GetComponent<Slider>().value = character.currentHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = character.currentHealth + "/" + character.maxHealth;
                healthBarsContainer.transform.GetChild(i).GetChild(2).GetComponent<Slider>().value = character.currentVie;
                healthBarsContainer.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = character.currentVie + "/" + character.currentHealth;
            }
        }
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

    public void WriteDamageText(int damage, Vector3 screenCoords) {
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(screenCoords.x + .5f, screenCoords.y + 1, screenCoords.z));
        RectTransformUtility.ScreenPointToLocalPointInRectangle(damageText.transform.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);
        damageText.transform.localPosition = canvasPos;

        damageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        Invoke("RemoveDamageText", gameObject.GetComponent<BattleAnimations>().DamageText(damageText));
    }
    private void RemoveDamageText() {
        damageText.GetComponent<TextMeshProUGUI>().text = "";
    }
}
