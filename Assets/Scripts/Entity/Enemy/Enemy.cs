using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour,IPointerDownHandler
{
    public EnemyInfo enemyInfo;
    public EnemyStat stat;
    [Header("Select")]
    public GameObject selectImage;
    public bool select;

    public SerializableDictionary<CardInfo,int> needCards = new SerializableDictionary<CardInfo,int>();
    private void Awake()
    {
        enemyInfo.enemy = this;
        stat.maxHealth.SetDefaultValue(enemyInfo.maxHealth);
        stat.attackPower = enemyInfo.attackPower;
    }

    void Start()
    {
        //NOTE::开始时添加进EnemyManager方便管理
        EnemyManager.instance.enemys.Add(this);
        stat.onHealthChanged?.Invoke();
        SetNeedCards(enemyInfo.needCardsCount);
    }

    
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //NOTE::只处理左键事件
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (!select)
        {
            OnSelect();
        }
    }

    private void OnSelect()
    {
        EnemyManager.instance.SetTargetEnemy(this, stat);
    }

    public void TakeDamage(int damage)
    {
        stat.currentHealth -= damage;
        stat.onHealthChanged?.Invoke();
        if (stat.currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void DoDamage(int damage)
    {
        
        PlayerManager.instance.player.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        EnemyManager.instance.enemys.Remove(this);
        GameManager.instance.IfEndGame();
    }

    private void SetNeedCards(int needCardsCount)
    {
        enemyInfo.needCards.Clear();
        for (int i = 0; i < needCardsCount; i++)
        {
            CardInfo cardInfo = CardManager.instance.playerCards.GetRandom();
            if (!enemyInfo.needCards.ContainsKey(cardInfo))
            {
                enemyInfo.needCards.Add(cardInfo,1);
            }
            else
            {
                enemyInfo.needCards[cardInfo]++;
            }
        }

        foreach (var card in enemyInfo.needCards)
        {
            needCards.Add(card.Key,card.Value);
        }
    }
}
