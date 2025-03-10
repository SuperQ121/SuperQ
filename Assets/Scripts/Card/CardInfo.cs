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
   public virtual void CardFuction()
   {
      
   }

   public void SetTargetCard(Card _card)
   {
      card = _card;
   }
}
