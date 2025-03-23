using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_10", menuName = "Data/CardInfo2_/Card2_10",order = 10)]
public class Card2_10 : CardInfo
{
    public int addLayerToSelf;
    public int addLayerToTarget;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult )
        {
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._紊乱,addLayerToSelf);
            foreach (var enemyBuff in GameManager.instance.enemyBuffInfo)
            {
                EnemyManager.instance.targetEnemyStat.AddBufflayers(enemyBuff.buffType,addLayerToTarget);
            }
            return true;
        }
        else
        { 
            return false;
        }
    }
}

