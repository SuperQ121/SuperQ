using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Data/Enemy2_/Enemy2_2",order = 2)]
public class Enemy2_2 : EnemyInfo
{
    public override void SkillFuction()
    {
        enemy.DoDamage(enemy.stat.attackPower);
    }
}