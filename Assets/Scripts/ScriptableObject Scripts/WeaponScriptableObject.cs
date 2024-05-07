using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public int damage;
    public WeaponType weaponType;
    public enum WeaponType
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

    public virtual int[] GetBasicAttackDamage() {
        int[] damageList = {damage};
        return damageList;
    }
}