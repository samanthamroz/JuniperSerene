using UnityEngine;
using System.Collections.Generic;
using BattleSystem;

[CreateAssetMenu(fileName = "MenuListScriptableObject", menuName = "ScriptableObjects/MenuList")]
public class MenuList : Action
{
    public List<Action> actionList;
}