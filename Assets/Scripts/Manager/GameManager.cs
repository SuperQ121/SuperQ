using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public HorizontalCardHolder playerCardHolder;

    [Header("Artifice")] 
    public GameObject buttonParentNode;
    [SerializeField]private Button artificeBtn;
    [SerializeField]private Button cancelArtificeBtn;
    
    [Header("Recycle")]
    [SerializeField]private Button recycleBtn;
    
    [Header("Buff Info")]
    public List<BuffType> debuffList = new List<BuffType>();
    
    public List<BuffInfo> playerBuffInfo=new List<BuffInfo>();
    public List<BuffType> playerBuffNeedExecuteEndRound = new List<BuffType>();
    [Space(5)]
    public List<BuffInfo> enemyBuffInfo=new List<BuffInfo>();
    public List<BuffType> enemyBuffNeedExecuteEndRound=new List<BuffType>();
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
        cancelArtificeBtn.onClick.AddListener(CancelArtificeBtnClicked);
        artificeBtn.onClick.AddListener(ArtificeBtnClicked);
        recycleBtn.onClick.AddListener(RecycleBtnClicked);
        LoadPlayerBuffNeedExecuteEndRound();
        LoadEnemyBuffNeedExecuteEndRound();
    }
    
    private void LoadPlayerBuffNeedExecuteEndRound()
    {
        playerBuffNeedExecuteEndRound.Add(BuffType._疗愈);
        playerBuffNeedExecuteEndRound.Add(BuffType._护花);
        playerBuffNeedExecuteEndRound.Add(BuffType._风伤);
        playerBuffNeedExecuteEndRound.Add(BuffType._灼血);
        playerBuffNeedExecuteEndRound.Add(BuffType._中毒);
    }
    private void LoadEnemyBuffNeedExecuteEndRound()
    {
        enemyBuffNeedExecuteEndRound.Add(BuffType._风伤);
        enemyBuffNeedExecuteEndRound.Add(BuffType._灼血);
        enemyBuffNeedExecuteEndRound.Add(BuffType._中毒);
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

        if (PlayerManager.instance.player.stat.buffState.Count>0)
        {
            foreach (var buffType in PlayerManager.instance.player.stat.buffState)
            {
                if(playerBuffNeedExecuteEndRound.Contains(buffType.Key))
                {
                    PlayerManager.instance.player.stat.ExecuteBuffFunction(buffType.Key);
                }
            }
        }

        foreach (var enemy in EnemyManager.instance.enemys)
        {
            if (enemy.stat.buffState.Count>0)
            {
                foreach (var buffType in enemy.stat.buffState)
                {
                    if (enemyBuffNeedExecuteEndRound.Contains(buffType.Key))
                    {
                        enemy.stat.ExecuteBuffFunction(buffType.Key);
                    }
                }
            }
        }

        List<Enemy> enemys = new List<Enemy>();
        enemys.AddRange(EnemyManager.instance.enemys);
        
        StartCoroutine(ExecuteEnemySkill(enemys,0.1f));
        /*foreach (var enemy in EnemyManager.instance.enemys)
        {
            {
                enemy.enemyInfo.SkillFuction();
            }
        }
        DrawCard(CardManager.instance.GetDarwCardAmount());*/
        
        IEnumerator ExecuteEnemySkill(List<Enemy> enemys,float startTime)
        {
            if (startTime>0)
            {
                yield return new WaitForSeconds(startTime);
            }
            
            if (enemys.Count == 0)
            {
                DrawCard(CardManager.instance.GetDarwCardAmount());
                yield break;
            }
            if(!enemys.First().stat.jumpRound)
            {
                enemys.First().enemyInfo.SkillFuction();
            }
            else
            {
                enemys.First().stat.jumpRound=false;
            }
            yield return new WaitForSeconds(0.1f);
            enemys.Remove(enemys.First());
            StartCoroutine(ExecuteEnemySkill(enemys,0));
        }
    }
   

    public void DrawCard(int amount)
    {
        if(playerCardHolder != null)
        {
            StartCoroutine(DrawCards(amount));
        }
    }

    //NOTE::协程解决异步操作导致的异常
    IEnumerator DrawCards(int remainingTimes)
    {
        if (remainingTimes <= 0) yield break;
        yield return new WaitForSeconds(0.1f);
        playerCardHolder.DrawCard();
        CardManager.instance.currentCardCount++;
        if (CardManager.instance.cardCount>=1)
        {
            CardManager.instance.cardCount--;
        }
        StartCoroutine(DrawCards(remainingTimes-1));
    }

    //NOTE::怪物死亡时判断使用
    public void IfEndGame()
    {
        if (EnemyManager.instance.enemys.Count <= 0||PlayerManager.instance.isDie)
        {
            EndGame();
        }
        else
        {
            EnemyManager.instance.SetTargetEnemy(EnemyManager.instance.enemys.First(),EnemyManager.instance.enemys.First().stat);
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
    }

    public void ShowButton()
    {
        buttonParentNode.SetActive(true);
    }

    public void HideButton()
    {
        buttonParentNode.SetActive(false);
    }

    public void CancelArtificeBtnClicked()
    {
        List<Card> cards = new List<Card>();

        cards.AddRange(CardManager.instance.selectedCards);
        foreach (var card in cards)
        {
            card.SelectedCard();
        }
    }
    
    public void ArtificeBtnClicked()
    {
        List<Card> selectedCards = new List<Card>();
        selectedCards.AddRange(CardManager.instance.selectedCards);
        
        Dictionary<CardInfo,int> cardInfos = new Dictionary<CardInfo, int>();
       cardInfos.AddRange(EnemyManager.instance.targetEnemy.enemyInfo.needCards);

        foreach (var card in selectedCards)
        {
            if (cardInfos.ContainsKey(card.cardInfo))
            {
                cardInfos[card.cardInfo]--;
                card.artificeRequired = true;
                if (cardInfos[card.cardInfo] == 0)
                {
                    cardInfos.Remove(card.cardInfo);
                }
            }
        }

        if (cardInfos.Count == 0)
        {
            foreach (var card in selectedCards)
            {
                if (card.artificeRequired)
                {
                    card.DestroyCard();
                }
            }
            CardManager.instance.AddCard(EnemyManager.instance.targetEnemy.enemyInfo.canGetCard);
            EnemyManager.instance.targetEnemy.DestroySelf();
        }
    }

    public void RecycleBtnClicked()
    {
        foreach (var card in CardManager.instance.selectedCards)
        {
            if (card.cardInfo.canRecycle)
            {
                PlayerManager.instance.player.stat.AddEnergy(card.cardInfo.recycleEnergy);
                card.DestroyCard();
            }
        }
    }
}
