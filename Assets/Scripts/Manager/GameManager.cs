using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int darwCardAmount;
    public HorizontalCardHolder playerCardHolder;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextRound()
    {
        if (EnemyManager.instance.enemys.Count<=0)
        {
            return;
        }
        
        foreach (var enemy in EnemyManager.instance.enemys)
        {
            {
                enemy.DoDamage();
            }
        }
        DrawCard(darwCardAmount);
    }

    public void DrawCard(int amount)
    {
        if(playerCardHolder != null)
            playerCardHolder.DrawCard(amount);
    }

    //NOTE::怪物死亡时判断使用
    public void IfEndGame()
    {
        if (EnemyManager.instance.enemys.Count <= 0||PlayerManager.instance.isDie)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
    }
}
