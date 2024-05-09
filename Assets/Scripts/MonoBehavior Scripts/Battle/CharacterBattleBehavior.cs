using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatBehavior : MonoBehaviour
{
    public bool isInFront;
    //REFERENCES TO OBJECTS IN SCENE
    public Character character;
    public CharacterCombatBehavior next, prev;
}
