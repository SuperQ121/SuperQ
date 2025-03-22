using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardInfo :ScriptableObject
{
   public string cardName;
   public Sprite sprite;
   public int cost;
   public uint cardId;
   protected Card card;
   public virtual bool CardFuction()
   {
      if (PlayerManager.instance.player.stat.buffState.ContainsKey(BuffType._紊乱))
      {
         _紊乱Info buffInfo=PlayerManager.instance.player.stat.buffInfos[9] as _紊乱Info;
         if (buffInfo != null)
         {
            cost += buffInfo.addEnergyCost;
            if(PlayerManager.instance.player.stat.energy>=cost)
            {
               PlayerManager.instance.player.stat.energy -= cost;
               PlayerManager.instance.player.stat.ExecuteBuffFunction(BuffType._紊乱);
               return true;
            }
            else
            {
               cost -= buffInfo.addEnergyCost;
               return false;
            }
         }
         else
         {
            Debug.Log("未能获取紊乱BuffInfo");
            return false;
         }
      }
      else
      {
         if (PlayerManager.instance.player.stat.energy>=cost)
         {
            PlayerManager.instance.player.stat.energy -= cost;
            return true;
         }
         else
         {
            return false;
         }
      }
   }

   public void SetTargetCard(Card _card)
   {
      card = _card;
   }
}
