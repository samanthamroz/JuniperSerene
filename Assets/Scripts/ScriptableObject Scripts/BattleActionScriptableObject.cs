using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "BattleActionScriptableObject", menuName = "ScriptableObjects/BattleAction")]
public class BattleAction : ScriptableObject
{
    public string displayName, description;
    public bool needsCharacterData;
    public bool targetsFriends = false;
    public WeaponType weaponTypeNeeded;
    public TargetType targetNeeded;
}

public enum TargetType {
    NONE, //no target needed
    SINGLE, //hits a single target
    MULTI, //hits a list of characters
    PARTY, //hits all characters of one party
    ALL //hits all characters in the battle
}


[CustomEditor(typeof(BattleAction))]
public class BattleActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BattleAction battleAction = (BattleAction)target;
        // Display a larger text area for the 'description' field
        battleAction.description = EditorGUILayout.TextArea(battleAction.description, GUILayout.Height(100));
    }
}