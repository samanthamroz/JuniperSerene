using UnityEngine;


[CreateAssetMenu(fileName = "BattleActionScriptableObject", menuName = "ScriptableObjects/BattleAction")]
public class BattleAction : ScriptableObject
{
    public string displayName, description;
    public bool needsTarget, needsCharacterData, needsWeapon;
}