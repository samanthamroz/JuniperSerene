using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public GameObject characterPrefab;
    private BattleManager bm;
    private BattleUIManager bui;
    void Start()
    {
        bm = gameObject.GetComponent<BattleManager>();
        bui = gameObject.GetComponent<BattleUIManager>();

        bui.DrawNewHealthBars(bm.playerParty);

        //initialize all players/enemies
        foreach (Character player in bm.playerParty.charactersStartingInFront) {
            player.BattleReset(true);
            CharacterCombatBehavior playerObj = Instantiate(characterPrefab).GetComponent<CharacterCombatBehavior>();
            playerObj.gameObject.name = player.name;
            playerObj.gameObject.GetComponent<SpriteRenderer>().sprite = player.sprite;
            playerObj.character = player;
            playerObj.isInFront = true;
            bm.NewCharacterPosition(playerObj);
        }

        foreach (Character player in bm.playerParty.charactersStartingInBack) {
            player.BattleReset(true);
            CharacterCombatBehavior playerObj = Instantiate(characterPrefab).GetComponent<CharacterCombatBehavior>();
            playerObj.gameObject.name = player.name;
            playerObj.gameObject.GetComponent<SpriteRenderer>().sprite = player.sprite;
            playerObj.character = player;
            playerObj.isInFront = false;
            bm.NewCharacterPosition(playerObj);
        }

        foreach (Character enemy in bm.enemyParty.charactersStartingInFront) {
            enemy.BattleReset(true);
            CharacterCombatBehavior enemyObj = Instantiate(characterPrefab).GetComponent<CharacterCombatBehavior>();
            enemyObj.gameObject.name = enemy.name;
            enemyObj.gameObject.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
            enemyObj.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            enemyObj.character = enemy;
            enemyObj.isInFront = true;
            bm.NewCharacterPosition(enemyObj);
        }

        foreach (Character enemy in bm.enemyParty.charactersStartingInBack) {
            enemy.BattleReset(true);
            CharacterCombatBehavior enemyObj = Instantiate(characterPrefab).GetComponent<CharacterCombatBehavior>();
            enemyObj.gameObject.name = enemy.name;
            enemyObj.gameObject.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
            enemyObj.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            enemyObj.character = enemy;
            enemyObj.isInFront = false;
            bm.NewCharacterPosition(enemyObj);
        }

        bui.ChangeActionText("Start");

        //Start Battle
        bm.StartBattle();
    }
}
