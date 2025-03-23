using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_5", menuName = "Data/CardInfo2_/Card2_5",order = 5)]
public class Card2_5 : CraftCardInfo
{
    public int addArmor;
    public int addLayers;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.AddArmor(addArmor);
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._反灼,addLayers);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
