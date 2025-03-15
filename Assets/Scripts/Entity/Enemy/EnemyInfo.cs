using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ScriptableObject
{
   public string enemyName;
   public int maxHealth;
   public int attackPower;
   
   public Sprite sprite;
   public Dictionary<CardInfo,int> needCards=new Dictionary<CardInfo,int>();
   public int needCardsCount;
   public CardInfo canGetCard;
   
   
   [SerializeField]public Enemy enemy;
   public virtual void SkillFuction()
   {
      
   }
}
