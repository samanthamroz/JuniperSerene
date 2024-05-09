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

    public bool IsPerformActionSuccessful(BattleAction action, CharacterBattleBehavior attacker) {
        if (action.targetNeeded != TargetType.NONE || action.weaponTypeNeeded != WeaponType.NONE) {
            throw new System.ArgumentException("Bad parameters for given action");
        }
        bui.ChangeActionText(action.displayName);
        switch (action.displayName) {
            case "Attacks":
                bui.RemoveActionText();
                bm.currBattleActions = attacker.character.attacksList;
                return false;
            case "Abilities":
                bui.RemoveActionText();
                bm.currBattleActions = attacker.character.abilitiesList;
                return false;
            case "Retreat":
                bm.Move();
                return true;
            case "Advance":
                bm.Move();
                return true;
            case "Surrender":
                Surrender();
                return true;
            default:
                throw new System.ArgumentException("Action name is undefined");
        }
    }
    public bool IsPerformActionSuccessful(BattleAction action, CharacterBattleBehavior attacker, CharacterBattleBehavior target) {
        if (action.targetNeeded == TargetType.NONE || action.weaponTypeNeeded != WeaponType.NONE) {
            throw new System.ArgumentException("Bad parameters for given action");
        }
        bui.ChangeActionText(action.displayName);
        switch (action.displayName) {
            default:
                throw new System.ArgumentException("Action name is undefined");
        }
    }
    public bool IsPerformActionSuccessful(BattleAction action, CharacterBattleBehavior attacker, List<CharacterBattleBehavior> targetAll, Weapon weapon) {
        /*
        if (action.targetNeeded == TargetType.PARTY || weapon.weaponType != action.weaponTypeNeeded) {
            throw new System.ArgumentException("Bad parameters for given action");
        } */
        if (!attacker.isInFront) {
            bm.Move();
        }

        bui.ChangeActionText(action.displayName);
        switch (action.displayName) {
            case "Wild Slash":
                foreach (CharacterBattleBehavior target in targetAll) {
                    StartCoroutine(BasicAttack(attacker, target, weapon));
                }
                return true;
            case "Quick Slash":
                foreach (CharacterBattleBehavior target in targetAll) {
                    StartCoroutine(BasicAttack(attacker, target, weapon));
                }
                return true;
            case "Multi-Stab":
                foreach (CharacterBattleBehavior target in targetAll) {
                    StartCoroutine(BasicAttack(attacker, target, weapon));
                }
                return true;
            case "Clobber":
                foreach (CharacterBattleBehavior target in targetAll) {
                    StartCoroutine(BasicAttack(attacker, target, weapon));
                }
                return true;
            default:
                throw new System.Exception("Action name is undefined");
        }
    }
    public bool IsPerformActionSuccessful(BattleAction action, CharacterBattleBehavior attacker, CharacterBattleBehavior target, Weapon weapon) {
        if (action.targetNeeded == TargetType.NONE || (action.weaponTypeNeeded != WeaponType.ANY && action.weaponTypeNeeded != weapon.weaponType)) {
            throw new System.ArgumentException("Bad parameters for given ");
        }

        if (!attacker.isInFront) {
            bm.Move();
        }

        bui.ChangeActionText(action.displayName);
        switch (action.displayName) {
            case "":
                bui.ChangeActionText("Basic");
                StartCoroutine(BasicAttack(attacker, target, weapon));
                return true;
            default:
                throw new System.ArgumentException("Action name is undefined");
        }
    }

    private void Surrender() {
        Debug.Log("You surrendered! Too bad!");
    }
    private IEnumerator BasicAttack(CharacterBattleBehavior attacker, CharacterBattleBehavior target, Weapon weapon) {
        //actual attack
        int[] totalDamageDone = weapon.GetBasicAttackDamage();
        foreach (int damage in totalDamageDone) {
            target.character.Hurt(damage);
        }

        //visual feedback
        bui.DrawDamageText(totalDamageDone, target.gameObject.transform.position);
        bui.UpdateHealthBar(target.character);

        ba.Attack(attacker.gameObject);
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
    }
}
