using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class Character : ScriptableObject
{
    //STATS
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] public int currentVie;

    void OnValidate() {
        //called when a value is changed in the inspector
        currentHealth = maxHealth;
        currentVie = currentHealth;
    }
    
    //BATTLE POSITIONING
    public bool isInFront, isPlayable, isHead;

    //BATTLE DISPLAY PROPERTIES
    public new string name;
    public Sprite sprite, uiSprite;

    //REFERENCES TO OBJECTS IN SCENE
    public GameObject gameObject;
    public Character next, prev;

    //Battle Functions
    public void Hurt(int damageDone) {
        currentHealth -= damageDone;
        if (currentVie > currentHealth) {
            currentVie = currentHealth;
        }
    }
    public int Attack() {
        //return damage done by the character's attack
        return 10; //temp value
    }
}