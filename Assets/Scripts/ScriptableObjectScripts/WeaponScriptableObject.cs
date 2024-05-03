using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public int damage;
    public type weaponType;
    public enum type
    {
        SWORD,
        DAGGER,
        STAFF,
        POLEARM,
        BOW,
        SLING
    }

    public bool hasAugment;
    public Sprite sprite;

    public virtual void OnValidate() {
        hasAugment = false;
    }

    public virtual int[] DoBasicAttack() {
        int[] damageList = {damage};
        return damageList;
    }
}