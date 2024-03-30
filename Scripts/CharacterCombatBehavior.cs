using UnityEngine;

public class CharacterCombatBehavior : MonoBehaviour
{
    [SerializeField] private int maxHealth; 
    public int currentHealth, vie;
    
    public bool isPlayable;

    public Color uiColor; //for debugging, will probably have a specific prefab with an Image for the UI here
    public string debugName;
    
    public void InitializeFromDebugPrefab(int newHealth, Color newColor, string newName) {
        maxHealth = newHealth;
        uiColor = newColor;
        debugName = newName;

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