using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour,IPointerDownHandler
{
    
    public EnemyStat stat;
    [Header("Select")]
    public GameObject selectImage;
    public bool select;
    void Start()
    {
        //NOTE::开始时添加进EnemyManager方便管理
        EnemyManager.instance.enemys.Add(this);
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
        if (stat.currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DoDamage()
    {
        PlayerManager.instance.Player.TakeDamage(stat.attackPower);
    }

    private void OnDestroy()
    {
        EnemyManager.instance.enemys.Remove(this);
        GameManager.instance.IfEndGame();
    }
}
