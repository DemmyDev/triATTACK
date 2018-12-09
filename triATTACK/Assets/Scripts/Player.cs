using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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
            GameMaster.KillPlayer(this);
        }
    }
	
}
