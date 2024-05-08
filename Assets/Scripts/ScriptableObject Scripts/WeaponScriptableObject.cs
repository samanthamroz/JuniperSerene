using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public int damage;
    public WeaponType weaponType;

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

public enum WeaponType
    {
        NONE,
        ANY,
        SWORD,
        DAGGER,
        STAFF,
        POLEARM,
        BOW,
        SLING
    }