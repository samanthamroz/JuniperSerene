using UnityEditor;
using UnityEngine;

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