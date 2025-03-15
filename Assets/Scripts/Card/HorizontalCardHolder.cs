using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

//NOTE::slot为卡槽,Card为卡牌,CardVisual为卡牌图片

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    private int cardsToSpawn ;
    public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    void Start()
    {
        cardsToSpawn = CardManager.instance.cardsToSpawn;
        for (int i = 0; i < cardsToSpawn; i++)
        {
            Instantiate(slotPrefab, transform);
            //NOTE::生成卡牌对象，卡牌槽和卡牌
            //NOTE::排列由Horizontal Layout Group实现
        }

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();//NOTE::将生成的卡牌加入List<>

        int cardCount = 0;

        foreach (Card card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);//NOTE::鼠标进入事件
            card.PointerExitEvent.AddListener(CardPointerExit);//NOTE::鼠标离开事件
            card.BeginDragEvent.AddListener(BeginDrag);//NOTE::拖拽事件
            card.EndDragEvent.AddListener(EndDrag);//NOTE::拖拽结束事件
            card.name = cardCount.ToString();
            cardCount++;
        }
        StartCoroutine(Frame());
        CardManager.instance.cardCount -= cardsToSpawn;
    }


    public void DrawCard()
    {
        int cardCount = cards.Count;
        Card card= Instantiate(slotPrefab, transform).GetComponentInChildren<Card>();
        //NOTE::生成卡牌对象，卡牌槽和卡牌
        //NOTE::排列由Horizontal Layout Group实现
        
        card.PointerEnterEvent.AddListener(CardPointerEnter);//NOTE::鼠标进入事件
        card.PointerExitEvent.AddListener(CardPointerExit);//NOTE::鼠标离开事件
        card.BeginDragEvent.AddListener(BeginDrag);//NOTE::拖拽事件
        card.EndDragEvent.AddListener(EndDrag);//NOTE::拖拽结束事件
        card.name = cardCount.ToString();
        cardCount++;

        cards.Add(card);
        StartCoroutine(Frame());
    }
    IEnumerator Frame()
    {
        yield return new WaitForSecondsRealtime(.1f);
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardVisual != null)
                cards[i].cardVisual.UpdateIndex(transform.childCount);
            //NOTE::设置卡牌图片索引为卡槽所在索引
        }
    }
    
    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        //NOTE::transform.DOLocalMove为DOTween插件里实现更好移动动画效果的函数
        if(card.transform.position.x<CardManager.instance.useCardCheckPoint.transform.position.x
           || card.transform.position.y<CardManager.instance.useCardCheckPoint.transform.position.y
           || EnemyManager.instance.targetEnemy==null)
        {
            selectedCard.transform.DOLocalMove(
                selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero,
                tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);
        }
        
        //NOTE::如果 selectedCard.selected 为 true，卡片会移动到 (0, selectionOffset, 0) 的位置。如果为 false，卡片会返回到原点 (0, 0, 0)。
        //NOTE::如果 tweenCardReturn 为 true，动画持续时间为 0.15 秒。如果为 false，动画瞬间完成（持续时间为 0）。
        
        //NOTE::卡槽略微晃动
        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void Update()
    {
        //NOTE::测试使用手动删除鼠标选择卡牌
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                Destroy(hoveredCard.transform.parent.gameObject);
                cards.Remove(hoveredCard);
                return;
            }
        }

        //NOTE::点击卡牌位置上移
        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;
        
        //NOTE::如果正在拖动卡牌且位置互换未完成则执行卡牌位置互换。
        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    //NOTE::卡牌替换实现
    void Swap(int index)
    {
        isCrossing = true;

        //NOTE::拖动的是卡牌而不是卡槽，将卡牌放置的卡槽位置交换即可实现位置交换
        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        //NOTE::根据交换位置播放卡牌图片偏转动画
        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //NOTE:: Updated Visual Indexes  更新卡牌图片索引
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

}
