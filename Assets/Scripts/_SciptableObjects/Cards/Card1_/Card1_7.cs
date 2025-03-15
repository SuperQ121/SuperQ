using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_7",order = 7)]
public class Card1_7 : CardInfo
{
    public int attackPower;
    public override void CardFuction()
    {
      
        if(EnemyManager.instance.targetEnemy!=null)
        {
            EnemyManager.instance.targetEnemy.TakeDamage(attackPower);
        }
    }
}

