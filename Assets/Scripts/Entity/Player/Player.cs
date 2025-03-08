using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        stat.currentHealth -= damage;
        
        if (stat.currentHealth <= 0)
        {
            PlayerManager.instance.isDie=true;
            GameManager.instance.EndGame();
            Destroy(gameObject);
            Debug.Log("玩家死亡");
        }
    }
}
