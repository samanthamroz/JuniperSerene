using UnityEngine;

[CreateAssetMenu(fileName = "VieSiphoningWeaponScriptableObject", menuName = "ScriptableObjects/VieSiphoningWeapon")]
public class VieSiphoningWeapon : Weapon
{
    public override void OnValidate()
    {
        hasAugment = true;
    }
    public void SpecialAttack()
    {
        Debug.Log("Performing double attack!");
        // Add logic for performing double attack
    }
}