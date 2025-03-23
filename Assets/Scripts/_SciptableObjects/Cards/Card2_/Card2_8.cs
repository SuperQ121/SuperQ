using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_8", menuName = "Data/CardInfo2_/Card2_8",order = 8)]
public class Card2_8 : CanSelfUseCardInfo
{
    public int healingHealth;
    public int addLayers;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult && useToEnemy)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._中毒,addLayers);
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
