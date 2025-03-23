using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_9", menuName = "Data/CardInfo2_/Card2_9",order = 9)]
public class Card2_9 : CardInfo
{
    public int addLayer_热浪;
    public int addLayer_灼血;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._热浪,addLayer_热浪);
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._灼血,addLayer_灼血);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

