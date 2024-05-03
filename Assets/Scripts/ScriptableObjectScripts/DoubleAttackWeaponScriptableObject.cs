using UnityEngine;

[CreateAssetMenu(fileName = "DoubleAttackWeaponScriptableObject", menuName = "ScriptableObjects/DoubleAttackWeapon")]
public class DoubleAttackWeapon : Weapon
{
    public override void OnValidate()
    {
        hasAugment = true;
    }
    public override int DoBasicAttack() {
        Debug.Log("Double attack");
        return damage*2;
    }
}