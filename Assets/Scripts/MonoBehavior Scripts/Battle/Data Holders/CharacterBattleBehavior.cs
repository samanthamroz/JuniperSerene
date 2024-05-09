using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleBehavior : MonoBehaviour
{
    public bool isInFront;
    //REFERENCES TO OBJECTS IN SCENE
    public Character character;
    public CharacterBattleBehavior next, prev;
}
