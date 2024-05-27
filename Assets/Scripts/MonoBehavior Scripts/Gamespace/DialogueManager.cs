using System.Collections.Generic;
using System;
using UnityEngine;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    private List<GameObject> otherSpeakers;
    private GameObject indicatorObj;
    private GameObject dialogueTarget { get { return indicatorObj.transform.parent.gameObject; } }
	private Story currentStory;
	[SerializeField] private TextMeshProUGUI dialogueText, speakerText;

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

    //===============================================================================================
	
	void OnInteract() {
		if (dialogueTarget == null) {
			return;
		}
		
		if (currentStory == null) {
			NonPlayableChracter npc = dialogueTarget.GetComponent<CharacterBattleBehavior>().character as NonPlayableChracter;
			currentStory = npc.interactionDialogue;
		}

		string text = currentStory.Continue();
		text = text.Trim();

		CreateContentView(text);
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Read all the content until we can't continue any more
		while (currentStory.canContinue) {
			// Continue gets the next line of the story
			string text = currentStory.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		/*
		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
		*/
	}

	/*
	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}
	*/
	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		dialogueText.text = text;
	}

	/*
	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}
	*/
}
