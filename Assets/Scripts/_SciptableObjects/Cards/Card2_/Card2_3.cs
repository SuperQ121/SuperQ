using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_3", menuName = "Data/CardInfo2_/Card2_3",order = 3)]
public class Card2_3 : CraftCardInfo
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
