using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Enemy targetEnemy;
    public EnemyStat targetEnemyStat;

    public List<Enemy> enemys;
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
        StartCoroutine(DefaultTargetEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetEnemy(Enemy _enemy,EnemyStat _enemyStat)
    {
        targetEnemy = _enemy;
        targetEnemyStat = _enemyStat;

        //NOTE::遍历将已有的选中图标关闭
        foreach (var enemy in enemys)
        {
            enemy.selectImage.SetActive(false);
            enemy.select = false;
        }
        _enemy.selectImage.SetActive(true);
        _enemy.select = true;
    }

    IEnumerator DefaultTargetEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        if (enemys.Count>0)
        {
            SetTargetEnemy(enemys[0],enemys[0].stat);
        }
        else
        {
            StartCoroutine(DefaultTargetEnemy());
        }
    }
}
