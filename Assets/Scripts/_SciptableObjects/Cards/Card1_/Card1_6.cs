using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_6", menuName = "Data/CardInfo1_/Card1_6",order = 6)]
public class Card1_6 : CraftCardInfo
{
    public int addBuffLayer;
    public override bool CardFuction()
    {
      
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._疗愈,addBuffLayer);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
