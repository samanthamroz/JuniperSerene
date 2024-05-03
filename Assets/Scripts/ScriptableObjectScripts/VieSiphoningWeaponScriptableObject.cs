using UnityEngine;

[CreateAssetMenu(fileName = "VieSiphoningWeaponScriptableObject", menuName = "ScriptableObjects/VieSiphoningWeapon")]
public class VieSiphoningWeapon : Weapon
{
    public override void OnValidate()
    {
        hasAugment = true;
    }
    public int[] DoBasicAttack(Character attackingCharacter) {
        return new int[] {damage};
    }
}