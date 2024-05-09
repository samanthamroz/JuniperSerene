using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject playersFrontContainer, playersBackContainer, enemiesFrontContainer, enemiesBackContainer;
    [SerializeField] public Party enemyParty, playerParty;
    private List<CharacterCombatBehavior> playersInFront = new();
    private List<CharacterCombatBehavior> playersInBack = new();
    private List<CharacterCombatBehavior> enemiesInFront = new();
    private List<CharacterCombatBehavior> enemiesInBack = new();
    private List<CharacterCombatBehavior> allCharacters {
        get {
            List<CharacterCombatBehavior> aggregate = new List<CharacterCombatBehavior>();
            aggregate.AddRange(playersInFront);
            aggregate.AddRange(playersInBack);
            aggregate.AddRange(enemiesInFront);
            aggregate.AddRange(enemiesInBack);
            return aggregate;
        }
    }
    private List<CharacterCombatBehavior> currentTurn, nextTurn;
    [SerializeField] public CharacterCombatBehavior curr {
        get { return currentTurn[0]; }
    }
    public List<BattleAction> currBattleActions;
    [SerializeField] private BattleMenu basicActionMenu;
    
    private BattleActionsManager bam;
    private BattleUIManager bui;
    private BattleAnimations ba;
    private BattleController bc;

    void Awake()
    {
        bam = gameObject.GetComponent<BattleActionsManager>();
        bui = gameObject.GetComponent<BattleUIManager>();
        ba = gameObject.GetComponent<BattleAnimations>();
        bc = gameObject.GetComponent<BattleController>();
    }

    /*
    CHARACTER OBJECT MANAGEMENT
    */
    public void NewCharacterPosition(CharacterCombatBehavior characterObj) {
        List<CharacterCombatBehavior> listToPlace;
        GameObject containerToPlace;
        int placeModifier;

        if (characterObj.character.isPlayable) {
            if (characterObj.isInFront) {
                listToPlace = playersInFront;
                containerToPlace = playersFrontContainer;
                placeModifier = 1;
            } else {
                listToPlace = playersInBack;
                containerToPlace = playersBackContainer;
                placeModifier = -1;
            }
        } else {
            if (characterObj.isInFront) {
                listToPlace = enemiesInFront;
                containerToPlace = enemiesFrontContainer;
                placeModifier = -1;
            } else {
                listToPlace = enemiesInBack;
                containerToPlace = enemiesBackContainer;
                placeModifier = 1;
            }
        }

        //sets the character's next and prev gameobjects according to what is in the scene
        if (listToPlace.Count == 0) {
            characterObj.next = characterObj;
            characterObj.prev = characterObj;
        } else {
            characterObj.next = listToPlace[0];
            characterObj.prev = listToPlace[^1];
            listToPlace[^1].next = characterObj;
            listToPlace[0].prev = characterObj;
        }


        //adds the character gameObject to the correct container gameObject for placement
        characterObj.gameObject.transform.parent = containerToPlace.transform;
        characterObj.gameObject.transform.localPosition = new Vector3(listToPlace.Count * 1.5f * placeModifier, 0, 0);

        //adds the character reference to the correct local list
        listToPlace.Add(characterObj);

        characterObj.gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
    }
    private void UpdateCharacterPosition(CharacterCombatBehavior characterObj, bool isGoingToFront) {
        List<CharacterCombatBehavior> listToLeave;
        List<CharacterCombatBehavior> shiftLeft = new();

        if (characterObj.character.isPlayable && !isGoingToFront) {
            listToLeave = playersInFront;
        } else if (characterObj.character.isPlayable && isGoingToFront) {
            listToLeave = playersInBack;
        } else if (!characterObj.character.isPlayable && !isGoingToFront) {
            listToLeave = enemiesInFront;
        } else if (!characterObj.character.isPlayable && isGoingToFront) {
            listToLeave = enemiesInBack;
        } else {
            throw new Exception("Character position could not be updated");
        }

        //removes the character from its current list and all characters to its "right"
        //places these characters in the shiftLeft list
        if (listToLeave.IndexOf(characterObj) != listToLeave.Count - 1) {
            shiftLeft.AddRange(listToLeave.GetRange(listToLeave.IndexOf(characterObj) + 1, listToLeave.Count - 1));
        }
        listToLeave.RemoveRange(listToLeave.IndexOf(characterObj), shiftLeft.Count + 1);

        //re-places the character at the "back" of the correct list and updates its property
        characterObj.isInFront = isGoingToFront;
        NewCharacterPosition(characterObj);

        //re-places each of the shiftLeft characters
        foreach (CharacterCombatBehavior characterToShift in shiftLeft) {
            NewCharacterPosition(characterToShift);
        }
    }
    private void UpdateAllCharacterPositions() {
        //copies the data from the global lists
        List<CharacterCombatBehavior> copyAggregate = playersInFront.GetRange(0, playersInFront.Count);
        copyAggregate.AddRange(playersInBack);
        copyAggregate.AddRange(enemiesInFront);
        copyAggregate.AddRange(enemiesInBack);

        //resets the global lists of positioned layers/enemies
        playersInFront.Clear();
        playersInBack.Clear();
        enemiesInFront.Clear();
        enemiesInBack.Clear();

        //repositions each character using the copied data
        foreach (CharacterCombatBehavior character in copyAggregate) {
            NewCharacterPosition(character);
        }
    }
    
    /*
    TURN MANAGEMENT
    */
    public void StartBattle() {
        //initialize 1st turn order
        CreateNextTurnOrder();

        //Start first turn
        StartNextTurn();
    }
    private void CreateNextTurnOrder() {
        nextTurn = new();

        List<CharacterCombatBehavior> allCharactersCopy = allCharacters.GetRange(0, allCharacters.Count);
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
        StartCoroutine(bui.CreateNewTurnUI(currentTurn, nextTurn));

        //start first action
        StartNextAction();
    }
    private void StartNextAction() {
        //Write stuff into menu if character is playable, else disable menu and choose enemy action
        if (!curr.character.isPlayable) {
            bc.StopController();
            bui.DisableActionMenu();
            
            Invoke(nameof(DoEnemyAction), ba.TurnStart(curr.gameObject));
        } else {
            //write options into menu
            bui.EnableActionMenu();
            CreateBaseBattleMenu();
            
            //play animations
            ba.TurnStart(curr.gameObject);
            
            //wait for UI input
            bc.RestartController();
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

        StartCoroutine(HandleDeadEnemies());

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
    TURN MANAGEMENT HELPERS
    */
    private IEnumerator HandleDeadEnemies() {
        //check for dead enemies
        List<Character> deadEnemies = enemyParty.RemoveDeadCharacters();

        if (deadEnemies.Count == 0) {
            yield break;
        }

        List<CharacterCombatBehavior> deadEnemyObjects = new();

        for (int i = enemiesInFront.Count - 1; i >= 0; i--) {
            if (deadEnemies.Contains(enemiesInFront[i].character)) {
                deadEnemyObjects.Add(enemiesInFront[i]);
                enemiesInFront.RemoveAt(i);
            }
        }
        for (int i = enemiesInBack.Count - 1; i >= 0; i--) {
            if (deadEnemies.Contains(enemiesInBack[i].character)) {
                deadEnemyObjects.Add(enemiesInBack[i]);
                enemiesInBack.RemoveAt(i);
            }
        }

       
        //Remove dead enemies from the current and next turn lists, since they have already been created
        //Destroy the dead enemy's gameObject
        foreach (CharacterCombatBehavior deadEnemy in deadEnemyObjects) {
            if (currentTurn.Contains(deadEnemy)) {
                StartCoroutine(bui.RemoveFromCurrentTurnUIAt(currentTurn.IndexOf(deadEnemy)));
                currentTurn.Remove(deadEnemy);
            }

            StartCoroutine(bui.RemoveFromNextTurnUIAt(nextTurn.IndexOf(deadEnemy)));
            nextTurn.Remove(deadEnemy);

            Destroy(deadEnemy.gameObject);
            yield return null;
        }

        //make sure all characters are in the right place
        //(specifically if an enemy not at the "end" or if the last enemy in the front was one who died)
        UpdateAllCharacterPositions();
        if (enemiesInFront.Count == 0) {
            foreach (CharacterCombatBehavior enemy in enemiesInBack) {
                enemy.isInFront = true;
                NewCharacterPosition(enemy);
            }
            enemiesInBack.Clear();
        }
    }
    private void CreateBaseBattleMenu() {
        currBattleActions = new List<BattleAction>();
        foreach (Weapon weapon in curr.character.weaponsList) {
            currBattleActions.Add(basicActionMenu.basicBattleAction);
        }
        foreach (BattleAction action in basicActionMenu.menuActions) {
            currBattleActions.Add(action);
        }

        //basicBattleMenu contains both Advance and Retreat, so we have to get rid of one
        if (curr.isInFront) {
            currBattleActions.RemoveAt(currBattleActions.Count - 3); //remove Advance
        } else {
            currBattleActions.RemoveAt(currBattleActions.Count - 2); //remove Retreat
        }

        StartCoroutine(bui.DrawNewActionMenu(currBattleActions, curr, true));
    }
    private void CreateBattleSubMenu() {
        StartCoroutine(bui.DrawNewActionMenu(currBattleActions, curr, false));
    }

    /*
    Helper Functions for BattleController's functions involving action codes
        List<GameObject> getEligibleTargets(int actionCode): returns the current list of eligible targets for the given action code
        HandleTargettedAction(int actionCode, CCB target): executes targetted actions
    */
    public List<CharacterCombatBehavior> GetEligibleTargets(BattleAction action) {
        List<CharacterCombatBehavior> targets = new();

        if ((action.targetsFriends && curr.character.isPlayable) || (action.targetNeeded == TargetType.PARTY && !curr.character.isPlayable)) {
            targets.AddRange(playersInFront);
            targets.AddRange(playersInBack);
        } else if ((action.targetsFriends && !curr.character.isPlayable) || (action.targetNeeded == TargetType.PARTY && curr.character.isPlayable)) {
            targets.AddRange(enemiesInFront);
            targets.AddRange(enemiesInBack);
        } else if (action.targetNeeded == TargetType.ALL) {
            targets = allCharacters;
        } else if ((action.targetNeeded == TargetType.SINGLE || action.targetNeeded == TargetType.MULTI) && !action.targetsFriends) {
            if (curr.character.isPlayable) {
                targets.AddRange(enemiesInFront);
            } else {
                targets.AddRange(playersInFront);
            }
        }
    
        if (targets.Count == 0) {
            throw new Exception("No targets found");
        }
        return targets;
    }
    
    public void DoAction(BattleAction action) {
        bc.StopController();
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    public void DoAction(BattleAction action, CharacterCombatBehavior target) {
        bc.StopController();
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr, target)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    public void DoAction(BattleAction action, CharacterCombatBehavior target, Weapon weapon) {
        bc.StopController();
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr, target, weapon)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }
    public void DoAction(BattleAction action, List<CharacterCombatBehavior> targets, Weapon weapon) {
        bc.StopController();
        bui.DisableActionMenu();

        if (bam.IsPerformActionSuccessful(action, curr, targets, weapon)) {
            Invoke(nameof(EndCurrentAction), 1f);
            return;
        }

        ContinueCurrAction();
    }


    private void DoEnemyAction() {
        CharacterCombatBehavior chosenPlayer = playersInFront[UnityEngine.Random.Range(0, playersInFront.Count)];
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand > 8) {
            bui.ChangeActionText("Move");
            Move();
        } else {
            bam.IsPerformActionSuccessful(basicActionMenu.basicBattleAction, curr, chosenPlayer, curr.character.weaponsList[0]);
        }

        Invoke(nameof(EndCurrentAction), 1f);
    }

    /*
    Action Functions:                 
    */
    public bool Move() {
        return Move(curr);
    }
    private bool Move(CharacterCombatBehavior characterObj) {
        bool success = true;
        if ((playersInFront.Contains(characterObj) && playersInFront.Count > 1) ||
                playersInBack.Contains(characterObj) ||
                (enemiesInFront.Contains(characterObj) && enemiesInFront.Count > 1) ||
                enemiesInBack.Contains(characterObj)) {
            UpdateCharacterPosition(characterObj, enemiesInBack.Contains(characterObj) || playersInBack.Contains(characterObj));
        } else {
            success = false;
        }
        return success;
    }
}