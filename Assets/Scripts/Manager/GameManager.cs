using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public HorizontalCardHolder playerCardHolder;

    [Header("Artifice")] 
    public GameObject artificeParentNode;
    [SerializeField]private Button artificeBtn;
    [SerializeField]private Button cancelArtificeBtn;
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
                enemy.enemyInfo.SkillFuction();
            }
        }
        DrawCard(CardManager.instance.GetDarwCardAmount());
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

    public void showArtificeButton()
    {
        artificeParentNode.SetActive(true);
    }

    public void hideArtificeButton()
    {
        artificeParentNode.SetActive(false);
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
}
