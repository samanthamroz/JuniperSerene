using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

class Weapon
{
    public string name;
    public int baseDamage;
}

class Character1
{
    public int maxHealth;
    public bool isPlayable;
    public List<Weapon> weapons;
    public Character1(int maxHealth, bool isPlayable) {
        this.maxHealth = maxHealth;
        this.isPlayable = isPlayable;
    }
}

public class CharacterCombatBehavior : MonoBehaviour
{
    private new string name;
    public bool isPlayable {
        get { return characterDict[name].isPlayable; }
    }
    public Sprite uiSprite {
        get {
            return Resources.Load<Sprite>(name + "_UI");
        }
    }
    private int maxHealth {
        get { return characterDict[name].maxHealth; }
    } 
    public int currentHealth, vie;

    private Dictionary<string, Character1> characterDict = new Dictionary<string, Character1>() {
        {"Juniper", new Character1(800, true)},
        {"Lenoir", new Character1(1200, true)},
        {"Solai", new Character1(1800, true)},
        {"Luwan", new Character1(1400, true)},
        {"Willow", new Character1(1500, true)},
        {"Moogle", new Character1(1200, false)},
        {"Patches", new Character1(2000, false)},
        {"OldMan", new Character1(10, false)}
    };

    public void InitCharacter(string initName) {
        name = initName;

        currentHealth = maxHealth;
        vie = currentHealth;
    }
    
    public void Hurt(int damageDone) {
        currentHealth -= damageDone;
        if (vie > currentHealth) {
            vie = currentHealth;
        }
    }

    public int Attack() {
        //return damage done by the character's attack
        return 10; //temp value
    }
}