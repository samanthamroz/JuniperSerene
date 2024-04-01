using System.Collections.Generic;
using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnDisplay, nextTurnDisplay, playerImage, enemyImage;
    [SerializeField] private GameObject playersFront, playersBack, enemiesFront, enemiesBack;
    private List<CharacterCombatBehavior> playersInFront = new List<CharacterCombatBehavior>();
    private List<CharacterCombatBehavior> playersInBack = new List<CharacterCombatBehavior>();
    private List<CharacterCombatBehavior> enemiesInFront = new List<CharacterCombatBehavior>();
    private List<CharacterCombatBehavior> enemiesInBack = new List<CharacterCombatBehavior>();

    private List<CharacterCombatBehavior> allPlayers {
        get {
            List<CharacterCombatBehavior> aggregate = new List<CharacterCombatBehavior>();
            foreach (CharacterCombatBehavior player in playersInFront) {
                aggregate.Add(player);
            }
            foreach (CharacterCombatBehavior player in playersInBack) {
                aggregate.Add(player);
            }
            return aggregate;
        }
    }
    private List<CharacterCombatBehavior> allEnemies {
        get {
            List<CharacterCombatBehavior> aggregate = new List<CharacterCombatBehavior>();
            foreach (CharacterCombatBehavior enemy in enemiesInFront) {
                aggregate.Add(enemy);
            }
            foreach (CharacterCombatBehavior enemy in enemiesInBack) {
                aggregate.Add(enemy);
            }
            return aggregate;
        }
    }
    private List<CharacterCombatBehavior> allCharacters {
        get {
            List<CharacterCombatBehavior> aggregate = new List<CharacterCombatBehavior>();
            foreach (CharacterCombatBehavior player in allPlayers) {
                aggregate.Add(player);
            }
            foreach (CharacterCombatBehavior enemy in allEnemies) {
                aggregate.Add(enemy);
            }
            return aggregate;
        }
    }
    
    private List<CharacterCombatBehavior> currentTurn, nextTurn;
    private List<GameObject> currentTurnUI;
    private CharacterCombatBehavior curr {
        get { return currentTurn[0]; }
    }

    private BattleUIManager bui;
    private BattleAnimations ba;

    public void NewCharacterPosition(GameObject character, bool isGoingToFront) {
        if (character.GetComponent<CharacterCombatBehavior>().isPlayable) {
            if (isGoingToFront) {
                if (playersInFront.Count == 0) {
                    character.GetComponent<DoubleLinkedList>().next = character;
                    character.GetComponent<DoubleLinkedList>().prev = character;
                } else {
                    character.GetComponent<DoubleLinkedList>().next = playersInFront[0].gameObject;
                    character.GetComponent<DoubleLinkedList>().prev = playersInFront[playersInFront.Count - 1].gameObject;
                }
                character.transform.parent = playersFront.transform;
                character.transform.localPosition = new Vector3((playersInFront.Count * 1.5f),0,0);

                playersInFront.Add(character.GetComponent<CharacterCombatBehavior>());
            } else {
                if (playersInBack.Count == 0) {
                    character.GetComponent<DoubleLinkedList>().next = character;
                    character.GetComponent<DoubleLinkedList>().prev = character;
                } else {
                    character.GetComponent<DoubleLinkedList>().next = playersInBack[0].gameObject;
                    character.GetComponent<DoubleLinkedList>().prev = playersInBack[playersInBack.Count - 1].gameObject;
                }
                character.transform.SetParent(playersBack.transform, true);
                character.transform.localPosition = new Vector3((playersInBack.Count * 1.5f),0,0);

                playersInBack.Add(character.GetComponent<CharacterCombatBehavior>());
            }
        } else {
            if (isGoingToFront) {
                if (enemiesInFront.Count == 0) {
                    character.GetComponent<DoubleLinkedList>().next = character;
                    character.GetComponent<DoubleLinkedList>().prev = character;
                } else {
                    character.GetComponent<DoubleLinkedList>().next = enemiesInFront[0].gameObject;
                    character.GetComponent<DoubleLinkedList>().prev = enemiesInFront[enemiesInFront.Count - 1].gameObject;
                }
                character.transform.SetParent(enemiesFront.transform, true);
                character.transform.localPosition = new Vector3((enemiesInFront.Count * 1.5f),0,0);

                enemiesInFront.Add(character.GetComponent<CharacterCombatBehavior>());
            } else {
                if (enemiesInBack.Count == 0) {
                    character.GetComponent<DoubleLinkedList>().next = character;
                    character.GetComponent<DoubleLinkedList>().prev = character;
                } else {
                    character.GetComponent<DoubleLinkedList>().next = enemiesInBack[0].gameObject;
                    character.GetComponent<DoubleLinkedList>().prev = enemiesInBack[enemiesInBack.Count - 1].gameObject;
                }
                character.transform.SetParent(enemiesBack.transform, true);
                character.transform.localPosition = new Vector3((enemiesInBack.Count * 1.5f),0,0);
                
                enemiesInBack.Add(character.GetComponent<CharacterCombatBehavior>());
            }
        }
        character.transform.localRotation = Quaternion.Euler(0,0,0);
        character.transform.localScale = new Vector3(1,1,1);
    }

    private void UpdateCharacterPosition(GameObject character, bool isGoingToFront) {
        CharacterCombatBehavior characterCCB =  character.GetComponent<CharacterCombatBehavior>();
        List<GameObject> shiftLeft = new List<GameObject>();
        int count = 1;

        if (characterCCB.isPlayable && !isGoingToFront) {
            if (playersInFront.IndexOf(characterCCB) != playersInFront.Count - 1) {
                for (int i = (playersInFront.IndexOf(characterCCB) + 1); i < playersInFront.Count; i++) {
                    shiftLeft.Add(playersInFront[i].gameObject);
                    count++;
                }
            }
            playersInFront.RemoveRange(playersInFront.IndexOf(characterCCB), count);
        } else if (characterCCB.isPlayable && isGoingToFront) {
            if (playersInBack.IndexOf(characterCCB) != playersInBack.Count - 1) {
                for (int i = (playersInBack.IndexOf(characterCCB) + 1); i < playersInBack.Count; i++) {
                    shiftLeft.Add(playersInBack[i].gameObject);
                    count++;
                }
            }
            playersInBack.RemoveRange(playersInBack.IndexOf(characterCCB), count);
        } else if (!characterCCB.isPlayable && !isGoingToFront) {
            if (enemiesInFront.IndexOf(characterCCB) != enemiesInFront.Count - 1) {
                for (int i = (enemiesInFront.IndexOf(characterCCB) + 1); i < enemiesInFront.Count; i++) {
                    shiftLeft.Add(enemiesInFront[i].gameObject);
                    count++;
                }
            }
            enemiesInFront.RemoveRange(enemiesInFront.IndexOf(characterCCB), count);
        } else if (!characterCCB.isPlayable && isGoingToFront) {
            if (enemiesInBack.IndexOf(characterCCB) != enemiesInBack.Count - 1) {
                for (int i = (enemiesInBack.IndexOf(characterCCB) + 1); i < enemiesInBack.Count; i++) {
                    shiftLeft.Add(enemiesInBack[i].gameObject);
                    count++;
                }
            }
            enemiesInBack.RemoveRange(enemiesInBack.IndexOf(characterCCB), count);
        } else {
            throw new Exception("'unreachable' error in UpdateCharacterPosition method");
        }

        NewCharacterPosition(character, isGoingToFront);
        foreach (GameObject shiftCharacter in shiftLeft) {
            NewCharacterPosition(shiftCharacter, !isGoingToFront);
        }
    }
    /*
    Turn Management:
        StartBattle(): called at the end of Start() in the BattleInitializer script. populates char lists and nextTurn
        CreateNextTurnOrder(): called in StartBattle() and StartNextTurn(). populates nextTurn list with random ordering of chars
        StartNextTurn(): called at the end of StartBattle(). updates turn order UIs and calls StartNextAction()
        StartNextAction(): initiates an enemy attack if curr is an enemy, or waits for BattleController input if curr is a player
        EndCurrentAction(): called at the end of all action functions. checks for battle end conditions and either calls StartNextAction() or StartNextTurn()
    */
    public void StartBattle() {
        bui = gameObject.GetComponent<BattleUIManager>();
        ba = gameObject.GetComponent<BattleAnimations>();

        //initialize 1st turn order
        CreateNextTurnOrder();

        //Start first turn
        StartNextTurn();
    }
    public void CreateNextTurnOrder() {
        nextTurn = new List<CharacterCombatBehavior>();

        List<CharacterCombatBehavior> allCharactersCopy = allCharacters;
        while (allCharactersCopy.Count > 0) {
            int randIndex = UnityEngine.Random.Range(0, allCharactersCopy.Count);
            nextTurn.Add(allCharactersCopy[randIndex]);
            allCharactersCopy.RemoveAt(randIndex);
        }
    }
    public void StartNextTurn() {
        //Update/display current turn order and get/display next turn order
        currentTurn = nextTurn;
        CreateNextTurnOrder();
        bui.CreateNewTurnUI(currentTurn, nextTurn);
        
        //start first action
        StartNextAction();
    }
    public void StartNextAction() {
        //Write stuff into menu if character is playable, else disable menu and choose enemy action
        if (allEnemies.Contains(curr)) {
            bui.DisableActionMenu();
            Invoke(nameof(DoEnemyAction), ba.TurnStart(curr.gameObject));
        } else {
            bui.EnableActionMenu();
            ba.TurnStart(curr.gameObject);
            //write options into menu
            //wait for UI input
        }
    }
    public void EndCurrentAction() {
        //check for dead players
        bool battleLostFlag = false;
        foreach (CharacterCombatBehavior player in allPlayers) {
            if (player.vie <= 0) {
                battleLostFlag = true;
            }
        }

        //check for dead enemies
        HandleDeadEnemies();

        //check if either side has lost
        if (battleLostFlag) {
            Debug.Log("players lost");
        } else if (allEnemies.Count == 0) {
            Debug.Log("players won");
        } else {
            int turnEndDelay = ba.TurnEnd(curr.gameObject);
            Invoke(nameof(RemoveFromFrontOfCurrentTurnUI), turnEndDelay);
            currentTurn.RemoveAt(0);
            //if no more characters on the current turn, go to next turn
            if (currentTurn.Count == 0) {
                Invoke(nameof(StartNextTurn), turnEndDelay);
            } else {
                Invoke(nameof(StartNextAction), turnEndDelay);
            }
        }
    }

    private void RemoveFromFrontOfCurrentTurnUI() {
        bui.RemoveFromCurrentTurnUIAt(0);
    }

    public void HandleDeadEnemies() {
        List<CharacterCombatBehavior> deadEnemies = new List<CharacterCombatBehavior>();
        foreach (CharacterCombatBehavior enemy in allEnemies) {
            if (enemy.vie <= 0) {
                deadEnemies.Add(enemy);
            }
        }
        
        foreach (CharacterCombatBehavior deadEnemy in deadEnemies) {
            DestroyImmediate(deadEnemy.gameObject, false); //DO *NOT* CHANGE TO TRUE, CHECK DOCUMENTATION

            if(currentTurn.Contains(deadEnemy)) {
                bui.RemoveFromCurrentTurnUIAt(currentTurn.IndexOf(deadEnemy));
                currentTurn.Remove(deadEnemy);
            }
            bui.RemoveFromNextTurnUIAt(nextTurn.IndexOf(deadEnemy));
            nextTurn.Remove(deadEnemy);
        }

        if (enemiesInFront.Count == 0) {
            foreach (CharacterCombatBehavior enemy in enemiesInBack) {
                Move(enemy);
            }
        }
    }
    /*
    Helper Functions for BattleController's action codes
        List<GameObject> getEligibleTargets(int actionCode): returns the current list of eligible targets for the given action code
        HandleTargettedAction(int actionCode, CCB target): executes targetted actions
    */
    public List<GameObject> GetActionTargets(float actionCode) {
        bui.DisableActionMenu();
        List<GameObject> targets = new List<GameObject>();

        switch (actionCode) {
            case -1:
                break;
            case 99:
                break;
            default:
                if (allPlayers.Contains(curr)) {
                    foreach (CharacterCombatBehavior enemy in enemiesInFront) {
                        targets.Add(enemy.gameObject);
                    }
                } else {
                    foreach (CharacterCombatBehavior player in playersInFront) {
                        targets.Add(player.gameObject);
                    }
                }
                break;
        }

        return targets;
    }
    public void DoAction(float actionCode) {
        bool success = true;
        switch (actionCode) {
            case -1:
                bui.ChangeActionText("Move");
                success = Move();
                break;
            case 99:
                bui.ChangeActionText("Surrender");
                //Surrender
                break;
            default:
                throw new Exception("Action code needs target");
        }
        if (success) {
            EndCurrentAction();
        }
    }
    public void DoAction(float actionCode, GameObject target) {
        switch (actionCode) {
            case 1:
                bui.ChangeActionText("Attack");
                BasicAttack(target.GetComponent<CharacterCombatBehavior>());
                break;
            default:
                throw new Exception("Invalid action code");
        }

        EndCurrentAction();
    }
    public void DoEnemyAction() {
        bool success = false;

        CharacterCombatBehavior chosenPlayer = playersInFront[UnityEngine.Random.Range(0, playersInFront.Count)];
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand > 8) {
            bui.ChangeActionText("Move");
            success = Move();
        }
        if (!success) {
            bui.ChangeActionText("Attack");
            BasicAttack(chosenPlayer);
        }
        
        EndCurrentAction();
    }

    /*
    Action Functions:
        
    */
    private void BasicAttack(CharacterCombatBehavior target) {
        int damageDone = curr.Attack();
        ba.Attack(curr.gameObject);
        target.Hurt(damageDone);

        //Debug.Log("Player " + currentTurn[0].name + " attacks Enemy " + target.debugName);
    }
    private bool Move() {
        return Move(curr);
    }
    private bool Move(CharacterCombatBehavior moving) {
        bool success = true;
        if ((playersInFront.Contains(moving) && playersInFront.Count > 1) ||
                playersInBack.Contains(moving) ||
                (enemiesInFront.Contains(moving) && enemiesInFront.Count > 1) ||
                (enemiesInBack.Contains(moving))) {
            UpdateCharacterPosition(moving.gameObject, (enemiesInBack.Contains(moving)) || playersInBack.Contains(moving));
        } else {
            success = false;
        }
        return success;
    }
}