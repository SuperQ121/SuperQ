using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_2", menuName = "Data/CardInfo2_/Card2_2",order = 2)]
public class Card2_2 : CraftCardInfo
{
    public int damage;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.TakeDamage(damage,PlayerManager.instance.player);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
