using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : CharacterStat
{
   public Stat maxEnergy;
   public int energy;
   public Player player;

   protected override void Start()
   {
      base.Start();
      LoadBuffInfo();
   }

   protected override void AddBuffAction()
   {
      base.AddBuffAction();
      buffActions.Add(BuffType._疗愈, _疗愈);
      buffActions.Add(BuffType._护花, _护花);
      buffActions.Add(BuffType._反灼, _反灼);
      buffActions.Add(BuffType._紊乱, _紊乱);

   }

   private void _疗愈()
   {
      _疗愈Info buffInfo = buffInfos[1] as _疗愈Info;
      if (buffInfo != null)
      {
         Healing(buffInfo.healingHealth);
         AddEnergy(buffInfo.healingEnergy);
      }

      ReduceBuffLayers(BuffType._疗愈);
   }

   private void _护花()
   {
      _护花Info buffInfo = buffInfos[2] as _护花Info;
      if (buffInfo != null)
      {
         AddArmor(buffInfo.addArmor);
         Healing(buffInfo.healingHealth);
         AddEnergy(buffInfo.healingEnergy);
      }

      ReduceBuffLayers(BuffType._护花);
   }

   private void _反灼()
   {
      ReduceBuffLayers(BuffType._反灼);
   }
   
   private void Execute_反灼(Entity enemy)
   {
      if (buffState.ContainsKey(BuffType._反灼))
      {
         ExecuteBuffFunction(BuffType._反灼);
         Enemy targetEnemy= enemy as Enemy;
         _反灼Info buffInfo = buffInfos[3] as _反灼Info;
         if (buffInfo != null)
         {
            targetEnemy.TakeDamage(buffInfo.reverseDamage,PlayerManager.instance.player);
         }

         if (armor<=0 && buffState.ContainsKey(BuffType._反灼))
         {
            buffState.Remove(BuffType._反灼);
         }
      }
   }

   private void _紊乱()
   {
      ReduceBuffLayers(BuffType._紊乱);
   }

   public override void TakeDamage(int amount,Entity enemy)
   {
      if (buffState.ContainsKey(BuffType._迷沙))
      {
         _迷沙Info buffInfo=buffInfos[6] as _迷沙Info;
         ExecuteBuffFunction(BuffType._迷沙);
         if (buffInfo != null)
         {
            if (armor >= buffInfo.armorDamageMultiplier * amount)
            {
               armor -= buffInfo.armorDamageMultiplier * amount;
            }
            else
            {
               currentHealth -= (buffInfo.armorDamageMultiplier * amount - armor) / 2;
               if (currentHealth < 0)
               {
                  currentHealth = 0;
               }

               onHealthChanged?.Invoke();
            }
         }
         else
         {
            Debug.Log("迷沙buffInfo获取出错");
         }
      }
      else
      {
         if (armor >= amount)
         {
            armor -= amount;
         }
         else
         {
            currentHealth = currentHealth + armor - amount;
            if (currentHealth < 0)
            {
               currentHealth = 0;
            }

            onHealthChanged?.Invoke();
         }
      }

      Execute_反灼(enemy);
   }

   public override void BuffReduceHealth(int amount)
   {
      currentHealth -=amount;
      if (currentHealth<0)
      {
         currentHealth = 0;
         onHealthChanged?.Invoke();
         PlayerManager.instance.isDie=true;
         GameManager.instance.EndGame();
         Destroy(player.gameObject);
         return;
      }
      onHealthChanged?.Invoke();
   }

   public void AddEnergy(int amount)
   {
      energy += amount;
      if (energy>maxEnergy.GetValue())
      {
         energy = maxEnergy.GetValue();
      }
   }

   public void LoadBuffInfo()
   {
      foreach (var obj in GameManager.instance.playerBuffInfo)
      {
         buffInfos.Add(obj.buffID,obj);
      }
   }
}
