using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ScriptableObject
{
   public string enemyName;
   public Sprite sprite;
   public List<CardInfo> needCards;
   public CardInfo canGetCard;
}
