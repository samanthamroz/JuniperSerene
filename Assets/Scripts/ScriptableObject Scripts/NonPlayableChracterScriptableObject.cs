using UnityEngine;
using Ink.Runtime;

[CreateAssetMenu(fileName = "NonPlayableCharacterScriptableObject", menuName = "ScriptableObjects/NonPlayableCharacter")]
public class NonPlayableChracter : Character
{
    [SerializeField] private TextAsset interactionActionJSON;
    [HideInInspector] public Story interactionDialogue;
    
    public void OnValidate()
    {
        currentHealth = maxHealth;
        currentVie = currentHealth;
        isPlayable = false;
        interactionDialogue = new Story(interactionActionJSON.text);
    }
}
