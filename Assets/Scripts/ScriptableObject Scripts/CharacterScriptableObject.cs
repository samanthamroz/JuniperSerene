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
    public bool isPlayable = true;

    //BATTLE DISPLAY PROPERTIES
    public new string name;
    public string role;
    public Sprite sprite, uiSprite;

    void OnValidate() {
        //called when a value is changed in the inspector
        currentHealth = maxHealth;
        currentVie = currentHealth;
    }

    public void BattleReset(bool resetHealth) {
        if (resetHealth) {
            OnValidate();
        }
    }

    public void Hurt(int damageDone) {
        currentHealth -= damageDone;
        if (currentVie > currentHealth) {
            currentVie = currentHealth;
        }
    }
}
