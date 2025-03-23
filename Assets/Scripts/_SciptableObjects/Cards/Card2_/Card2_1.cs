using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_1", menuName = "Data/CardInfo2_/Card2_1",order = 1)]
public class Card2_1 : CraftCardInfo
{
    public int addLayers;
    public int healingHealth;  
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._护花,addLayers);
            PlayerManager.instance.player.stat.Healing(healingHealth);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
