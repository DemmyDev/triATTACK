using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject deathText;

    [System.Serializable]
    public class PlayerStats
    {
        public int health;
    }

    public PlayerStats playerStats = new PlayerStats();

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            DamagePlayer(1);
        }
    }

    public void DamagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            GameObject.Find("GameMaster").GetComponent<EnemySpawner>().enabled = false;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                GameObject.Destroy(bullet);
            }
            
            DeathText dText = deathText.GetComponent<DeathText>();
            dText.EnableText();
            gameObject.SetActive(false);
        }
    }
	
}
