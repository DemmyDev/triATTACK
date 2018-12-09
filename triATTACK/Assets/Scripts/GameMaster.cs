using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    /*
    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay;

    
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    */

    public static void KillPlayer(Player player)
    {
        //Destroy(player.gameObject);
        //gm.StartCoroutine(gm.RespawnPlayer());
        SceneManager.LoadScene("SampleScene");
    }

    public static void KillShootingEnemy(ShootingEnemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    public static void KillHomingEnemy(HomingEnemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
