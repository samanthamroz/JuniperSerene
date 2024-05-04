using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnContainer, nextTurnContainer, healthBarsContainer, actionMenuButtonsContainer;
    private GameObject initActionMenuButtonsContainerCopy;
    [SerializeField] private GameObject characterImagePrefab, healthBarPrefab, damageTextPrefab, textActionMenuButtonPrefab, imageActionMenuButtonPrefab;
    [SerializeField] private TextMeshProUGUI actionText;
    private List<GameObject> currentTurnUI, nextTurnUI;

    void Start() {
        initActionMenuButtonsContainerCopy = actionMenuButtonsContainer;
    }
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
        actionMenuButtonsContainer.SetActive(true);
    }
    public void DisableActionMenu() {
        actionMenuButtonsContainer.SetActive(false);
    }
    public void DrawNewActionMenu(List<Weapon> weaponsList = null, String[] labelsList = null) {
        //actionMenuButtonsContainer = initActionMenuButtonsContainerCopy;

        if (weaponsList == null || weaponsList.Count == 0) {
            actionMenuButtonsContainer.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.grey;
        } else {
            GameObject basicButtonsContainer = actionMenuButtonsContainer.transform.GetChild(0).gameObject;
            basicButtonsContainer.GetComponent<Image>().color = Color.white;
            foreach (Weapon weapon in weaponsList) {
                GameObject temp = Instantiate(imageActionMenuButtonPrefab, basicButtonsContainer.transform);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = weapon.sprite;
            }
        }

        if (labelsList == null) {
            labelsList = new string[] {"Attacks", "Abilities", "Retreat", "Surrender"};
        }

        foreach (String label in labelsList) {
            GameObject temp = Instantiate(textActionMenuButtonPrefab, actionMenuButtonsContainer.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = label;
        }
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
            textObjs[i] = Instantiate(damageTextPrefab, actionMenuButtonsContainer.transform.parent.parent);
        }
        StartCoroutine(DrawDamageText(textObjs, damage, screenCoords));
    }
    private IEnumerator DrawDamageText(GameObject[] textObjs, int[] damage, Vector3 screenCoords) {
        for (int i = 0; i < damage.Length; i++) {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(screenCoords.x + .5f, screenCoords.y + 1, screenCoords.z));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                actionMenuButtonsContainer.transform.parent.parent.GetComponent<RectTransform>(), 
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
    private void RemoveDamageText(GameObject damageText, float delayInSeconds) {
        Destroy(damageText, delayInSeconds);
    }
}
