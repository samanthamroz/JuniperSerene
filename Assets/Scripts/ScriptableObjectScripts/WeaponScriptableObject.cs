using UnityEngine;
using System.Collections.Generic;
using System.Data.SqlTypes;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public int damage;
    public string type;
}