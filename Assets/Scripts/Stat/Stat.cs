using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    _疗愈=0,
    _护花=2,
    _反灼=4,
    _风伤=8,
    _热浪=16,
    _迷沙=32,
    _灼血=64,
    _中毒=128,
    _紊乱=256
}
[System.Serializable]
public class Stat 
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        //NOTE::以此实现装备等对伤害的加成
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value; 
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
    
}