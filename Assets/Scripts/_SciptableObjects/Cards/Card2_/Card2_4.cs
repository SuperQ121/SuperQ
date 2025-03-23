using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_4", menuName = "Data/CardInfo2_/Card2_4",order = 4)]
public class Card2_4 : CraftCardInfo
{
    public int addArmor;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.AddArmor(addArmor);
            return true;
        }
        else
        { 
            return false;
        }
    }
}
