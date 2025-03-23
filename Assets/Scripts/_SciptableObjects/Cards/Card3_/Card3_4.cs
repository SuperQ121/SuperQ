using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card3_4", menuName = "Data/CardInfo3_/Card3_4",order = 4)]
public class Card3_4 : CardInfo
{
    public int addLayers;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._中毒,addLayers);
            return true;
        }
        else
        { 
            return false;
        }
    }
}


