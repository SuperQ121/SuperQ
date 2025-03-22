using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage,Entity attacker)
    {
        stat.TakeDamage(damage,attacker);
        
        if (stat.currentHealth <= 0)
        {
            PlayerManager.instance.isDie=true;
            GameManager.instance.EndGame();
            Destroy(gameObject);
            Debug.Log("玩家死亡");
        }
    }
    
}
