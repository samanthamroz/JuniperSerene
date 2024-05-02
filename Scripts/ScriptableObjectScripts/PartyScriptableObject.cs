using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PartyScriptableObject", menuName = "ScriptableObjects/Party")]
public class Party : ScriptableObject
{
    public List<Character> partyCharacters;
}