using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_4",order = 4)]
public class Card1_4 : CardInfo
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
