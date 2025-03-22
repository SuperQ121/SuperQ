using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Data/CardInfo1_/Card1_1",order = 1)]
public class Card1_1 : CardInfo
{
   public int attackPower;
   public override bool CardFuction()
   {
      bool baseResult= base.CardFuction();

      if (baseResult)
      {
         if (EnemyManager.instance.targetEnemy != null)
         {
            EnemyManager.instance.targetEnemy.TakeDamage(attackPower, PlayerManager.instance.player);
         }
         return true;
      }
      else
      {
         return false;
      }
   }
}
