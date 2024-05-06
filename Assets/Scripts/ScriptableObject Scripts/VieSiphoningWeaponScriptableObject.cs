using UnityEngine;

[CreateAssetMenu(fileName = "VieSiphoningWeaponScriptableObject", menuName = "ScriptableObjects/VieSiphoningWeapon")]
public class VieSiphoningWeapon : Weapon
{
    public override void OnValidate()
    {
        hasAugment = true;
    }
    public override int[] GetBasicAttackDamage() {
        return new int[] {damage};
    }
}