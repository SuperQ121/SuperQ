using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public Transform useCardCheckPoint;
    
    public List<CardInfo> cards = new List<CardInfo>(); 
    [SerializeField] public Dictionary<uint, CardInfo> cardGroup = new Dictionary<uint, CardInfo>();
    
    public List<CardInfo> playerCards = new List<CardInfo>();
    [SerializeField]public Dictionary<uint,CardInfo> playerCardGroup = new Dictionary<uint, CardInfo>();
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
    }
}
