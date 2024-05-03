using UnityEngine;
using System.Collections.Generic;
using BattleSystem;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class Character : ScriptableObject
{
    //STATS
    public int maxHealth, currentHealth, currentVie;

    //INVENTORY
    public List<Weapon> weaponsList;

    //BATTLE ACTIONS
    public List<Action> attacksList, abilitiesList;
    
    //BATTLE POSITIONING
    public bool isInFront, isPlayable;

    //BATTLE DISPLAY PROPERTIES
    public new string name;
    public Sprite sprite, uiSprite;

    //REFERENCES TO OBJECTS IN SCENE
    [HideInInspector] public GameObject gameObject;
    [HideInInspector] public Character next, prev;

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

    public int BasicAttack(Character target, int weaponIndex) {
        int totalDamageDone = 0;
        try {
            Weapon weapon = weaponsList[weaponIndex];
            totalDamageDone = weapon.DoBasicAttack();
            target.Hurt(totalDamageDone);
        } catch (System.IndexOutOfRangeException) {
            Debug.Log("Bad weapon index for BasicAttack(). No damage awarded.");
        }
        return totalDamageDone;
    }
}