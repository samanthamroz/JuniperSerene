using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PartyScriptableObject", menuName = "ScriptableObjects/Party")]
public class Party : ScriptableObject
{
    public List<Character> partyCharacters { 
        get { 
            List<Character> aggregate = charactersStartingInFront.GetRange(0, charactersStartingInFront.Count);
            aggregate.AddRange(charactersStartingInBack);
            return aggregate;
        }
        private set {}
    }
    public List<Character> charactersStartingInFront;
    public List<Character> charactersStartingInBack;

    public List<Character> RemoveDeadCharacters() {
        List<Character> deadCharacters = new();
        for (int i = partyCharacters.Count; i >= 0; i--) {
            if (partyCharacters[i].currentVie <= 0) {
                deadCharacters.Add(partyCharacters[i]);
                partyCharacters.RemoveAt(i);
            }
        }
        return deadCharacters;
    }
}