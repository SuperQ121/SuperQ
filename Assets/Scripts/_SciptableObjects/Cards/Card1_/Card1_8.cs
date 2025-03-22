using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_8",order = 8)]
public class Card1_8 : CardInfo
{
    public int attackPower;
    public override bool CardFuction()
    {
      
        if(EnemyManager.instance.targetEnemy!=null)
        {
            
        }
        return true;
    }
}

