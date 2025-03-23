using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card3_3", menuName = "Data/CardInfo3_/Card3_3",order = 3)]
public class Card3_3 : CardInfo
{
    public int addLayer_灼血;
    public int addLayer_紊乱;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._灼血,addLayer_灼血);
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._紊乱,addLayer_紊乱);
            return true;
        }
        else
        { 
            return false;
        }
    }
}


