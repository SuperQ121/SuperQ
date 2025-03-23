using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card2_7", menuName = "Data/CardInfo2_/Card2_7",order = 7)]
public class Card2_7 : CardInfo
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

