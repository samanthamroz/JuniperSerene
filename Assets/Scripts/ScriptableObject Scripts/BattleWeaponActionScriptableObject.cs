using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "BattleWeaponActionScriptableObject", menuName = "ScriptableObjects/BattleWeaponAction")]
public class BattleWeaponAction : BattleAction
{
    public Weapon.WeaponType weaponTypeNeeded;

    void OnValidate() {
        needsWeapon = true;
    }
}

[CustomEditor(typeof(BattleWeaponAction))]
public class BattleWeaponActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BattleWeaponAction battleWeaponAction = (BattleWeaponAction)this.target;
        // Display a larger text area for the 'description' field
        battleWeaponAction.description = EditorGUILayout.TextArea(battleWeaponAction.description, GUILayout.Height(100));
    }
}