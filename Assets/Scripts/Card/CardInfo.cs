using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum CardType
{
   _气=2,
   _水=4,
   _火=8,
   _土=16,
   _正=32,
   _反=64
}

public class CardInfo :ScriptableObject
{
   public bool canRecycle=true;
   [TextArea(2,5)]
   public string description;
   [Space(10)]
   public uint cardId;
   public CardType type;
   public string cardName;
   public Sprite sprite;
   public int costEnergy;
   public int recycleEnergy;
   
   protected Card card;
   public virtual bool CardFuction()
   {
      if (PlayerManager.instance.player.stat.buffState.ContainsKey(BuffType._紊乱))
      {
         _紊乱Info buffInfo=PlayerManager.instance.player.stat.buffInfos[9] as _紊乱Info;
         if (buffInfo != null)
         {
            costEnergy += buffInfo.addEnergyCost;
            if(PlayerManager.instance.player.stat.currentEnergy>=costEnergy)
            {
               PlayerManager.instance.player.stat.currentEnergy -= costEnergy;
               PlayerManager.instance.player.stat.ExecuteBuffFunction(BuffType._紊乱);
               return true;
            }
            else
            {
               costEnergy -= buffInfo.addEnergyCost;
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
         if (PlayerManager.instance.player.stat.currentEnergy>=costEnergy)
         {
            PlayerManager.instance.player.stat.currentEnergy -= costEnergy;
            return true;
         }
         else
         {
            return false;
         }
      }
   }

   public void Recycle()
   {
      PlayerManager.instance.player.stat.currentEnergy+=recycleEnergy;
      if (PlayerManager.instance.player.stat.currentEnergy>PlayerManager.instance.player.stat.maxEnergy.GetValue())
      {
         PlayerManager.instance.player.stat.currentEnergy = PlayerManager.instance.player.stat.maxEnergy.GetValue();
      }
   }
   public void SetTargetCard(Card _card)
   {
      card = _card;
   }
}
