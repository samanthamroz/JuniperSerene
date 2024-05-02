using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleAnimations : MonoBehaviour
{
    //All Battle Animation methods return the number of seconds of delay needed for the animation
    public int TurnStart(GameObject character) {
        LeanTween.scale(character, new Vector3(1.35f,1.35f,1.35f), .5f).setLoopPingPong();
        return 0;
    }

    public int TurnEnd(GameObject character) {
        LeanTween.cancel(character);
        LeanTween.scale(character, new Vector3(1.25f,1.25f,1.25f), .5f);
        return 1;
    }

    public float Attack(GameObject character) {
        LeanTween.cancel(character);
        LeanTween.scale(character, new Vector3(1.35f,1.35f,1.35f), .5f);
        if (character.GetComponent<SpriteRenderer>().flipX == true) {
            LeanTween.moveLocal(character, new Vector3(
                (character.transform.localPosition.x + 0.5f),
                (character.transform.localPosition.y - 0.5f),
                0f), .2f).setRepeat(2).setLoopPingPong().setEase(LeanTweenType.easeInOutExpo);
        } else {
            LeanTween.moveLocal(character, new Vector3(
                (character.transform.localPosition.x - 0.5f),
                (character.transform.localPosition.y - 0.5f),
                0f), .2f).setRepeat(2).setLoopPingPong().setEase(LeanTweenType.easeInOutExpo);
        }

        return 1f;
    }

    public float Hurt(GameObject character) {
        LeanTween.cancel(character);
        if (character.GetComponent<SpriteRenderer>().flipX == true) {
            LeanTween.moveLocal(character, new Vector3(
                (character.transform.localPosition.x - 0.1f),
                (character.transform.localPosition.y + 0.1f),
                0f), .15f).setDelay(.1f).setRepeat(2).setLoopPingPong().setEase( LeanTweenType.easeInQuad ).setEase( LeanTweenType.easeOutExpo );
        } else {
            LeanTween.moveLocal(character, new Vector3(
                (character.transform.localPosition.x + 0.1f),
                (character.transform.localPosition.y + 0.1f),
                0f), .15f).setDelay(.1f).setRepeat(2).setLoopPingPong().setEase( LeanTweenType.easeInQuad ).setEase( LeanTweenType.easeOutExpo );
        }


        return .5f;
    }

    public int DamageText(GameObject text) {
        LeanTween.cancel(text);

        text.GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0f);
        LeanTween.value(text, fadeToBlack, new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), .2f).setDelay(.1f);
        void fadeToBlack(Color val){
            text.GetComponent<TextMeshProUGUI>().color = val;
        }

        LeanTween.moveLocal(text, new Vector3(text.transform.localPosition.x, text.transform.localPosition.y - 100f, 0f), 1f);

        LeanTween.value(text, fadeToClear, new Color(0f, 0f, 0f, 1f), new Color(0f, 0f, 0f, 0f), .2f).setDelay(.8f);
        void fadeToClear(Color val){
            text.GetComponent<TextMeshProUGUI>().color = val;
        }

        return 1;
    }
}
