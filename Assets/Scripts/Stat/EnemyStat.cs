using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStat : CharacterStat
{
    public Enemy enemy;
    public int attackPower;

    public override void BuffReduceHealth(int amount)
    {
        currentHealth -=amount;
        if (currentHealth<0)
        {
            currentHealth = 0;
            onHealthChanged?.Invoke();
            Destroy(enemy.gameObject);
            return;
        }
        onHealthChanged?.Invoke();
       
    }
}
