using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private List<GameObject> otherSpeakers;
    [SerializeField] private GameObject indicatorPrefab;
    private GameObject indicatorObj;

    void Awake()
    {
        otherSpeakers = new();
    }

    void TriggerHit(GameObject other) { //Message called by children
        if (otherSpeakers.Contains(other)) {
            otherSpeakers.Remove(other);
        }
        otherSpeakers.Insert(0, other);

        Destroy(indicatorObj);
        DrawDialogueIndicator(otherSpeakers[0]);
    }
    void TriggerExit(GameObject other) { //Message called by children
        Destroy(indicatorObj);
        
        if (otherSpeakers.Count > 0) {
            otherSpeakers.RemoveAt(0);
        }
        
        if (otherSpeakers.Count > 0) {
            DrawDialogueIndicator(otherSpeakers[0]);
        } else {
            indicatorObj = null;
        }
    }

    private void DrawDialogueIndicator(GameObject other) {
        indicatorObj = Instantiate(indicatorPrefab, other.transform);

        float verticalOffset = 0.7f;

        indicatorObj.transform.localPosition = new Vector3(0, verticalOffset, 0);
    }
}
