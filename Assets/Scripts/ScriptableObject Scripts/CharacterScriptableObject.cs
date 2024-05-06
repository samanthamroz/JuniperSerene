using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class Character : ScriptableObject
{
    //STATS
    public int maxHealth, currentHealth, currentVie;

    //INVENTORY
    public List<Weapon> weaponsList;
    public List<BattleAction> attacksList;
    public List<BattleAction> abilitiesList;
    
    //BATTLE POSITIONING
    public bool isInFront, isPlayable;

    //BATTLE DISPLAY PROPERTIES
    public new string name;
    public string role;
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
}
