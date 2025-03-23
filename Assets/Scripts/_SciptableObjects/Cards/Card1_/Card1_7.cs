using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_7", menuName = "Data/CardInfo1_/Card1_7",order = 7)]
public class Card1_7 : CraftCardInfo
{
    public int addBuffLayer;
    public override bool CardFuction()
    {
      
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._热浪,addBuffLayer);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

