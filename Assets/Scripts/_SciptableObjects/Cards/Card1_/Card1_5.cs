using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_5", menuName = "Data/CardInfo1_/Card1_5",order = 5)]
public class Card1_5 : CraftCardInfo
{
    public int addBuffLayer;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            if (EnemyManager.instance.targetEnemyStat.buffState.ContainsKey(BuffType._风伤))
            {
                EnemyManager.instance.targetEnemyStat.buffState[BuffType._风伤]++;
            }
            EnemyManager.instance.targetEnemyStat.AddBufflayers(BuffType._风伤,addBuffLayer);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

