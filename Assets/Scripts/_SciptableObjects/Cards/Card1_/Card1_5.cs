using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_5",order = 5)]
public class Card1_5 : CardInfo
{
    public int healingAmount;
    public override void CardFuction()
    {
        PlayerManager.instance.player.Healing(healingAmount);
    }
}

