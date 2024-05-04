using System.Collections.Generic;
using UnityEngine;

public class ActionMenuButton : MonoBehaviour
{
    public GameObject prev, next, left, right;
    public int actionCode { get { return actionCodeDictionary[actionName]; }}
    public string actionName;
    private static readonly Dictionary<string, int> actionCodeDictionary = new Dictionary<string, int>();

    void Awake() {
        //0-9: Basic Actions
        actionCodeDictionary["Surrender"] = 0;
        actionCodeDictionary["Basic"] = 1;
        actionCodeDictionary["Attacks"] = 2;
        actionCodeDictionary["Abilities"] = 3;
        actionCodeDictionary["Item"] = 4;

        actionCodeDictionary["Move"] = 9;

        //10-19: Juniper's Attacks/Abilities


        //20-29: Lenoir's Attacks/Abilities


        //30-39: Solai's Attacks/Abilities


        //40-49: Luwan's Attacks/Abilities


        //50-59: Willow's Attacks/Abilities


        //60-99: Unassigned



        //100+: Enemy Attacks/Abilities

    }
}
