using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_6", menuName = "Data/CardInfo2_/Card2_6",order = 6)]
public class Card2_6 : CanSelfUseCardInfo
{
    public int healingHealth;
    public int damage;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult && useToEnemy)
        {
            EnemyManager.instance.targetEnemyStat.TakeDamage(damage,PlayerManager.instance.player);
            return true;
        }
        else if(baseResult && useToSelf)
        {
            PlayerManager.instance.player.stat.Healing(healingHealth);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
