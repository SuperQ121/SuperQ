using System;
using UnityEngine;

public class CharacterStat:MonoBehaviour
{
    [Space(8)]
    [Header("Character stats")]
    public Stat maxHealth;
    public Stat armor;
    
    public int currentHealth;
    public System.Action onHealthChanged;
    protected void Start()
    {
        currentHealth = maxHealth.GetValue();
    }
}
