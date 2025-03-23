using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat:MonoBehaviour
{
    [Space(8)]
    [Header("Character stats")]
    public Stat maxHealth;
    public int armor;
    
    public int currentHealth;
    public System.Action onHealthChanged;
    
    public Dictionary<BuffType,int> buffState = new Dictionary<BuffType,int>();//NOTE::类型，层数
    public Dictionary<BuffType,Action> buffActions = new Dictionary<BuffType, Action>();//NOTE::类型，函数
    public Dictionary<int,BuffInfo> buffInfos = new Dictionary<int,BuffInfo>();//NOTE::buff ID,buff信息

    public bool jumpRound=false;
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        AddBuffAction();
    }

    
    protected virtual void AddBuffAction()
    {
        buffActions.Add(BuffType._风伤, _风伤);
        buffActions.Add(BuffType._热浪, _热浪);
        buffActions.Add(BuffType._迷沙, _迷沙);
        buffActions.Add(BuffType._灼血, _灼血);
        buffActions.Add(BuffType._中毒, _中毒);
    }

    private void _风伤()
    {
        _风伤Info buffInfo = buffInfos[4] as _风伤Info;
        if (buffInfo != null)
        {
            BuffReduceHealth(buffInfo.damage);
        }
        ReduceBuffLayers(BuffType._风伤);
    }

    private void _热浪()
    {
        ReduceBuffLayers(BuffType._热浪);
    }

    private void _迷沙()
    {
        ReduceBuffLayers(BuffType._迷沙);
    }

    private void _灼血()
    {
        _灼血Info buffInfo = buffInfos[7] as _灼血Info;
        if (buffInfo != null)
        {
            BuffReduceHealth(buffInfo.damage);
        }
        ReduceBuffLayers(BuffType._灼血);
    }

    private void _中毒()
    {
        _中毒Info buffInfo = buffInfos[8] as _中毒Info;
        if (buffInfo != null)
        {
            BuffReduceHealth(buffInfo.damage);
        }
        ReduceBuffLayers(BuffType._中毒);
    }
    
    public void ExecuteBuffFunction(BuffType type)
    {
        if (buffActions.ContainsKey(type))
        {
            buffActions[type].Invoke();
        }
    }
    public void Healing(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }
        onHealthChanged?.Invoke();
    }
    

    public virtual void TakeDamage(int amount, Entity attacker)
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
    }

    public virtual void BuffReduceHealth(int amount)
    {
        currentHealth -=amount;
        if (currentHealth<0)
        {
            currentHealth = 0;
        }
        onHealthChanged?.Invoke();
    }

    public void ReduceBuffLayers(BuffType type,int amount=1)
    {
        buffState[type]--;
        if (buffState[type] <= 0)
        {
            buffState.Remove(type);
        }
    }

    public void AddBufflayers(BuffType type, int amount)
    {
        if (buffState.ContainsKey(type))
        {
            buffState[type] += amount;
        }
        else
        {
            buffState.Add(type,amount);
        }
    }
    public void AddArmor(int amount)
    {
        armor += amount;
    }
}
