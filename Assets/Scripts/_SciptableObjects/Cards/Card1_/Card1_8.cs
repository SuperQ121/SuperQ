using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_8", menuName = "Data/CardInfo1_/Card1_8",order = 8)]
public class Card1_8 : CraftCardInfo
{
    public int addBuffLayer;
    public override bool CardFuction()
    {
      
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._迷沙,addBuffLayer);
            EnemyManager.instance.targetEnemyStat.jumpRound=true;
            return true;
        }
        else
        { 
            return false;
        }
    }
}

