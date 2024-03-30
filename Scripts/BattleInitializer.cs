using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class BattleInitializer : MonoBehaviour
{
    public List<GameObject> playerPrefabs, enemyPrefabs;
    private BattleManager bm;
    private BattleController bc;
    private BattleUIManager bui;
    void Start()
    {
        bm = gameObject.GetComponent<BattleManager>();
        bc = gameObject.GetComponent<BattleController>();
        bui = gameObject.GetComponent<BattleUIManager>();

        //initialize all players/enemies
        GameObject player1 = Instantiate(playerPrefabs[0]);
        bm.NewCharacterPosition(player1, true);
        player1.GetComponent<SpriteRenderer>().color = new Color(1,1,0);
        player1.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(1,1,0), "Yellow");

        GameObject player2 = Instantiate(playerPrefabs[0]);
        bm.NewCharacterPosition(player2, true);
        player2.GetComponent<SpriteRenderer>().color = new Color(0,1,1);
        player2.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(0,1,1), "Cyan");

        GameObject player3 = Instantiate(playerPrefabs[0]);
        bm.NewCharacterPosition(player3, false);
        player3.GetComponent<SpriteRenderer>().color = new Color(1,0,1);
        player3.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(1,0,1), "Pink");

        GameObject enemy1 = Instantiate(enemyPrefabs[0]);
        bm.NewCharacterPosition(enemy1, true);
        enemy1.GetComponent<SpriteRenderer>().color = new Color(1f,.5f,0);
        enemy1.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(1f,.5f,0), "Orange");

        GameObject enemy2 = Instantiate(enemyPrefabs[0]);
        bm.NewCharacterPosition(enemy2, true);
        enemy2.GetComponent<SpriteRenderer>().color = new Color(0,1f,.5f);
        enemy2.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(0,1f,.5f), "Green");
        
        GameObject enemy3 = Instantiate(enemyPrefabs[0]);
        bm.NewCharacterPosition(enemy3, false);
        enemy3.GetComponent<SpriteRenderer>().color = new Color(1f,0,.5f);
        enemy3.GetComponent<CharacterCombatBehavior>().InitializeFromDebugPrefab(300, new Color(1f,0,.5f), "Rose");
        
        bui.ChangeActionText("Start");

        //Start Battle
        bm.StartBattle();
        bc.StartController();
    }
}
