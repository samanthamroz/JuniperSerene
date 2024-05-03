using UnityEngine;
using System.Collections.Generic;
using BattleSystem;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class Character : ScriptableObject
{
    //STATS
    public int maxHealth;
    public int currentHealth;
    public int currentVie;

    //INVENTORY
    public List<Weapon> weaponsList;

    //BATTLE ACTIONS
    public List<Action> attacksList;
    public List<Action> abilitiesList;
    
    //BATTLE POSITIONING
    public bool isInFront, isPlayable;

    //BATTLE DISPLAY PROPERTIES
    public new string name;
    public Sprite sprite, uiSprite;

    //REFERENCES TO OBJECTS IN SCENE
    public GameObject gameObject;
    public Character next, prev;

    void OnValidate() {
        //called when a value is changed in the inspector
        currentHealth = maxHealth;
        currentVie = currentHealth;
    }
    
    public void BattleReset(bool resetHealth) {
        if (resetHealth) {
            OnValidate();
        }
        gameObject = null;
        next = null;
        prev = null;
    }

    //Battle Functions
    public void Hurt(int damageDone) {
        currentHealth -= damageDone;
        if (currentVie > currentHealth) {
            currentVie = currentHealth;
        }
    }
    public int Attack() {
        //return damage done by the character's attack
        return 100; //temp value
    }
}