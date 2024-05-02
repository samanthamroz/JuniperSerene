using UnityEngine;
using System.Collections.Generic;

namespace BattleSystem {
    [CreateAssetMenu(fileName = "ActionScriptableObject", menuName = "ScriptableObjects/Action")]
    public class Action : ScriptableObject
    {
        public new string name;
    }
}