using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimations : MonoBehaviour
{
    //All Battle Animation methods return the number of seconds of delay needed for the animation
    public int TurnStart(GameObject character) {
        LeanTween.scale(character, new Vector3(1.1f,1.1f,1.1f), .5f).setLoopPingPong();
        if (character.GetComponent<CharacterCombatBehavior>().isPlayable) {
            return 0;
        } else {
            return 1;
        }
    }

    public int TurnEnd(GameObject character) {
        //LeanTween.cancel(character);
        LeanTween.scale(character, new Vector3(1f,1f,1f), .5f);
        return 1;
    }

    public float Attack(GameObject character) {
        LeanTween.cancel(character);
        LeanTween.scale(character, new Vector3(1.1f,1.1f,1.1f), .5f);
        LeanTween.moveLocal(character, new Vector3(
            (character.transform.localPosition.x - 0.5f),
            (character.transform.localPosition.y - 0.5f),
            0f), .25f).setRepeat(2).setLoopPingPong();

        return 1f;
    }
}
