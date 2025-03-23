using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card3_5", menuName = "Data/CardInfo3_/Card3_5",order = 5)]
public class Card3_5 : CardInfo
{
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            return true;
        }
        else
        { 
            return false;
        }
    }
}


