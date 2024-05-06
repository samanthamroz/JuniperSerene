using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEditor;

public class BattleInitializer : MonoBehaviour
{
    public GameObject characterPrefab;
    public List<BattleAction> actionsList;
    private BattleManager bm;
    private BattleUIManager bui;
    void Start()
    {
        bm = gameObject.GetComponent<BattleManager>();
        bui = gameObject.GetComponent<BattleUIManager>();

        bui.DrawNewHealthBars(bm.playerParty);

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
    }
}
