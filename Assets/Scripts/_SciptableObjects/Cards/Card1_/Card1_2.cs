using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card1_2", menuName = "Data/CardInfo1_/Card1_2",order = 2)]
public class Card1_2 : CardInfo
{
   public int healingHealth;
   public int addEnergy;
   public override bool CardFuction()
   {
      
      bool baseResult= base.CardFuction();

      if (baseResult)
      {
         PlayerManager.instance.player.stat.Healing(healingHealth);
         PlayerManager.instance.player.stat.AddEnergy(addEnergy);
         return true;
      }
      else
      {
         return false;
      }
   }
}

