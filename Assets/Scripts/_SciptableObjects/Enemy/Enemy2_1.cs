using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Data/Enemy2_/Enemy2_1",order = 1)]
public class Enemy2_1 : EnemyInfo
{
   public override void SkillFuction()
   {
      enemy.DoDamage(enemy.stat.attackPower);
   }
}
