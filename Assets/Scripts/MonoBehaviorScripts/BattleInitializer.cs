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
        player1.GetComponent<CharacterCombatBehavior>().InitCharacter("Juniper");
        bm.NewCharacterPosition(player1, false);

        GameObject player2 = Instantiate(playerPrefabs[1]);
        player2.GetComponent<CharacterCombatBehavior>().InitCharacter("Lenoir");
        bm.NewCharacterPosition(player2, true);

        GameObject player3 = Instantiate(playerPrefabs[2]);
        player3.GetComponent<CharacterCombatBehavior>().InitCharacter("Solai");
        bm.NewCharacterPosition(player3, true);

        GameObject player4 = Instantiate(playerPrefabs[3]);
        player4.GetComponent<CharacterCombatBehavior>().InitCharacter("Luwan");
        bm.NewCharacterPosition(player4, true);

        GameObject player5 = Instantiate(playerPrefabs[4]);
        player5.GetComponent<CharacterCombatBehavior>().InitCharacter("Willow");
        bm.NewCharacterPosition(player5, true);

        GameObject enemy1 = Instantiate(enemyPrefabs[0]);
        enemy1.GetComponent<CharacterCombatBehavior>().InitCharacter("OldMan");
        bm.NewCharacterPosition(enemy1, true);

        GameObject enemy2 = Instantiate(enemyPrefabs[1]);
        enemy2.GetComponent<CharacterCombatBehavior>().InitCharacter("Patches");
        bm.NewCharacterPosition(enemy2, true);
        
        GameObject enemy3 = Instantiate(enemyPrefabs[2]);
        enemy3.GetComponent<CharacterCombatBehavior>().InitCharacter("Moogle");
        bm.NewCharacterPosition(enemy3, false);
        
        bui.ChangeActionText("Start");

        //Start Battle
        bm.StartBattle();
        bc.StartController();
    }
}
