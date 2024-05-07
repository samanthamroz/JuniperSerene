using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject playersFront, playersBack, enemiesFront, enemiesBack;
    public Party enemyParty, playerParty;
    private List<Character> playersInFront = new List<Character>();
    private List<Character> playersInBack = new List<Character>();
    private List<Character> enemiesInFront = new List<Character>();
    private List<Character> enemiesInBack = new List<Character>();
    private List<Character> allCharacters {
        get {
            List<Character> aggregate = new List<Character>();
            foreach (Character player in playerParty.partyCharacters) {
                aggregate.Add(player);
            }
            foreach (Character enemy in enemyParty.partyCharacters) {
                aggregate.Add(enemy);
            }
            return aggregate;
        }
    }
    private List<Character> currentTurn, nextTurn;
    [SerializeField] public Character curr {
        get { return currentTurn[0]; }
    }
    public List<BattleAction> currBattleActions;
    public BattleMenu basicBattleMenu;
    private BattleActionsManager bam;
    private BattleUIManager bui;
    private BattleAnimations ba;
    private BattleController bc;

    public void NewCharacterPosition(Character character) {
        List<Character> listToPlace;
        GameObject containerToPlace;
        int placeModifier;

        if (character.isPlayable) {
            if (character.isInFront) {
                listToPlace = playersInFront;
                containerToPlace = playersFront;
                placeModifier = 1;
            } else {
                listToPlace = playersInBack;
                containerToPlace = playersBack;
                placeModifier = -1;
            }
        } else {
            if (character.isInFront) {
                listToPlace = enemiesInFront;
                containerToPlace = enemiesFront;
                placeModifier = -1;
            } else {
                listToPlace = enemiesInBack;
                containerToPlace = enemiesBack;
                placeModifier = 1;
            }
        }

        //sets the character's next and prev gameobjects according to what is in the scene
        if (listToPlace.Count == 0) {
            character.next = character;
            character.prev = character;
        } else {
            character.next = listToPlace[0];
            character.prev = listToPlace[listToPlace.Count - 1];
            listToPlace[listToPlace.Count - 1].next = character;
            listToPlace[0].prev = character;
        }


        //adds the character gameObject to the correct container gameObject for placement
        character.gameObject.transform.parent = containerToPlace.transform;
        character.gameObject.transform.localPosition = new Vector3((listToPlace.Count * 1.5f * placeModifier),0,0);

        //adds the character reference to the correct local list
        listToPlace.Add(character);

        character.gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
    }
    private void UpdateCharacterPosition(Character character, bool isGoingToFront) {
        List<Character> listToPlace;
        List<Character> shiftLeft = new List<Character>();

        if (character.isPlayable && !isGoingToFront) {
            listToPlace = playersInFront;
        } else if (character.isPlayable && isGoingToFront) {
            listToPlace = playersInBack;
        } else if (!character.isPlayable && !isGoingToFront) {
            listToPlace = enemiesInFront;
        } else if (!character.isPlayable && isGoingToFront) {
            listToPlace = enemiesInBack;
        } else {
            throw new Exception("some error in UpdateCharacterPosition method");
        }

        //removes the character from its current list and all characters to its "right"
        //places these characters in the shiftLeft list
        if (listToPlace.IndexOf(character) != listToPlace.Count - 1) {
                for (int i = (listToPlace.IndexOf(character) + 1); i < listToPlace.Count; i++) {
                    shiftLeft.Add(listToPlace[i]);
                }
            }
        listToPlace.RemoveRange(listToPlace.IndexOf(character), shiftLeft.Count + 1);

        //replaces the character at the "back" of the correct list and updates its property
        character.isInFront = isGoingToFront;
        NewCharacterPosition(character);

        //replaces each of the shiftLeft characters
        foreach (Character characterToShift in shiftLeft) {
            NewCharacterPosition(characterToShift);
        }
    }
    private void UpdateAllCharacterPositions() {
        //resets the local lists of positioned layers/enemies
        playersInFront = new List<Character>();
        playersInBack = new List<Character>();
        enemiesInFront = new List<Character>();
        enemiesInBack = new List<Character>();

        //repositions each character remaining in both global lists
        foreach (Character player in playerParty.partyCharacters) {
            NewCharacterPosition(player);
        }
        foreach (Character enemy in enemyParty.partyCharacters) {
            NewCharacterPosition(enemy);
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
        bam = gameObject.GetComponent<BattleActionsManager>();
        bui = gameObject.GetComponent<BattleUIManager>();
        ba = gameObject.GetComponent<BattleAnimations>();
        bc = gameObject.GetComponent<BattleController>();
        bc.StartController();

        //initialize 1st turn order
        CreateNextTurnOrder();

        //Start first turn
        StartNextTurn();
    }
    private void CreateNextTurnOrder() {
        nextTurn = new List<Character>();

        List<Character> allCharactersCopy = allCharacters;
        while (allCharactersCopy.Count > 0) {
            int randIndex = UnityEngine.Random.Range(0, allCharactersCopy.Count);
            nextTurn.Add(allCharactersCopy[randIndex]);
            allCharactersCopy.RemoveAt(randIndex);
        }
    }
    private void StartNextTurn() {
        //Update/display current turn order and get/display next turn order
        currentTurn = nextTurn;
        CreateNextTurnOrder();
        bui.CreateNewTurnUI(currentTurn, nextTurn);
        
        //start first action
        StartNextAction();
    }
    private void StartNextAction() {
        //Write stuff into menu if character is playable, else disable menu and choose enemy action
        if (enemyParty.partyCharacters.Contains(curr)) {
            bui.DisableActionMenu();
            bc.StopController();
            Invoke(nameof(DoEnemyAction), ba.TurnStart(curr.gameObject));
        } else {
            bui.EnableActionMenu();
            CreateBaseBattleMenu();
            bc.RestartController();
            ba.TurnStart(curr.gameObject);
            //write options into menu
            //wait for UI input
        }
    }
    private void ContinueCurrAction() {
        bui.EnableActionMenu();
        bc.RestartController();
    }
    private void EndCurrentAction() {
        bc.StopController();

        //check for dead players
        bool battleLostFlag = false;
        foreach (Character player in playerParty.partyCharacters) {
            if (player.currentVie <= 0) {
                battleLostFlag = true;
            }
        }

        //check for dead enemies
        HandleDeadEnemies();

        //check if either side has lost
        if (battleLostFlag) {
            Debug.Log("players lost");
        } else if (enemyParty.partyCharacters.Count == 0) {
            Debug.Log("players won");
        } else {
            //neither side has lost yet
            int turnEndDelay = ba.TurnEnd(curr.gameObject);
            bui.Invoke("RemoveFromFrontOfCurrentTurnUI", turnEndDelay);
            bui.Invoke("RemoveActionText", turnEndDelay);
            currentTurn.RemoveAt(0);
            //if no more characters on the current turn, go to next turn
            if (currentTurn.Count == 0) {
                Invoke(nameof(StartNextTurn), turnEndDelay);
            } else {
                Invoke(nameof(StartNextAction), turnEndDelay);
            }
        }
    }
    
    /*
    Turn Management Helper Functions:
    */
    private void HandleDeadEnemies() {
        //check for dead enemies
        List<Character> deadEnemies = new List<Character>();
        foreach (Character enemy in enemyParty.partyCharacters) {
            if (enemy.currentVie <= 0) {
                deadEnemies.Add(enemy);
                if (enemiesInFront.Contains(enemy)) {
                    enemiesInFront.Remove(enemy);
                } else {
                    enemiesInBack.Remove(enemy);
                }
            }
        }

        if (deadEnemies.Count == 0) {
            return;
        }

        //remove dead enemies from the enemyParty list, destroy its associated gameObject in the scene, and set its gameObject reference to null
        //remove dead enemies from the current and next turn lists, since they have already been created
        foreach (Character deadEnemy in deadEnemies) {
            enemyParty.partyCharacters.Remove(deadEnemy);
            DestroyImmediate(deadEnemy.gameObject, false); //DO *NOT* CHANGE TO TRUE, CHECK DOCUMENTATION
            deadEnemy.gameObject = null;

            if(currentTurn.Contains(deadEnemy)) {
                bui.RemoveFromCurrentTurnUIAt(currentTurn.IndexOf(deadEnemy));
                currentTurn.Remove(deadEnemy);
            }
            bui.RemoveFromNextTurnUIAt(nextTurn.IndexOf(deadEnemy));
            nextTurn.Remove(deadEnemy);
        }

        //make sure all characters are in the right place
        //(specifically if an enemy not at the "end" or if the last enemy in the front was one who died)
        UpdateAllCharacterPositions();
        if (enemiesInFront.Count == 0) {
            foreach (Character enemy in enemiesInBack) {
                enemy.isInFront = true;
                NewCharacterPosition(enemy);
            }
            enemiesInBack = new List<Character>();
        }
    }
    private void CreateBaseBattleMenu() {
        currBattleActions = new List<BattleAction>();
        foreach (Weapon weapon in curr.weaponsList) {
            currBattleActions.Add(basicBattleMenu.basicBattleAction);
        }
        foreach (BattleAction action in basicBattleMenu.menuActions) {
            currBattleActions.Add(action);
        }

        //basicBattleMenu contains both Advance and Retreat, so we have to get rid of one
        if (curr.isInFront) {
            currBattleActions.RemoveAt(currBattleActions.Count - 3); //remove Advance
        } else {
            currBattleActions.RemoveAt(currBattleActions.Count - 2); //remove Retreat
        }
        bui.DrawNewActionMenu(currBattleActions, curr, true);
    }
    private void CreateBattleSubMenu() {
        bui.DrawNewActionMenu(currBattleActions, curr, false);
    }

    /*
    Helper Functions for BattleController's functions involving action codes
        List<GameObject> getEligibleTargets(int actionCode): returns the current list of eligible targets for the given action code
        HandleTargettedAction(int actionCode, CCB target): executes targetted actions
    */
    public List<Character> GetActionTargets(BattleAction action) {
        List<Character> targets = new List<Character>();

        if (action.needsTarget) {
            if (playersInFront.Contains(curr) || enemiesInBack.Contains(curr)) {
                foreach (Character enemy in enemiesInFront) {
                    targets.Add(enemy);
                }
            } else if (enemiesInFront.Contains(curr) || playersInBack.Contains(curr)) {
                foreach (Character player in playersInFront) {
                    targets.Add(player);
                }
            }
        }

        return targets;
    }
    public void DoAction(BattleAction action) {
        bui.DisableActionMenu();

        if (action.needsTarget) {
            throw new Exception("Action code needs target");
        }

        if (bam.IsPerformActionSuccessful(action, curr)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    public void DoAction(BattleAction action, Character target) {
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr, target)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    public void DoAction(BattleAction action, Character target, Weapon weapon) {
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr, target, weapon)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    private void DoEnemyAction() {
        Character chosenPlayer = playersInFront[UnityEngine.Random.Range(0, playersInFront.Count)];
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand > 8) {
            bui.ChangeActionText("Move");
            Move();
        } else {
            bam.IsPerformActionSuccessful(basicBattleMenu.basicBattleAction, curr, chosenPlayer, curr.weaponsList[0]);
        }

        Invoke(nameof(EndCurrentAction), 1f);
    }

    /*
    Action Functions:                 
    */
    public bool Move() {
        return Move(curr);
    }
    private bool Move(Character moving) {
        bool success = true;
        if ((playersInFront.Contains(moving) && playersInFront.Count > 1) ||
                playersInBack.Contains(moving) ||
                (enemiesInFront.Contains(moving) && enemiesInFront.Count > 1) ||
                (enemiesInBack.Contains(moving))) {
            UpdateCharacterPosition(moving, enemiesInBack.Contains(moving) || playersInBack.Contains(moving));
        } else {
            success = false;
        }
        return success;
    }
}