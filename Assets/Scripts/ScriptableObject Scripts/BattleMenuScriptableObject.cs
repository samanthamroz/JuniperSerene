using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleMenuScriptableObject", menuName = "ScriptableObjects/BattleMenu")]
public class BattleMenu : ScriptableObject
{
    public List<BattleAction> menuActions;
    public BattleAction basicBattleAction;
}