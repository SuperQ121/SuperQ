using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_3",order = 3)]
public class Card1_3 : CardInfo
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

