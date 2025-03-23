using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_4", menuName = "Data/CardInfo1_/Card1_4",order = 4)]
public class Card1_4 : CardInfo
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
