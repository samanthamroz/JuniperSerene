using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class BattleInitializer : MonoBehaviour
{
    public GameObject characterPrefab;
    private BattleManager bm;
    private BattleController bc;
    private BattleUIManager bui;
    void Start()
    {
        bm = gameObject.GetComponent<BattleManager>();
        bc = gameObject.GetComponent<BattleController>();
        bui = gameObject.GetComponent<BattleUIManager>();

        bui.WriteNewHealthBars(bm.playerParty);

        //initialize all players/enemies
        foreach (Character player in bm.playerParty.partyCharacters) {
            player.BattleReset(true);
            player.gameObject = Instantiate(characterPrefab);
            player.gameObject.name = player.name;
            player.gameObject.GetComponent<SpriteRenderer>().sprite = player.sprite;
            bm.NewCharacterPosition(player);
        }

        foreach (Character enemy in bm.enemyParty.partyCharacters) {
            enemy.BattleReset(false);
            enemy.gameObject = Instantiate(characterPrefab);
            enemy.gameObject.name = enemy.name;
            enemy.gameObject.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
            enemy.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            bm.NewCharacterPosition(enemy);
        }

        bui.ChangeActionText("Start");

        //Start Battle
        bm.StartBattle();
        bc.StartController();
    }
}
