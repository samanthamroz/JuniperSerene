using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BattleActionsManager : MonoBehaviour
{
    BattleManager bm;
    BattleAnimations ba;
    BattleUIManager bui;

    void Awake()
    {
        ba = gameObject.GetComponent<BattleAnimations>();
        bui = gameObject.GetComponent<BattleUIManager>();
        bm = gameObject.GetComponent<BattleManager>();
    }

    public void PerformAction(BattleAction action, Character attacker) {
        if (action.needsTarget || action.needsWeapon) {
            throw new System.Exception("Needs target and/or weapon");
        }
        switch (action.displayName) {
            case "Attacks":
                Debug.Log("Show attacks");
                break;
            case "Abilities":
                Debug.Log("Show abilties");
                break;
            case "Retreat":
                bui.ChangeActionText("Retreat");
                bm.Move();
                break;
            case "Advance":
                bui.ChangeActionText("Advance");
                bm.Move();
                break;
            case "Surrender":
                Surrender();
                break;
            default:
                throw new System.Exception("Action name is undefined");
        }
    }

    public void PerformAction(BattleAction action, Character attacker, Character target) {
        if (!action.needsTarget || action.needsWeapon) {
            throw new System.Exception("Doesn't need target and/or needs weapon");
        }

        switch (action.displayName) {
            default:
                throw new System.Exception("Action name is undefined");
        }
    }

    public void PerformAction(BattleAction action, Character attacker, Character target, Weapon weapon) {
        if (!action.needsTarget || !action.needsWeapon) {
            throw new System.Exception("Doesn't need target and/or weapon");
        }
        switch (action.displayName) {
            case "":
                StartCoroutine(BasicAttack(attacker, target, weapon));
                break;
            default:
                throw new System.Exception("Action name is undefined");
        }
    }

    private void Hurt(Character target, int damageDone) {
        target.currentHealth -= damageDone;
        if (target.currentVie > target.currentHealth) {
            target.currentVie = target.currentHealth;
        }
    }

    private void Surrender() {
        bui.ChangeActionText("Surrender");
        Debug.Log("You surrendered! Too bad!");
    }

    private IEnumerator BasicAttack(Character attacker, Character target, Weapon weapon) {
        //actual attack
        int[] totalDamageDone = weapon.GetBasicAttackDamage();
        foreach (int damage in totalDamageDone) {
            Hurt(target, damage);
        }

        //visual feedback
        bui.ChangeActionText("Basic");
        bui.DrawDamageText(totalDamageDone, target.gameObject.transform.position);
        bui.UpdateHealthBar(target);

        ba.Attack(attacker.gameObject);
        /*
        ba.Hurt(target.gameObject);
        if (totalDamageDone.Length > 1) {
            for (int i = 1; i < totalDamageDone.Length; i++) {
                yield return new WaitForSeconds(0.2f);
                ba.Attack(attacker.gameObject);
                ba.Hurt(target.gameObject);
            }
        }

        yield return new WaitForSeconds(0.2f + ((totalDamageDone.Length - 1) * 0.2f));
        
        ba.FinishAttack(attacker.gameObject);
        if (totalDamageDone.Length > 1) {
            for (int i = 1; i < totalDamageDone.Length; i++) {
                yield return new WaitForSeconds(0.2f);
                ba.FinishAttack(attacker.gameObject);
            }
        }
        */
        yield return null;
    }

}
