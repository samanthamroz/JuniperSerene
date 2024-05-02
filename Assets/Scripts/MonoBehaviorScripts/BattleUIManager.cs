using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnDisplay, nextTurnDisplay;
    public GameObject actionMenu;
    [SerializeField] private GameObject characterImage, damageText;
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
            curr = Instantiate(characterImage, currentTurnDisplay.transform);

            curr.GetComponent<Image>().sprite = character.uiSprite;
            currentTurnUI.Add(curr);
        }
        
        nextTurnUI = new List<GameObject>();
        foreach (Character character in nextTurn) {
            GameObject temp;
            temp = Instantiate(characterImage, nextTurnDisplay.transform);

            temp.GetComponent<Image>().sprite = character.uiSprite;
            nextTurnUI.Add(temp);
        }

        if (currentTurnUI.Count > 7) {
            currentTurnDisplay.GetComponent<GridLayoutGroup>().cellSize = new Vector2(700 / currentTurnUI.Count, 700 / currentTurnUI.Count);
        } else {
            currentTurnDisplay.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,100);
        }

        if (nextTurnUI.Count > 7) {
            nextTurnDisplay.GetComponent<GridLayoutGroup>().cellSize = new Vector2(700 / currentTurnUI.Count, 700 / currentTurnUI.Count);
        } else {
            nextTurnDisplay.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,100);
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
