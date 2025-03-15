using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public Transform useCardCheckPoint;
    
    public List<CardInfo> cards = new List<CardInfo>(); 
    [SerializeField] public Dictionary<uint, CardInfo> cardGroup = new Dictionary<uint, CardInfo>();
    
    public List<CardInfo> playerCards = new List<CardInfo>();
    
    public Dictionary<CardInfo,int> playerDrawCardGroup = new Dictionary<CardInfo, int>();
    public Dictionary<CardInfo,int> playerThrowCardGroup = new Dictionary<CardInfo, int>();
    
    public int cardsToSpawn;
    public int currentCardCount;
    public int cardCount;
    
    [SerializeField]public List<Card> selectedCards = new List<Card>();
    
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

    void Start()
    {
        currentCardCount=cardsToSpawn;
        LoadCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void LoadCards()
    {
        //NOTE::将卡牌加载进卡牌堆
        foreach (var card in cards)
        {
            cardGroup.Add(card.cardId, card);
        }

        foreach (var card in playerCards)
        {
            playerDrawCardGroup.Add(card,5);
            playerThrowCardGroup.Add(card,0);
            cardCount += 5;
        }
    }

    public CardInfo GetRandomCardFormPlayerCards()
    {
        //NOTE::抽牌堆用完时将弃牌堆卡牌放入抽牌堆
        if (playerDrawCardGroup.Count == 0 || playerDrawCardGroup.Count<GetDarwCardAmount())
        {
            if (playerDrawCardGroup == null || playerDrawCardGroup.Count == 0)
            {
                playerDrawCardGroup.AddRange(playerThrowCardGroup);
            }
            else
            {
                foreach (var card in playerDrawCardGroup)
                {
                    playerThrowCardGroup[card.Key]+=card.Value;
                }
                playerDrawCardGroup.Clear();
                playerDrawCardGroup.AddRange(playerThrowCardGroup);
            }

            cardCount = 0;
            foreach (var card in playerCards)
            {
                playerThrowCardGroup[card] = 0;
                cardCount += playerDrawCardGroup[card];
                if (playerDrawCardGroup[card] == 0)
                {
                    playerDrawCardGroup.Remove(card);
                }
            }

            cardCount--;
        }

        CardInfo cardInfo = GetrandomCardFormCardGroup(playerDrawCardGroup);
        playerDrawCardGroup[cardInfo]--;
        if (playerDrawCardGroup[cardInfo] == 0)
            playerDrawCardGroup.Remove(cardInfo);
        return cardInfo;
    }

    private CardInfo GetrandomCardFormCardGroup(Dictionary<CardInfo, int> dictionary)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            Debug.LogError("字典为空或未初始化！");
            return null; // 返回一个无效值
        }

        // 获取所有键
        List<CardInfo> keys = new List<CardInfo>(dictionary.Keys);

        // 随机选择一个键
        int randomIndex = UnityEngine.Random.Range(0, keys.Count);
        return keys[randomIndex];
    }

    public void AddCard(CardInfo cardInfo)
    {
        if (!playerCards.Contains(cardInfo))
        {
            playerCards.Add(cardInfo);
        }

        if (playerDrawCardGroup.ContainsKey(cardInfo))
        {
            playerDrawCardGroup[cardInfo]++;
        }
        else
        {
            playerDrawCardGroup.Add(cardInfo,1);
        }
    }

    public int GetDarwCardAmount()
    {
        return cardsToSpawn-currentCardCount;
    }
}
