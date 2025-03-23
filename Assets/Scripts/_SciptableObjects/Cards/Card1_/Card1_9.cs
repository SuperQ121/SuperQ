using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card1_9", menuName = "Data/CardInfo1_/Card1_9",order = 9)]
public class Card1_9 : CraftCardInfo
{
    public int healingHealth;
    public override bool CardFuction()
    {
      
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            PlayerManager.instance.player.stat.Healing(healingHealth);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

