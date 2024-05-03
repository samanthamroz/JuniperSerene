using UnityEngine;

[CreateAssetMenu(fileName = "DoubleAttackWeaponScriptableObject", menuName = "ScriptableObjects/DoubleAttackWeapon")]
public class DoubleAttackWeapon : Weapon
{
    public override void OnValidate()
    {
        hasAugment = true;
    }
    public void SpecialAttack()
    {
        Debug.Log("Performing siphon attack!");
        // Add logic for performing double attack
    }
}