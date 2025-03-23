using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card3_2", menuName = "Data/CardInfo3_/Card3_2",order = 2)]
public class Card3_2 : CardInfo
{
    public int addLayers;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._紊乱,addLayers);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

